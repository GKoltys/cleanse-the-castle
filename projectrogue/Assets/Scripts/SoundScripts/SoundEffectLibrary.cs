using System.Collections.Generic;
using UnityEngine;

public enum SoundGroupName
{
    PLAYERHURT,
    SWORDSLASH,
    COIN,
    HEAL,
    BUFF,
    RELIC,
    CHEST,
    LOCKEDCHEST,
    PLAYERDEATH,
    STAIRS,
    KEY,
    SWORDWHOOSH
}

// https://www.youtube.com/watch?v=rAX_r0yBwzQ
public class SoundEffectLibrary : MonoBehaviour
{
    [SerializeField] private SoundEffectGroup[] soundEffectGroups;
    private Dictionary<SoundGroupName, List<AudioClip>> soundDictionary;

    private void Awake()
    {
        InitialiseDictionary();
    }

    private void InitialiseDictionary()
    {
        soundDictionary = new Dictionary<SoundGroupName, List<AudioClip>>();
        foreach (SoundEffectGroup soundEffectGroup in soundEffectGroups)
        {
            soundDictionary[soundEffectGroup.name] = soundEffectGroup.audioClips;
        }
    }

    public AudioClip GetRandomClip(SoundGroupName name)
    {
        if (soundDictionary.ContainsKey(name))
        {
            List<AudioClip> audioClips = soundDictionary[name];
            if (audioClips.Count > 0)
            {
                return audioClips[UnityEngine.Random.Range(0, audioClips.Count)];
            }
        }
        return null;
    }
}

[System.Serializable]
public class SoundEffectGroup
{
    public SoundGroupName name;
    public List<AudioClip> audioClips;
}
