using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class EnemyDropTests
{
    // verifies DropLoot spawns item under MapGenerator.EntitiesRoot
    [Test]
    public void DropLoot_WhenValidDropTable_SpawnsLootPrefab()
    {
        var mapGenerator = CreateMapGenerator(out var entitiesRoot);

        var dropper = CreateDropper(mapGenerator);

        var lootPrefab = new GameObject("CoinLoot");

        var dropTable = CreateDropTable(
            new DropEntry
            {
                itemPrefab = lootPrefab,
                weight = 1f
            },
            noDropWeight: 0f
        );

        SetPrivate(dropper, "dropTable", dropTable);

        dropper.DropLoot();

        Assert.AreEqual(1, entitiesRoot.childCount);
        Assert.AreEqual("CoinLoot(Clone)", entitiesRoot.GetChild(0).name);
    }


    // verifies no loot spawns when dropTable returns null
    [Test]
    public void DropLoot_WhenOnlyNoDropWeight_DropsNothing()
    {
        var mapGenerator = CreateMapGenerator(out var entitiesRoot);

        var dropper = CreateDropper(mapGenerator);

        var dropTable = CreateDropTable(null, noDropWeight: 1f);

        SetPrivate(dropper, "dropTable", dropTable);

        dropper.DropLoot();

        Assert.AreEqual(0, entitiesRoot.childCount);
    }


    // creates a MapGenerator with EntitiesRoot assigned
    private static MapGenerator CreateMapGenerator(out Transform entitiesRoot)
    {
        var mapObj = new GameObject("MapGenerator");

        var generator = mapObj.AddComponent<MapGenerator>();

        entitiesRoot = new GameObject("EntitiesRoot").transform;

        SetPrivate(generator, "entitiesRoot", entitiesRoot);
        SetPrivate(generator, "buildOnStart", false);

        return generator;
    }


    // creates EnemyLootDropper and initializes it
    private static EnemyLootDropper CreateDropper(MapGenerator generator)
    {
        var enemyObj = new GameObject("Enemy");

        var dropper = enemyObj.AddComponent<EnemyLootDropper>();

        dropper.Init(generator);

        return dropper;
    }


    // creates deterministic drop table for testing
    private static EnemyDropTable CreateDropTable(DropEntry entry, float noDropWeight)
    {
        var dropTable = ScriptableObject.CreateInstance<EnemyDropTable>();

        var list = entry != null
            ? new List<DropEntry> { entry }
            : new List<DropEntry>();

        SetPrivate(dropTable, "lootTable", list);
        SetPrivate(dropTable, "noDropWeight", noDropWeight);

        return dropTable;
    }

    [Test]
    public void Despawn_WhenEnemyHasLootDropper_DropsLoot()
    {
        var mapGenerator = CreateMapGenerator(out var entitiesRoot);

        var enemyObj = new GameObject("Enemy");
        var enemy = enemyObj.AddComponent<TestLootEnemy>();

        var dropper = enemyObj.AddComponent<EnemyLootDropper>();
        dropper.Init(mapGenerator);

        var lootPrefab = new GameObject("CoinLoot");

        var dropTable = CreateDropTable(
            new DropEntry
            {
                itemPrefab = lootPrefab,
                weight = 1f
            },
            noDropWeight: 0f
        );

        SetPrivate(dropper, "dropTable", dropTable);
        enemy.ForceLootDropper(dropper);

        enemy.Despawn();

        Assert.AreEqual(1, entitiesRoot.childCount);
        Assert.AreEqual("CoinLoot(Clone)", entitiesRoot.GetChild(0).name);
    }

    [Test]
    public void Despawn_CalledTwice_DropsLootOnlyOnce()
    {
        var mapGenerator = CreateMapGenerator(out var entitiesRoot);

        var enemyObj = new GameObject("Enemy");
        var enemy = enemyObj.AddComponent<TestLootEnemy>();

        var dropper = enemyObj.AddComponent<EnemyLootDropper>();
        dropper.Init(mapGenerator);

        var lootPrefab = new GameObject("CoinLoot");

        var dropTable = CreateDropTable(
            new DropEntry
            {
                itemPrefab = lootPrefab,
                weight = 1f
            },
            noDropWeight: 0f
        );

        SetPrivate(dropper, "dropTable", dropTable);
        enemy.ForceLootDropper(dropper);

        enemy.Despawn();
        enemy.Despawn();

        Assert.AreEqual(1, entitiesRoot.childCount);
    }

    private class TestLootEnemy : EnemyBase
    {
        public void ForceLootDropper(EnemyLootDropper dropper)
        {
            enemyLootDropper = dropper;
        }
    }


    // helper for assigning private serialized fields
    private static void SetPrivate(object target, string fieldName, object value)
    {
        target.GetType()
            .GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(target, value);
    }
}