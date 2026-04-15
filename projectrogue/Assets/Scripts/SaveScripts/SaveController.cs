using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveController : MonoBehaviour
{
    private readonly Vector3 defaultPosition = Vector3.zero;
    private readonly int defaultFloorCount = 0;
    private readonly float defaultSpeed = 5f;
    private readonly float defaultIFrameSeconds = 0.5f;
    private readonly float defaultMaxHealth = 100;
    private readonly float defaultHealth = 100;
    private readonly int defaultCoinCount = 0;
    private readonly int defaultKeyCount = 0;
    private readonly int defaultWeaponId = 0;
    private readonly float defaultDamageMultiplier = 1f;

    private bool newRun = false;
    private string saveLocation;
    private bool shouldLoadOnNextScene;

    GameObject player;
    public PlayerStats playerStats;
    PlayerBase playerBase;
    PlayerRelics playerRelics;
    [SerializeField] private RelicLookup relicLookup;

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
        // Keep this commented until final build
        // if (!shouldLoadOnNextScene) return;

        shouldLoadOnNextScene = false;
        Debug.Log(scene.name);

        if (scene.name == "StartingArea")
        {
            DungeonBgmController.Instance.ChangeMusic(0);
        }
        if (scene.name == "MainMenu" || scene.name == "EndingScene") return;
        if (scene.name == "OpeningScene")
        {
            WipeSaveFile();
            return;
        }
        if (newRun)
        {
            newRun = false;
            WipeSaveFile();
        }

        Debug.Log("Player should be instantiated");
        // Get new references to Player
        player = GameObject.FindGameObjectWithTag("Player");
        playerBase = player.GetComponent<PlayerBase>();
        playerStats = player.GetComponent<PlayerStats>();
        playerRelics = player.GetComponent<PlayerRelics>();

        

        LoadGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosistion = player.transform.position,
            playerFloorCount = playerBase.GetFloorCount,
            playerSpeed = playerBase.GetSpeed,
            playerIFrameSeconds = playerBase.GetIFrameSeconds,
            playerMaxHealth = playerBase.GetMaxHealth,
            playerHealth = playerBase.GetHealth,
            playerCoinCount = playerBase.GetCoinCount,
            playerKeyCount = playerBase.GetKeyCount,
            playerWeaponId = playerBase.GetWeaponId,
            playerDamageMultiplier = playerBase.GetDamageMultiplier,
            playerRelicIds = playerRelics != null ? playerRelics.GetRelicNames() : new List<string>()
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (HasSaveFile())
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            player.transform.position = saveData.playerPosistion;
            playerStats.SetFloorCount(saveData.playerFloorCount);
            playerStats.SetSpeed(saveData.playerSpeed);
            playerStats.SetIFrameSeconds(saveData.playerIFrameSeconds);
            playerStats.SetMaxHealth(saveData.playerMaxHealth);
            playerStats.SetHealth(saveData.playerHealth);
            playerStats.SetCoinCount(saveData.playerCoinCount);
            playerStats.SetKeyCount(saveData.playerKeyCount);
            playerStats.SetWeapon(saveData.playerWeaponId);
            playerStats.SetDamageMulitplier(saveData.playerDamageMultiplier);

            playerBase.ApplyLoadedStats(playerStats);

            // get each relic information
            foreach (string relicId in saveData.playerRelicIds)
            {
                ConsumableItemData relic = relicLookup.GetRelicByName(relicId);

                if (relic != null)
                    playerRelics.AddRelic(relic);
            }
        }
        else
        {
            SaveGame();
        }
    }

    public void StartNewRun()
    {
        newRun = true;
        RequestLoad();
        SceneManager.LoadScene(2);
    }

    public void FinishGame()
    {
        if (HasSaveFile())
        {
            File.Delete(saveLocation);
            RequestLoad();
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }

    public void WipeSaveFile()
    {
        SaveData saveData = new SaveData
        {
            playerPosistion = defaultPosition,
            playerFloorCount = defaultFloorCount,
            playerSpeed = defaultSpeed,
            playerIFrameSeconds = defaultIFrameSeconds,
            playerMaxHealth = defaultMaxHealth,
            playerHealth = defaultHealth,
            playerCoinCount = defaultCoinCount,
            playerKeyCount = defaultKeyCount,
            playerWeaponId = defaultWeaponId,
            playerDamageMultiplier = defaultDamageMultiplier
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public int GetLastFloor()
    {
        if (HasSaveFile())
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            return saveData.playerFloorCount;
        }
        return 0;
    }

    public String GetSaveLocation() { return saveLocation; }
}
