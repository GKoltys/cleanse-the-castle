public class MapData
{

	public readonly int width;
	public readonly int height;
	public TileType[,] tiles;

	public MapData(int w, int h)
	{
		width = w; height = h;
		tiles = new TileType[w, h];
	}

	public bool InBounds(int x, int y) =>
		x >= 0 && y >= 0 && x < width && y < height;
}
