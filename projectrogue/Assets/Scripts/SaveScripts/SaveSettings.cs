using System.IO;
using UnityEngine;

public class SaveSettings : MonoBehaviour
{
    private string saveLocation;
    
    public SettingsData CurrentSettings {  get; private set; } = new SettingsData();

    public static SaveSettings Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }


        Instance = this;
        DontDestroyOnLoad(gameObject);

        saveLocation = Path.Combine(Application.persistentDataPath, "settings.json");
        Load();
    }

    public void Save()
    {
        File.WriteAllText(saveLocation, JsonUtility.ToJson(CurrentSettings, true));
    }

    public void Load()
    {
        if (!File.Exists(saveLocation))
        {
            Save();
            return;
        }
        else
        {
            CurrentSettings = JsonUtility.FromJson<SettingsData>(File.ReadAllText(saveLocation));
        }
    }
}
