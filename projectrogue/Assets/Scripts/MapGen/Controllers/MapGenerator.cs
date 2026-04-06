using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{

    [SerializeField] private Transform player;
    [SerializeField] private PlayerBase playerBase;

    [SerializeField] private int width = 50;
    [SerializeField] private int height = 50;
    [SerializeField] private int padding = 3;
    [SerializeField] private int bossWidth = 25;
    [SerializeField] private int bossHeight = 25;
    [SerializeField] private int bspMaxDepth = 4; // amount of times the map size can be cut
    [SerializeField] private int minLeafSize = 10;
    [SerializeField] private int minRoomSize = 5;
    [SerializeField] private int maxRoomSize = 12;
    [SerializeField] private int corridorWidth = 1;
    [SerializeField] private int seed;

    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private Tilemap decorTilemap;
    [SerializeField] private Tilemap colliderDecorTilemap;

    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase floorTileFire;
    [SerializeField] private TileBase wallTile;
    [SerializeField] private TileBase wallTileFire;
    [SerializeField] private TileBase colliderDecorTile;
    [SerializeField] private TileBase colliderDecorTileFire;
    [SerializeField] private TileBase[] decorTiles;
    [SerializeField] private TileBase[] decorTilesFire;
    [SerializeField] private TileBase[] decorWallTiles;
    [SerializeField] private GameObject[] decorPrefabs;
    [SerializeField] private GameObject[] bossPrefabs;
    [SerializeField] private GameObject stairsPrefab;

    [SerializeField] private float decorChance;
    [SerializeField] private float colliderDecorChance;

    [SerializeField] private Transform entitiesRoot;
    [SerializeField] private Transform decorRoot;
    public Transform EntitiesRoot => entitiesRoot;

    [SerializeField] private SpawnTable[] spawnEntries;

    [SerializeField] private StartingAreaCameraClamp cameraClamp;

    private MapData currentMap;
    private int bossFloorThreshold = 10;
    private int floorFireThreshold = 15;
    private GameObject currentBoss;
    private Vector2Int currentBossTile;
    private bool bossStairsSpawned;
    private GameObject currentStairs;
    private int floorNumber;

    private void Start()
    {
        floorNumber = playerBase.GetFloorCount;
        BuildFloor();
    }

    private void BuildFloor()
    {
        // for clearing previous level entities
        ClearEntitiesRoot();
        ClearDecorRoot();

        bool isBoss = IsBossFloor(floorNumber);

        bossStairsSpawned = false;
        currentStairs = null;

        // choose whether to make boss or dungeon floor
        if (isBoss)
        {
            BuildBossFloor(floorNumber, isBoss);
        } else
        {
            BuildDungeonFloor(isBoss);
        }

        // Clamping camera after map is generated
        cameraClamp.SetBoundsAfterGeneration(currentMap.width, currentMap.height);

        // Set the new floor count and SaveGame()
        playerBase.SetFloorCount(floorNumber);
        SaveController.Instance.SaveGame();
        DungeonBgmController.Instance.ChangeMusic(floorNumber);
    }

    private void BuildDungeonFloor(bool isBoss)
    {
        int floorSeed = seed;
        floorSeed = Random.Range(int.MinValue, int.MaxValue);
        currentMap = ProceduralGenerator.GenerateFloor(width, height, padding,
            bspMaxDepth, minLeafSize, minRoomSize, maxRoomSize, floorSeed, corridorWidth);
        Render(currentMap, isBoss);

        var freeFloors = CollectFloorTiles(currentMap);

        // choose safe player spawn tile
        Vector2Int playerPos = PickPlayerSpawnTile(freeFloors);

        // reserve tile so nothing spawns there later
        freeFloors.Remove(playerPos);

        // place prefabs on map
        PlacePrefabs(freeFloors, playerPos);

        // place player position
        PlacePlayer(playerPos);

    }

    private void BuildBossFloor(int floorNumber, bool isBoss)
    {
        currentMap = BossFloorGenerator.GenerateBossRoom(bossWidth, bossHeight, padding);
        Render(currentMap, isBoss);

        Vector2Int bossTile = BossFloorGenerator.GetBossSpawnTile(currentMap);
        Vector2Int playerTile = BossFloorGenerator.GetPlayerSpawnTile(currentMap, padding);

        currentBossTile = bossTile;

        PlacePlayer(playerTile);
        PlaceBoss(floorNumber, bossTile);
        SpawnStairsNearBoss();
    }

    private void PlaceBoss(int floorNumber, Vector2Int bossPos)
    {
        GameObject boss = BossFloorGenerator.GetBossForFloor(bossPrefabs, floorNumber, bossFloorThreshold);

        Vector3 world = new Vector3(bossPos.x + 0.5f, bossPos.y + 0.5f, 0f);

        currentBoss = Instantiate(
            boss,
            world,
            Quaternion.identity,
            entitiesRoot
        );

        var initializables = currentBoss.GetComponentsInChildren<IMapGenInit>();

        foreach (var init in initializables)
        {
            init.Init(this);
        }

    }

    public void GoToNextFloor()
    {
        floorNumber++;
        StartCoroutine(NextFloorTransitionRoutine());
    }

    private IEnumerator NextFloorTransitionRoutine()
    {

        if (FadeUIController.Instance != null)
            yield return FadeUIController.Instance.FadeOut();

        BuildFloor();

        yield return null;

        if (FadeUIController.Instance != null)
            yield return FadeUIController.Instance.FadeIn();
    }

    private void Render(MapData map, bool isBoss)
    {
        // for clearing previous generated map's tiles
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
        decorTilemap.ClearAllTiles();
        colliderDecorTilemap.ClearAllTiles();

        // get reference to player floor count
        float floors = playerBase.GetFloorCount;

        TileBase currentFloorTile;
        TileBase currentWallTile;
        TileBase[] currentDecorTile;
        TileBase currentColliderDecorTile;

        // change floor/wall tiles depending on floor count
        if (floors < floorFireThreshold)
        {
            currentFloorTile = floorTile;
            currentWallTile = wallTile;
            currentDecorTile = decorTiles;
            currentColliderDecorTile = colliderDecorTile;
        }
        else
        {
            currentFloorTile = floorTileFire;
            currentWallTile = wallTileFire;
            currentDecorTile = decorTilesFire;
            currentColliderDecorTile = colliderDecorTileFire;
        }

        // for each tile sets to which TileType it is
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                var pos = new Vector3Int(x, y, 0);
                switch (map.tiles[x, y])
                {
                    case TileType.Floor:
                        floorTilemap.SetTile(pos, currentFloorTile);
                        // check for boss floor
                        if (isBoss)
                        {
                            // surround boss arena with collider decor on floor tiles next to walls
                            if (IsBossArenaBorder(map, x, y))
                            {
                                colliderDecorTilemap.SetTile(pos, currentColliderDecorTile);
                            }
                        }
                        else
                        {
                            // randomly place decor on some floor tiles
                            if (currentDecorTile.Length > 0 && Random.value < decorChance)
                            {
                                TileBase randomDecor = currentDecorTile[Random.Range(0, currentDecorTile.Length)];
                                decorTilemap.SetTile(pos, randomDecor);
                            }
                            if (Random.value < colliderDecorChance)
                            {
                                colliderDecorTilemap.SetTile(pos, currentColliderDecorTile);
                            }
                        }

                        break;
                    case TileType.Wall:
                        wallTilemap.SetTile(pos, currentWallTile);
                        // check for boss floor
                        if (!isBoss)
                        {
                            // random place decor or prefab on some wall tiles
                            if (IsWallFace(map, x, y))
                            {
                                float roll = Random.value;

                                if (roll < decorChance && decorWallTiles.Length > 0)
                                {
                                    TileBase decor =
                                        decorWallTiles[Random.Range(0, decorWallTiles.Length)];

                                    decorTilemap.SetTile(pos, decor);
                                }
                                else if (roll < decorChance * 2f && decorPrefabs.Length > 0)
                                {
                                    SpawnDecorPrefab(pos);
                                }
                            }
                        }
                        break;
                }
            }
        }
    }

    // set player to given position
    private void PlacePlayer(Vector2Int pos)
    {
        player.position = new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0f);
    }

    // randomly place different types of prefabs across the generated level
    // https://docs.unity3d.com/2020.3/Documentation/Manual/InstantiatingPrefabs.html
    private void PlacePrefabs(List<Vector2Int> floors, Vector2Int playerTile)
    {
        // collect tiles from map that are set to floor
        if (floors.Count == 0) return;

        // get player position
        Vector2 playerPos = new Vector2(playerTile.x + 0.5f, playerTile.y + 0.5f);

        // loop through each prefab
        foreach (var entry in spawnEntries)
        {
            if (entry == null || entry.prefab == null || entry.count <= 0) continue;

            // get the count of spawnable prefabs, compare to floor type count in case it exceeds it
            int n = Mathf.Min(entry.count, floors.Count);

            // guarantee at least one instance of a spawn entry
            int spawned = 0;

            for (int i = 0; i < n; i++)
            {
                // increase/decrease spawnchance of spawn entry
                float chance = entry.GetSpawnChance(playerBase.GetFloorCount);

                // roll chance for this instance of the object
                if (Random.value > chance)
                {
                    continue;
                }

                // pick a random floor tile using min distance from player
                Vector2Int tile = PickTile(floors, playerPos, entry.minDistanceFromPlayer, attempts: 30);

                // instantiate the prefab
                SpawnAtTile(entry, tile, floors);

                // remove the tile so nothing else can be spawned
                if (entry.uniqueTile)
                {
                    floors.Remove(tile);
                }

                // if an object with a spawnAlso element spawned, add to list
                if (entry.spawnAlso != null)
                { 
                    // pick another free tile
                    Vector2Int extraTile = PickTile(floors, playerPos, entry.minDistanceFromPlayer, attempts: 30);
                    Vector3 world = new Vector3(extraTile.x + 0.5f, extraTile.y + 0.5f, 0f);
                    // instantiate prefab
                    var go = Instantiate(entry.spawnAlso, world, Quaternion.identity, entitiesRoot);

                    // initialize components
                    var initializables = go.GetComponentsInChildren<IMapGenInit>();
                    foreach (var init in initializables)
                    {
                        init.Init(this);
                    }
       

                    // remove tile
                    floors.Remove(extraTile);
                }

                spawned++;
            }

            // guarantee one spawn
            if (spawned == 0 && entry.count > 0 && entry.spawnChance > 0f && entry.guaranteeSpawn)
            {
                // pick tile
                Vector2Int tile = PickTile(floors, playerPos, entry.minDistanceFromPlayer, attempts: 30);
                // instantiate the prefab
                SpawnAtTile(entry, tile, floors);
            }
        }
    }

    // make a list of tiles that are floor type
    private List<Vector2Int> CollectFloorTiles(MapData map)
    {
        var floors = new List<Vector2Int>(map.width * map.height);
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                if (map.tiles[x, y] != TileType.Floor)
                    continue;

                var cellPos = new Vector3Int(x, y, 0);
                // don't spawn prefabs on decor tiles
                if (decorTilemap.HasTile(cellPos))
                    continue;

                if (colliderDecorTilemap.HasTile(cellPos))
                    continue;

                floors.Add(new Vector2Int(x, y));
            }

        }    
        return floors;
    }

    // chose a random floor tile with min distance from player
    private Vector2Int PickTile(List<Vector2Int> floors, Vector2 playerPos, float minDist, int attempts)
    {
        Vector2Int chosen = floors[Random.Range(0, floors.Count)];
        if (minDist <= 0f) return chosen;

        for (int i = 0; i < attempts; i++)
        {
            var c = floors[Random.Range(0, floors.Count)];
            float d = Vector2.Distance(new Vector2(c.x + 0.5f, c.y + 0.5f), playerPos);
            if (d >= minDist) return c;
        }

        return chosen;
    }

    private void SpawnAtTile(SpawnTable entry, Vector2Int tile, List<Vector2Int> floors)
    {
        Vector3 world = new Vector3(tile.x + 0.5f, tile.y + 0.5f, 0f);
        // instantiate prefab
        var go = Instantiate(entry.prefab, world, Quaternion.identity, entitiesRoot);

        // initialize components
        var initializables = go.GetComponentsInChildren<IMapGenInit>();
        foreach (var init in initializables)
        {
            init.Init(this);
        }


        if (entry.uniqueTile)
        {
            floors.Remove(tile);
        }

    }

    // remove entities/prefabs from the map
    private void ClearEntitiesRoot()
    {
        if (entitiesRoot == null) return;
        for (int i = entitiesRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(entitiesRoot.GetChild(i).gameObject);
        }
    }

    // check if wall has floor below to place decor
    private bool IsWallFace(MapData map, int x, int y)
    {
        if (map.tiles[x, y] != TileType.Wall)
            return false;

        // visible wall face if floor is below
        if (y > 0 && map.tiles[x, y - 1] == TileType.Floor)
            return true;

        return false;
    }

    private void ClearDecorRoot()
    {
        if (decorRoot == null) return;

        for (int i = decorRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(decorRoot.GetChild(i).gameObject);
        }
    }

    private void SpawnDecorPrefab(Vector3Int pos)
    {
        if (decorPrefabs.Length == 0)
            return;

        GameObject prefab =
            decorPrefabs[Random.Range(0, decorPrefabs.Length)];

        Vector3 worldPos =
            wallTilemap.GetCellCenterWorld(pos) +
            new Vector3(0f, -0.2f, 0f);

        Instantiate(prefab, worldPos, Quaternion.identity, decorRoot);
    }

    private Vector2Int PickPlayerSpawnTile(List<Vector2Int> freeFloors)
    {
        if (freeFloors == null || freeFloors.Count == 0)
            return new Vector2Int(0, 0);

        return freeFloors[Random.Range(0, freeFloors.Count)];
    }

    // check floor count so every 10 is a boss room
    private bool IsBossFloor(int nextFloorNumber)
    {
        return nextFloorNumber % bossFloorThreshold == 0 && nextFloorNumber != 0;
    }

    // called by boss enemy
    public void OnBossDied()
    {
        if (bossStairsSpawned)
            return;

        bossStairsSpawned = true;
        if (currentStairs != null)
        {
            var stairs = currentStairs.GetComponentInChildren<StairsController>();
            if (stairs != null)
            {
                stairs.Unlock();
            }
        }
    }

    private void SpawnStairsNearBoss()
    {
        if (stairsPrefab == null)
        {
            Debug.LogWarning("No stairs prefab assigned.");
            return;
        }

        var freeFloors = CollectFloorTiles(currentMap);

        if (freeFloors == null || freeFloors.Count == 0)
        {
            Debug.LogWarning("No valid tiles available for stairs spawn.");
            return;
        }

        // don't place stairs directly on boss tile
        freeFloors.Remove(currentBossTile);

        Vector2 bossWorldPos = new Vector2(currentBossTile.x + 0.5f, currentBossTile.y + 0.5f);

        // pick spawn for stairs
        Vector2Int stairsTile = BossFloorGenerator.PickTileNear(freeFloors, bossWorldPos, 3f, 5f);

        Vector3 world = new Vector3(stairsTile.x + 0.5f, stairsTile.y + 0.5f, 0f);
        currentStairs = Instantiate(
          stairsPrefab,
          world,
          Quaternion.identity,
          entitiesRoot
        );

        var initializables = currentStairs.GetComponentsInChildren<IMapGenInit>();

        // initialize prefab
        foreach (var init in initializables)
        {
            init.Init(this);
        }

        // lock stairs
        var stairs = currentStairs.GetComponentInChildren<StairsController>();
        if (stairs != null)
        {
            stairs.Lock();
        }
    }

    private bool IsBossArenaBorder(MapData map, int x, int y)
    {
        if (map.tiles[x, y] != TileType.Floor)
            return false;

        // floor tile touching any wall tile
        if (x > 0 && map.tiles[x - 1, y] == TileType.Wall) return true;
        if (x < map.width - 1 && map.tiles[x + 1, y] == TileType.Wall) return true;
        if (y > 0 && map.tiles[x, y - 1] == TileType.Wall) return true;
        if (y < map.height - 1 && map.tiles[x, y + 1] == TileType.Wall) return true;

        return false;
    }

}
