using NUnit.Framework;
using UnityEngine;
using System.Reflection;
using UnityEngine.TestTools;
using System.Collections;

public class BSPFloorLogicTests
{
    // assert boss floors occur every 10 floors except floor 0
    [Test]
    public void IsBossFloor_ReturnsTrueEvery10FloorsExceptZero()
    {
        var generator = CreateGenerator();

        Assert.IsFalse(generator.IsBossFloor(0));
        Assert.IsFalse(generator.IsBossFloor(1));
        Assert.IsFalse(generator.IsBossFloor(9));

        Assert.IsTrue(generator.IsBossFloor(10));
        Assert.IsTrue(generator.IsBossFloor(20));
        Assert.IsTrue(generator.IsBossFloor(30));
    }


    // assert non-multiple-of-10 floors are not boss floors
    [Test]
    public void IsBossFloor_ReturnsFalseForNonBossFloors()
    {
        var generator = CreateGenerator();

        Assert.IsFalse(generator.IsBossFloor(3));
        Assert.IsFalse(generator.IsBossFloor(7));
        Assert.IsFalse(generator.IsBossFloor(15));
    }

    // https://discussions.unity.com/t/what-is-the-difference-between-unitytest-and-test/780827/2
    // coroutine test so it waits for Destroy() to occur
    // assert entitiesRoot children are removed when clearing entities
    [UnityTest]
    public IEnumerator ClearEntitiesRoot_RemovesAllChildren()
    {
        var generator = CreateGenerator();

        var entitiesRoot = new GameObject("EntitiesRoot").transform;

        new GameObject("Enemy1").transform.parent = entitiesRoot;
        new GameObject("Enemy2").transform.parent = entitiesRoot;
        new GameObject("Enemy3").transform.parent = entitiesRoot;

        SetPrivate(generator, "entitiesRoot", entitiesRoot);

        generator.ClearEntitiesRoot();

        yield return null;

        Assert.AreEqual(0, entitiesRoot.childCount);
    }


    // assert method safely handles null entitiesRoot
    [Test]
    public void ClearEntitiesRoot_WithNullRoot_DoesNothing()
    {
        var generator = CreateGenerator();

        SetPrivate(generator, "entitiesRoot", null);

        generator.ClearEntitiesRoot();

        Assert.Pass();
    }


    // helper functions
    private static MapGenerator CreateGenerator()
    {
        var obj = new GameObject("MapGenerator");
        var generator = obj.AddComponent<MapGenerator>();

        SetPrivate(generator, "buildOnStart", false);

        return generator;
    }

    private static void SetPrivate(object target, string fieldName, object value)
    {
        target.GetType()
            .GetField(fieldName,
            BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(target, value);
    }
}