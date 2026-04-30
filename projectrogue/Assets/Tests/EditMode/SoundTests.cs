using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public class SoundTests
{
    // https://docs.nunit.org/articles/nunit/writing-tests/attributes/teardown.html
    // resetting test environment
    [TearDown]
    public void TearDown()
    {
        foreach (GameObject obj in Object.FindObjectsByType<GameObject>(
                     FindObjectsSortMode.None))
        {
            Object.DestroyImmediate(obj);
        }
    }

    // asserts that requesting a valid sound group returns a clip from that group
    [Test]
    public void SoundEffectLibrary_GetRandomClip_ReturnsClipFromCorrectGroup()
    {

        SoundEffectLibrary library = CreateSoundEffectLibrary();

        AudioClip coinClip = CreateClip("Coin Clip");

        SoundEffectGroup coinGroup = new SoundEffectGroup
        {
            name = SoundGroupName.COIN,
            audioClips = new List<AudioClip> { coinClip }
        };

        SetPrivateField(library, "soundEffectGroups", new[] { coinGroup });

        InvokePrivateMethod(library, "InitialiseDictionary");

        AudioClip result = library.GetRandomClip(SoundGroupName.COIN);

        Assert.AreEqual(coinClip, result);
    }


    // asserts that requesting a sound group that does not exist returns null
    [Test]
    public void SoundEffectLibrary_GetRandomClip_ReturnsNullWhenGroupDoesNotExist()
    {

        SoundEffectLibrary library = CreateSoundEffectLibrary();

        SetPrivateField(library, "soundEffectGroups", new SoundEffectGroup[0]);

        InvokePrivateMethod(library, "InitialiseDictionary");

        AudioClip result = library.GetRandomClip(SoundGroupName.COIN);

        Assert.IsNull(result);
    }

    // asserts that requesting a sound group with no clips returns null
    [Test]
    public void SoundEffectLibrary_GetRandomClip_ReturnsNullWhenGroupIsEmpty()
    {

        SoundEffectLibrary library = CreateSoundEffectLibrary();

        SoundEffectGroup emptyGroup = new SoundEffectGroup
        {
            name = SoundGroupName.COIN,
            audioClips = new List<AudioClip>()
        };

        SetPrivateField(library, "soundEffectGroups", new[] { emptyGroup });

        InvokePrivateMethod(library, "InitialiseDictionary");

        AudioClip result = library.GetRandomClip(SoundGroupName.COIN);

        Assert.IsNull(result);
    }

    // asserts that calling Play without an initialised manager safely exits without throwing errors
    [Test]
    public void SoundEffectManager_Play_DoesNotThrowWhenNotInitialised()
    {

        Assert.DoesNotThrow(() =>
        {
            SoundEffectManager.Play(SoundGroupName.COIN);
        });
    }


    private SoundEffectLibrary CreateSoundEffectLibrary()
    {
        GameObject obj = new GameObject("SoundEffectLibrary");

        return obj.AddComponent<SoundEffectLibrary>();
    }


    private AudioClip CreateClip(string name)
    {
        AudioClip clip = AudioClip.Create(name, 44100, 1, 44100, false);

        clip.name = name;

        return clip;
    }

    // helper functions
    private void SetPrivateField(object target, string fieldName, object value)
    {
        FieldInfo field = target.GetType().GetField(
            fieldName,
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        field.SetValue(target, value);
    }


    private void InvokePrivateMethod(object target, string methodName)
    {
        MethodInfo method = target.GetType().GetMethod(
            methodName,
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        method.Invoke(target, null);
    }
}