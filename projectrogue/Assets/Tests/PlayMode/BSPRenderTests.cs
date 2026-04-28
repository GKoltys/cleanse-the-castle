using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Reflection;

public class BSPRenderTests
{
    // assert the floor and wall tiles are rendered correctly
    [Test]
    public void Render_GivenMapData_PaintsFloorAndWallTiles()
    {
        var context = CreateRenderContext();

        MapData map = CreateSimpleMap();

        context.Generator.Render(map, false, 1);

        Assert.AreEqual(context.NormalFloorTile, context.FloorTilemap.GetTile(new Vector3Int(0, 0, 0)));
        Assert.AreEqual(context.NormalWallTile, context.WallTilemap.GetTile(new Vector3Int(1, 0, 0)));
        Assert.AreEqual(context.NormalWallTile, context.WallTilemap.GetTile(new Vector3Int(0, 1, 0)));
        Assert.AreEqual(context.NormalFloorTile, context.FloorTilemap.GetTile(new Vector3Int(1, 1, 0)));
    }

    // assert floor and wall tiles on floorCount 15 and over are fire variants
    [Test]
    public void Render_WhenFloorIsPastFireThreshold_UsesFireTileVariants()
    {
        var context = CreateRenderContext();

        MapData map = CreateSimpleMap();

        context.Generator.Render(map, false, 15);

        Assert.AreEqual(context.FireFloorTile, context.FloorTilemap.GetTile(new Vector3Int(0, 0, 0)));
        Assert.AreEqual(context.FireWallTile, context.WallTilemap.GetTile(new Vector3Int(1, 0, 0)));
    }

    // assert old tiles are removed when another map is rendered
    [Test]
    public void Render_WhenCalledAgain_ClearsPreviousTiles()
    {
        var context = CreateRenderContext();

        var firstMap = new MapData(2, 2);
        firstMap.tiles[0, 0] = TileType.Floor;
        firstMap.tiles[1, 0] = TileType.Floor;
        firstMap.tiles[0, 1] = TileType.Floor;
        firstMap.tiles[1, 1] = TileType.Floor;

        context.Generator.Render(firstMap, false, 1);

        Assert.AreEqual(
            context.NormalFloorTile,
            context.FloorTilemap.GetTile(new Vector3Int(1, 1, 0))
        );

        var secondMap = new MapData(2, 2);
        secondMap.tiles[0, 0] = TileType.Wall;
        secondMap.tiles[1, 0] = TileType.Wall;
        secondMap.tiles[0, 1] = TileType.Wall;
        secondMap.tiles[1, 1] = TileType.Wall;

        context.Generator.Render(secondMap, false, 1);

        Assert.IsNull(context.FloorTilemap.GetTile(new Vector3Int(1, 1, 0)));
        Assert.AreEqual(
            context.NormalWallTile,
            context.WallTilemap.GetTile(new Vector3Int(1, 1, 0))
        );
    }


    // helper functions
    private static RenderTestContext CreateRenderContext()
    {
        var mapObj = new GameObject("MapGenerator");
        var generator = mapObj.AddComponent<MapGenerator>();

        SetPrivate(generator, "buildOnStart", false);

        var gridObj = new GameObject("Grid");
        gridObj.AddComponent<Grid>();

        var floorTilemap = CreateTilemap("FloorTilemap", gridObj.transform);
        var wallTilemap = CreateTilemap("WallTilemap", gridObj.transform);
        var decorTilemap = CreateTilemap("DecorTilemap", gridObj.transform);
        var colliderDecorTilemap = CreateTilemap("ColliderDecorTilemap", gridObj.transform);

        var normalFloorTile = ScriptableObject.CreateInstance<Tile>();
        var normalWallTile = ScriptableObject.CreateInstance<Tile>();
        var fireFloorTile = ScriptableObject.CreateInstance<Tile>();
        var fireWallTile = ScriptableObject.CreateInstance<Tile>();

        SetPrivate(generator, "floorTilemap", floorTilemap);
        SetPrivate(generator, "wallTilemap", wallTilemap);
        SetPrivate(generator, "decorTilemap", decorTilemap);
        SetPrivate(generator, "colliderDecorTilemap", colliderDecorTilemap);

        SetPrivate(generator, "floorTile", normalFloorTile);
        SetPrivate(generator, "wallTile", normalWallTile);
        SetPrivate(generator, "floorTileFire", fireFloorTile);
        SetPrivate(generator, "wallTileFire", fireWallTile);

        SetPrivate(generator, "decorTiles", new TileBase[0]);
        SetPrivate(generator, "decorTilesFire", new TileBase[0]);
        SetPrivate(generator, "decorWallTiles", new TileBase[0]);
        SetPrivate(generator, "decorPrefabs", new GameObject[0]);

        SetPrivate(generator, "decorChance", 0f);
        SetPrivate(generator, "colliderDecorChance", 0f);

        return new RenderTestContext
        {
            Generator = generator,
            FloorTilemap = floorTilemap,
            WallTilemap = wallTilemap,
            DecorTilemap = decorTilemap,
            ColliderDecorTilemap = colliderDecorTilemap,
            NormalFloorTile = normalFloorTile,
            NormalWallTile = normalWallTile,
            FireFloorTile = fireFloorTile,
            FireWallTile = fireWallTile
        };
    }

    private static Tilemap CreateTilemap(string name, Transform parent)
    {
        var obj = new GameObject(name);
        obj.transform.parent = parent;

        var tilemap = obj.AddComponent<Tilemap>();
        obj.AddComponent<TilemapRenderer>();

        return tilemap;
    }

    private static MapData CreateSimpleMap()
    {
        var map = new MapData(2, 2);

        map.tiles[0, 0] = TileType.Floor;
        map.tiles[1, 0] = TileType.Wall;
        map.tiles[0, 1] = TileType.Wall;
        map.tiles[1, 1] = TileType.Floor;

        return map;
    }

    private static void SetPrivate(object target, string fieldName, object value)
    {
        target.GetType()
            .GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(target, value);
    }

    private class RenderTestContext
    {
        public MapGenerator Generator;

        public Tilemap FloorTilemap;
        public Tilemap WallTilemap;
        public Tilemap DecorTilemap;
        public Tilemap ColliderDecorTilemap;

        public TileBase NormalFloorTile;
        public TileBase NormalWallTile;
        public TileBase FireFloorTile;
        public TileBase FireWallTile;
    }

}