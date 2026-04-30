using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class BSPPlaceTests
{
    // assert a prefab was instantiated
    [Test]
    public void PlacePrefabs_WithGuaranteedSpawn_SpawnsPrefabUnderEntitiesRoot()
    {
        var context = CreatePlaceContext();

        var prefab = new GameObject("EnemyPrefab");
        var spawnEntry = CreateSpawnEntry(prefab);

        SetPrivate(context.Generator, "spawnEntries", new SpawnTable[] { spawnEntry });

        var floors = CreateFloors(new Vector2Int(5, 5), new Vector2Int(6, 6));

        context.Generator.PlacePrefabs(floors, Vector2Int.zero, 1);

        Assert.AreEqual(1, context.EntitiesRoot.childCount);
        Assert.AreEqual("EnemyPrefab(Clone)", context.EntitiesRoot.GetChild(0).name);
    }

    // assert a prefab does not spawn with 0 spawn chance
    [Test]
    public void PlacePrefabs_WhenSpawnChanceIsZero_DoesNotSpawn()
    {
        var context = CreatePlaceContext();

        var prefab = new GameObject("EnemyPrefab");
        var spawnEntry = CreateSpawnEntry(prefab);
        spawnEntry.spawnChance = 0f;
        spawnEntry.guaranteeSpawn = false;

        SetPrivate(context.Generator, "spawnEntries", new SpawnTable[] { spawnEntry });

        var floors = CreateFloors(new Vector2Int(5, 5));

        context.Generator.PlacePrefabs(floors, Vector2Int.zero, 1);

        Assert.AreEqual(0, context.EntitiesRoot.childCount);
    }

    // assert spawnentry does not spawn with null prefab
    [Test]
    public void PlacePrefabs_WhenSpawnEntryHasNullPrefab_DoesNotSpawn()
    {
        var context = CreatePlaceContext();

        var spawnEntry = CreateSpawnEntry(null);

        SetPrivate(context.Generator, "spawnEntries", new SpawnTable[] { spawnEntry });

        var floors = CreateFloors(new Vector2Int(5, 5));

        context.Generator.PlacePrefabs(floors, Vector2Int.zero, 1);

        Assert.AreEqual(0, context.EntitiesRoot.childCount);
    }

    // assert that unique tiles are removed after placing prefabs
    [Test]
    public void PlacePrefabs_WithUniqueTile_RemovesUsedTile()
    {
        var context = CreatePlaceContext();

        var prefab = new GameObject("EnemyPrefab");
        var spawnEntry = CreateSpawnEntry(prefab);

        SetPrivate(context.Generator, "spawnEntries", new SpawnTable[] { spawnEntry });

        var floors = CreateFloors(new Vector2Int(5, 5));

        context.Generator.PlacePrefabs(floors, Vector2Int.zero, 1);

        Assert.AreEqual(0, floors.Count);
    }

    // assert spawnAlso places another prefab
    [Test]
    public void PlacePrefabs_WithSpawnAlso_SpawnsExtraPrefab()
    {
        var context = CreatePlaceContext();

        var mainPrefab = new GameObject("EnemyPrefab");
        var extraPrefab = new GameObject("CoinPrefab");

        var spawnEntry = CreateSpawnEntry(mainPrefab);
        spawnEntry.spawnAlso = extraPrefab;

        SetPrivate(context.Generator, "spawnEntries", new SpawnTable[] { spawnEntry });

        var floors = CreateFloors(new Vector2Int(5, 5), new Vector2Int(6, 6));

        context.Generator.PlacePrefabs(floors, Vector2Int.zero, 1);

        Assert.AreEqual(2, context.EntitiesRoot.childCount);
    }

    // assert init is called on imapgeninit prefabs
    [Test]
    public void PlacePrefabs_CallsIMapGenInit_OnSpawnedPrefab()
    {
        TestMapGenInit.InitCalls = 0;

        var context = CreatePlaceContext();

        var prefab = new GameObject("EnemyPrefab");
        prefab.AddComponent<TestMapGenInit>();

        var spawnEntry = CreateSpawnEntry(prefab);

        SetPrivate(context.Generator, "spawnEntries", new SpawnTable[] { spawnEntry });

        var floors = CreateFloors(new Vector2Int(5, 5));

        context.Generator.PlacePrefabs(floors, Vector2Int.zero, 1);

        Assert.AreEqual(1, TestMapGenInit.InitCalls);
    }

    // helper functions
    private static PlaceTestContext CreatePlaceContext()
    {
        var mapObj = new GameObject("MapGenerator");
        var generator = mapObj.AddComponent<MapGenerator>();

        SetPrivate(generator, "buildOnStart", false);

        var entitiesRoot = new GameObject("EntitiesRoot").transform;
        SetPrivate(generator, "entitiesRoot", entitiesRoot);

        return new PlaceTestContext
        {
            Generator = generator,
            EntitiesRoot = entitiesRoot
        };
    }

    private static SpawnTable CreateSpawnEntry(GameObject prefab)
    {
        return new SpawnTable
        {
            prefab = prefab,
            count = 1,
            spawnChance = 1f,
            guaranteeSpawn = true,
            uniqueTile = true,
            minDistanceFromPlayer = 0f
        };
    }

    private static List<Vector2Int> CreateFloors(params Vector2Int[] tiles)
    {
        return new List<Vector2Int>(tiles);
    }

    private static void SetPrivate(object target, string fieldName, object value)
    {
        target.GetType()
            .GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(target, value);
    }

    private class PlaceTestContext
    {
        public MapGenerator Generator;
        public Transform EntitiesRoot;
    }
}

public class TestMapGenInit : MonoBehaviour, IMapGenInit
{
    public static int InitCalls;

    public void Init(MapGenerator controller)
    {
        InitCalls++;
    }
}