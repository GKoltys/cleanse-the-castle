using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Should be C:\Users\[Your username]\AppData\LocalLow\DefaultCompany\projectrogue
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");

        LoadGame();
    }

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
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosistion = GameObject.FindGameObjectWithTag("Player").transform.position
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosistion;
        }
        else
        {
            SaveGame();
        }
    }
}
