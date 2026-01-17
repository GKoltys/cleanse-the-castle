using UnityEngine;
using UnityEngine.Tilemaps;

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

	private void Start()
	{
		var map = GenerateRoom(width, height, padding);
		Render(map);
        PlacePlayerInCenter(map);
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
		//floorTilemap.ClearAllTiles();
		//wallTilemap.ClearAllTiles();

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

}
