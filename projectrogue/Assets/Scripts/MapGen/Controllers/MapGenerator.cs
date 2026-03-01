using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class MapGenerator : MonoBehaviour
{

    [SerializeField] private Transform player;

    [SerializeField] private int width = 50;
    [SerializeField] private int height = 50;
    [SerializeField] private int padding = 3;
    [SerializeField] private int bspMaxDepth = 4; // amount of times the map size can be cut
    [SerializeField] private int minLeafSize = 10;
    [SerializeField] private int minRoomSize = 5;
    [SerializeField] private int maxRoomSize = 12;
    [SerializeField] private int corridorWidth = 1;
    [SerializeField] private int seed;

    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap wallTilemap;

    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase wallTile;

    [SerializeField] private Transform entitiesRoot;
    [SerializeField] private SpawnTable[] spawnEntries;

    [SerializeField] private StartingAreaCameraClamp cameraClamp;

    private MapData currentMap;
    private Vector2Int playerPos;
    //private bool isTransitioning;

    private void Start()
    {
        BuildFloor();
    }

    private void BuildFloor()
    {
        int floorSeed = seed;
        floorSeed = Random.Range(int.MinValue, int.MaxValue);
        //currentMap = GenerateRoom(width, height, padding);
        currentMap = ProceduralGenerator.GenerateFloor(width, height, padding,
            bspMaxDepth, minLeafSize, minRoomSize, maxRoomSize, floorSeed, corridorWidth, out playerPos);
        Render(currentMap);
        PlacePlayer(playerPos);
        PlacePrefabs(currentMap);
        
        // Clamping camera after map is generated
        cameraClamp.SetBoundsAfterGeneration(currentMap.width, currentMap.height);
    }

    public void GoToNextFloor()
    {
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


    private MapData GenerateRoom(int w, int h, int pad)
    {
        var map = new MapData(w, h);

        int xMin = pad;
        int yMin = pad;
        int xMax = w - pad - 1;
        int yMax = h - pad - 1;

        // create a room with tiles set to floor
        for (int x = xMin; x <= xMax; x++)
        {
            for (int y = yMin; y <= yMax; y++)
            {
                map.tiles[x, y] = TileType.Floor;
            }

        }
        // set any tiles adjacent to floor as walls
        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
            {
                if (map.tiles[x, y] != TileType.Floor) continue;

                for (int dx = -1; dx <= 1; dx++)
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        int nx = x + dx;
                        int ny = y + dy;
                        if (!map.InBounds(nx, ny)) continue;

                        if (map.tiles[nx, ny] == TileType.Empty)
                        {
                            map.tiles[nx, ny] = TileType.Wall;
                        }
                    }
            }

        // set padded boundary to wall
        for (int x = 0; x < w; x++)
        {
            if (map.tiles[x, 0] == TileType.Empty) map.tiles[x, 0] = TileType.Wall;
            if (map.tiles[x, h - 1] == TileType.Empty) map.tiles[x, h - 1] = TileType.Wall;
        }

        for (int y = 0; y < h; y++)
        {
            if (map.tiles[0, y] == TileType.Empty) map.tiles[0, y] = TileType.Wall;
            if (map.tiles[w - 1, y] == TileType.Empty) map.tiles[w - 1, y] = TileType.Wall;
        }

        return map;
    }

    private void Render(MapData map)
    {
        // for clearing previous generated map's tiles
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();

        // for each tile sets to which TileType it is
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                var pos = new Vector3Int(x, y, 0);
                switch (map.tiles[x, y])
                {
                    case TileType.Floor:
                        floorTilemap.SetTile(pos, floorTile);
                        break;
                    case TileType.Wall:
                        wallTilemap.SetTile(pos, wallTile);
                        break;
                }
            }
        }
    }

    private void PlacePlayerInCenter(MapData map)
    {
        int x = map.width / 2;
        int y = map.height / 2;

        // Center player in the tile
        player.position = new Vector3(x + 0.5f, y + 0.5f, 0f);
    }

    // set player to given position
    private void PlacePlayer(Vector2Int pos)
    {
        player.position = new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0f);
    }

    // randomly place different types of prefabs across the generated level
    // https://docs.unity3d.com/2020.3/Documentation/Manual/InstantiatingPrefabs.html
    private void PlacePrefabs(MapData map)
    {
        // for clearing previous level entities
        ClearEntitiesRoot();

        // collect tiles from map that are set to floor
        var floors = CollectFloorTiles(map);
        if (floors.Count == 0) return;

        // get player position
        Vector2 playerPos = new Vector2(player.position.x, player.position.y);

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
                // roll chance for this instance of the object
                if (Random.value > entry.spawnChance)
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
                if (map.tiles[x, y] == TileType.Floor)
                {
                    floors.Add(new Vector2Int(x, y));
                }
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

        // set item inside chest, for now it's the spawnAlso but could make a list of objects to randomly choose
        var chest = go.GetComponent<LockedChestController>();
        if (chest != null && entry.spawnAlso != null)
        {
            chest.SetDrop(entry.spawnAlso);
        }

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

}
