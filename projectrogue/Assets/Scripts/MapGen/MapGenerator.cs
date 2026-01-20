using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class MapGenerator : MonoBehaviour
{

    [SerializeField] private Transform player;

    [SerializeField] private int width = 25;
    [SerializeField] private int height = 25;
    [SerializeField] private int padding = 3;

    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap wallTilemap;

    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase wallTile;

    [SerializeField] private Transform entitiesRoot;
    [SerializeField] private SpawnTable[] spawnEntries;

    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 0.35f;

    [SerializeField] private CameraController cameraClamp;

    private MapData currentMap;
    private bool isTransitioning;

    private void Start()
    {
        BuildFloor();
    }

    private void BuildFloor()
    {
        currentMap = GenerateRoom(width, height, padding);
        Render(currentMap);
        PlacePlayerInCenter(currentMap);
        PlacePrefabs(currentMap);
        cameraClamp.SetBounds(0, width, 0, height);
    }

    public void GoToNextFloor()
    {
        if (!isTransitioning)
            StartCoroutine(NextFloorRoutine());
    }

    private IEnumerator NextFloorRoutine()
    {
        isTransitioning = true;

        yield return FadeOut();

        BuildFloor();

        yield return FadeIn();

        isTransitioning = false;
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
            for (int y = yMin; y <= yMax; y++)
                map.tiles[x, y] = TileType.Floor;

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
                            map.tiles[nx, ny] = TileType.Wall;
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

    private void PlacePlayerInCenter(MapData map)
    {
        int x = map.width / 2;
        int y = map.height / 2;

        // Center player in the tile
        player.position = new Vector3(x + 0.5f, y + 0.5f, 0f);
    }

    // https://docs.unity3d.com/2020.3/Documentation/Manual/InstantiatingPrefabs.html
    private void PlacePrefabs(MapData map)
    {
        // for clearing previous level entities
        ClearEntitiesRoot();

        var floors = CollectFloorTiles(map);
        if (floors.Count == 0) return;

        Vector2 playerPos = new Vector2(player.position.x, player.position.y);

        foreach (var entry in spawnEntries)
        {
            if (entry == null || entry.prefab == null || entry.count <= 0) continue;

            int n = Mathf.Min(entry.count, floors.Count);

            for (int i = 0; i < n; i++)
            {
                Vector2Int tile = PickTile(floors, playerPos, entry.minDistanceFromPlayer, attempts: 30);
                Vector3 world = new Vector3(tile.x + 0.5f, tile.y + 0.5f, 0f);

                var go = Instantiate(entry.prefab, world, Quaternion.identity, entitiesRoot);

                var initializables = go.GetComponentsInChildren<IMapGenInit>();
                foreach (var init in initializables)
                {
                    init.Init(this);
                }


                if (entry.uniqueTile)
                    floors.Remove(tile);
            }

        }
    }

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

    private void ClearEntitiesRoot()
    {
        if (entitiesRoot == null) return;
        for (int i = entitiesRoot.childCount - 1; i >= 0; i--)
            Destroy(entitiesRoot.GetChild(i).gameObject);
    }


    private IEnumerator FadeOut()
    {
        if (fadeCanvasGroup == null) yield break;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f;
    }

    private IEnumerator FadeIn()
    {
        if (fadeCanvasGroup == null) yield break;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = 1f - Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 0f;
    }


}
