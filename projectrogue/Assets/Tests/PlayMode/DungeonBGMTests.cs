using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class DungeonBGMTests
{
    // resetting test environment
    [TearDown]
    public void TearDown()
    {
        foreach (GameObject obj in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            Object.DestroyImmediate(obj);
        }

        typeof(DungeonBgmController)
            .GetProperty("Instance")
            .SetValue(null, null);
    }

    // asserts that floor 0 selects the starting area background music clip
    [Test]
    public void DungeonBgmController_ChangeMusic_FloorZeroUsesStartingArea()
    {
        DungeonBgmController controller = CreateDungeonBgmController(out AudioSource audioSource);
        AudioClip clip = CreateClip("Starting Area");

        SetPrivateField(controller, "startingArea", clip);

        controller.ChangeMusic(0);

        Assert.AreEqual(clip, audioSource.clip);
    }

    // https://docs.nunit.org/articles/nunit/writing-tests/attributes/testcase.html
    // asserts that boss floors 10 and 20 select the boss battle music clip
    [TestCase(10)]
    [TestCase(20)]
    public void DungeonBgmController_ChangeMusic_BossFloorsUseBossBattle(int floor)
    {
        DungeonBgmController controller = CreateDungeonBgmController(out AudioSource audioSource);
        AudioClip clip = CreateClip("Boss Battle");

        SetPrivateField(controller, "bossBattle", clip);

        controller.ChangeMusic(floor);

        Assert.AreEqual(clip, audioSource.clip);
    }

    // asserts that floor 30 selects the final battle music clip
    [Test]
    public void DungeonBgmController_ChangeMusic_FloorThirtyUsesFinalBattle()
    {
        DungeonBgmController controller = CreateDungeonBgmController(out AudioSource audioSource);
        AudioClip clip = CreateClip("Final Battle");

        SetPrivateField(controller, "finalBattle", clip);

        controller.ChangeMusic(30);

        Assert.AreEqual(clip, audioSource.clip);
    }

    // asserts that floors below 16 select the water dungeon music clip
    [TestCase(1)]
    [TestCase(5)]
    [TestCase(15)]
    public void DungeonBgmController_ChangeMusic_FloorsBelowSixteenUseWaterDungeon(int floor)
    {
        DungeonBgmController controller = CreateDungeonBgmController(out AudioSource audioSource);
        AudioClip clip = CreateClip("Water Dungeon");

        SetPrivateField(controller, "waterDungeon", clip);

        controller.ChangeMusic(floor);

        Assert.AreEqual(clip, audioSource.clip);
    }

    // asserts that floors from 16 to 29 select the lava dungeon music clip
    [TestCase(16)]
    [TestCase(21)]
    [TestCase(29)]
    public void DungeonBgmController_ChangeMusic_FloorsBelowThirtyUseLavaDungeon(int floor)
    {
        DungeonBgmController controller = CreateDungeonBgmController(out AudioSource audioSource);
        AudioClip clip = CreateClip("Lava Dungeon");

        SetPrivateField(controller, "lavaDungeon", clip);

        controller.ChangeMusic(floor);

        Assert.AreEqual(clip, audioSource.clip);
    }

    // asserts that PlayEndingSceneMusic sets the ending scene background music clip
    [Test]
    public void DungeonBgmController_PlayEndingSceneMusic_UsesEndingSceneClip()
    {
        DungeonBgmController controller = CreateDungeonBgmController(out AudioSource audioSource);
        AudioClip clip = CreateClip("Ending Scene");

        SetPrivateField(controller, "endingScene", clip);

        controller.PlayEndingSceneMusic();

        Assert.AreEqual(clip, audioSource.clip);
    }

    // helper functions
    private DungeonBgmController CreateDungeonBgmController(out AudioSource audioSource)
    {
        GameObject obj = new GameObject("Dungeon BGM Controller");

        audioSource = obj.AddComponent<AudioSource>();

        DungeonBgmController controller = obj.AddComponent<DungeonBgmController>();

        SetPrivateField(controller, "audioSource", audioSource);

        return controller;
    }

    private AudioClip CreateClip(string name)
    {
        AudioClip clip = AudioClip.Create(name, 44100, 1, 44100, false);
        clip.name = name;

        return clip;
    }

    private void SetPrivateField(object target, string fieldName, object value)
    {
        FieldInfo field = target.GetType().GetField(
            fieldName,
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        field.SetValue(target, value);
    }
}