using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonBgmController : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip startingArea;
    [SerializeField] private AudioClip waterDungeon;
    [SerializeField] private AudioClip lavaDungeon;
    [SerializeField] private AudioClip bossBattle;
    [SerializeField] private AudioClip finalBattle;
    [SerializeField] private AudioClip endingScene;

    public static DungeonBgmController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    public void ChangeMusic(int floor)
    {
        AudioClip newClip;

        switch (floor)
        {
            case 0:
                newClip = startingArea;
                break;

            case 10:
            case 20:
                newClip = bossBattle;
                break;

            case 30:
                newClip = finalBattle;
                break;

            case < 16:
                newClip = waterDungeon;
                break;

            case < 30:
                newClip = lavaDungeon;
                break;

            default:
                newClip = waterDungeon;
                break;
        }

        if (audioSource.clip == newClip) return;

        audioSource.clip = newClip;
        audioSource.Play();
    }

    public void PlayEndingSceneMusic()
    {
        audioSource.clip = endingScene;
        audioSource.Play();
    }
}
