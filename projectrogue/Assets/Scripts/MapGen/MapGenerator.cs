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
    [SerializeField] private GameObject stairsPrefab;
    [SerializeField] private int minDistanceFromPlayer = 10;

    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 0.35f;

    private MapData currentMap;
    private GameObject currentStairs;
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

    private void PlacePrefabs(MapData map)
    {
        // for clearing previous prefabs (just stairs atm)
        if (currentStairs != null) Destroy(currentStairs);

        // Collect floor tiles
        var floors = new List<Vector2Int>(map.width * map.height);
        for (int x = 0; x < map.width; x++)
            for (int y = 0; y < map.height; y++)
                if (map.tiles[x, y] == TileType.Floor)
                    floors.Add(new Vector2Int(x, y));

        // Pick random floor tile far enough from player
        Vector2 playerGrid = new Vector2(player.position.x, player.position.y);
        Vector2Int chosen = floors[Random.Range(0, floors.Count)];

        for (int i = 0; i < 30; i++)
        {
            var candidate = floors[Random.Range(0, floors.Count)];
            float dist = Vector2.Distance(new Vector2(candidate.x + 0.5f, candidate.y + 0.5f), playerGrid);
            if (dist >= minDistanceFromPlayer)
            {
                chosen = candidate;
                break;
            }
        }

        // Spawn stairs GameObject at the tile center
        Vector3 worldPos = new Vector3(chosen.x + 0.5f, chosen.y + 0.5f, 0f);
        currentStairs = Instantiate(stairsPrefab, worldPos, Quaternion.identity, entitiesRoot);

        // assign StairsController object to stairs var
        var stairs = currentStairs.GetComponent<StairsController>();
        if (stairs != null) stairs.Init(this);
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
