using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonBgmController : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip startingArea;
    [SerializeField] private AudioClip dungeon;
    [SerializeField] private AudioClip bossBattle;
    [SerializeField] private AudioClip finalBattle;

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

            default:
                newClip = dungeon;
                break;
        }

        if (audioSource.clip == newClip) return;

        audioSource.clip = newClip;
        audioSource.Play();
    }
}
