using NUnit.Framework;

public class BSPTests
{
    // assert same bsp layout with same seed
    [Test]
    public void GenerateFloor_WithSameSeed_ProducesSameMap()
    {
        MapData mapA = ProceduralGenerator.GenerateFloor(
            w: 50,
            h: 50,
            pad: 2,
            bspMaxDepth: 4,
            minLeafSize: 8,
            minRoomSize: 4,
            maxRoomSize: 10,
            seed: 12345,
            corridorWidth: 1
        );

        MapData mapB = ProceduralGenerator.GenerateFloor(
            w: 50,
            h: 50,
            pad: 2,
            bspMaxDepth: 4,
            minLeafSize: 8,
            minRoomSize: 4,
            maxRoomSize: 10,
            seed: 12345,
            corridorWidth: 1
        );

        for (int x = 0; x < 50; x++)
        {
            for (int y = 0; y < 50; y++)
            {
                Assert.AreEqual(
                    mapA.tiles[x, y],
                    mapB.tiles[x, y]
                );
            }
        }
    }

    // assert floor tiles are generating correctly
    [Test]
    public void GenerateFloor_CreatesSomeFloorTiles()
    {
        MapData map = ProceduralGenerator.GenerateFloor(
            w: 50,
            h: 50,
            pad: 2,
            bspMaxDepth: 4,
            minLeafSize: 8,
            minRoomSize: 4,
            maxRoomSize: 10,
            seed: 12345,
            corridorWidth: 1
        );

        int floorCount = CountTiles(map, TileType.Floor);

        Assert.Greater(
            floorCount,
            0
        );
    }

    // assert padding is wall tiles
    [Test]
    public void GenerateFloor_OuterPaddingStaysWall()
    {
        int width = 50;
        int height = 50;
        int pad = 2;

        MapData map = ProceduralGenerator.GenerateFloor(
            w: width,
            h: height,
            pad: pad,
            bspMaxDepth: 4,
            minLeafSize: 8,
            minRoomSize: 4,
            maxRoomSize: 10,
            seed: 12345,
            corridorWidth: 1
        );

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < pad; y++)
            {
                Assert.AreEqual(TileType.Wall, map.tiles[x, y]);
                Assert.AreEqual(TileType.Wall, map.tiles[x, height - 1 - y]);
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < pad; x++)
            {
                Assert.AreEqual(TileType.Wall, map.tiles[x, y]);
                Assert.AreEqual(TileType.Wall, map.tiles[width - 1 - x, y]);
            }
        }
    }

    // assert map dimensions are correct
    [Test]
    public void GenerateFloor_MapHasCorrectSize()
    {
        MapData map = ProceduralGenerator.GenerateFloor(
            w: 50,
            h: 40,
            pad: 2,
            bspMaxDepth: 4,
            minLeafSize: 8,
            minRoomSize: 4,
            maxRoomSize: 10,
            seed: 12345,
            corridorWidth: 1
        );

        Assert.AreEqual(50, map.tiles.GetLength(0));
        Assert.AreEqual(40, map.tiles.GetLength(1));
    }

    private int CountTiles(MapData map, TileType tileType)
    {
        int count = 0;

        for (int x = 0; x < map.tiles.GetLength(0); x++)
        {
            for (int y = 0; y < map.tiles.GetLength(1); y++)
            {
                if (map.tiles[x, y] == tileType)
                {
                    count++;
                }
            }
        }

        return count;
    }
}