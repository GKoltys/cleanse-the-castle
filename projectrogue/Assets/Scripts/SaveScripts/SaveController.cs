using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private bool shouldLoadOnNextScene;

    GameObject player;
    PlayerStats playerStats;
    PlayerBase playerBase;

    // https://www.youtube.com/watch?v=VTZ1TQR80Qc
    // Declaring SaveController as a singleton
    public static SaveController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Should be C:\Users\[Your username]\AppData\LocalLow\DefaultCompany\projectrogue
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this) SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public bool HasSaveFile()
    {
        return File.Exists(saveLocation);
    }

    public void RequestLoad()
    {
        shouldLoadOnNextScene = true;
    }

    // Called automatically by the engine after LoadScenceAsync()
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //if (!shouldLoadOnNextScene) return;

        shouldLoadOnNextScene = false;

        LoadGame();
    }

    public void SaveGame()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerBase = player.GetComponent<PlayerBase>();

        SaveData saveData = new SaveData
        {
            
            playerPosistion = player.transform.position,
            playerSpeed = playerBase.GetSpeed,
            playerIFrameSeconds = playerBase.GetIFrameSeconds,
            playerMaxHealth = playerBase.GetMaxHealth,
            playerHealth = playerBase.GetHealth,
            playerCoinCount = playerBase.GetCoinCount,
            playerWeaponId = playerBase.GetWeaponId
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerStats = player.GetComponent<PlayerStats>();
            playerBase = player.GetComponent<PlayerBase>();

            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            player.transform.position = saveData.playerPosistion;
            playerStats.SetSpeed(saveData.playerSpeed);
            playerStats.SetIFrameSeconds(saveData.playerIFrameSeconds);
            playerStats.SetMaxHealth(saveData.playerMaxHealth);
            playerStats.SetHealth(saveData.playerHealth);
            playerStats.SetCoinCount(saveData.playerCoinCount);
            playerStats.SetWeapon(saveData.playerWeaponId);

            playerBase.ApplyLoadedStats(playerStats);
        }
        else
        {
            SaveGame();
        }
    }
}
