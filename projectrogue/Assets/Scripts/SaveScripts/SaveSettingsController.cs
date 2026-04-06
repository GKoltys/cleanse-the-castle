using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class SaveSettingsController : MonoBehaviour
{
    private string saveLocation;
    private List<Resolution> uniqueResolutions = new();
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private SettingsMenuController settingsMenuController;

    public SettingsData CurrentSettings;

    public static SaveSettingsController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        CurrentSettings = new();
    }

    private void Start()
    {
        // Filters out all resolution duplicates with different Hz values
        uniqueResolutions.Clear();

        Dictionary<(int w, int h), Resolution> resolutionMap = new();

        foreach (var r in Screen.resolutions)
        {
            var key = (r.width, r.height);

            if (!resolutionMap.ContainsKey(key))
            {
                resolutionMap[key] = r;
            }
            else
            {
                if (r.refreshRateRatio.value > resolutionMap[key].refreshRateRatio.value)
                {
                    resolutionMap[key] = r;
                }
            }
        }

        uniqueResolutions = resolutionMap.Values
            .OrderBy(r => r.width)
            .ThenBy(r => r.height)
            .ToList();

        saveLocation = Path.Combine(Application.persistentDataPath, "settings.json");
        Load();

        ApplySettings();
    }

    public void ApplySettings()
    {
        ApplyDisplayMode(CurrentSettings.displayModeIndex);
        ApplyResolution(CurrentSettings.resolutionIndex);
        ApplyMasterVolume(CurrentSettings.masterVolume);
        ApplyMusicVolume(CurrentSettings.musicVolume);
        ApplySfxVolume(CurrentSettings.sfxVolume);
    }

    public void ApplyDisplayMode(int displayOption)
    {
        if (displayOption == 0)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (displayOption == 1)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (displayOption == 2)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Cursor.lockState = CursorLockMode.None;
        }

        Cursor.visible = true;
    }

    public void ApplyResolution(int index)
    {
        if (index >= 0 && index < uniqueResolutions.Count)
        {
            var r = uniqueResolutions[index];
            //Debug.Log("Resolution changed to: " + r.width + " x " + r.height);
            Screen.SetResolution(r.width, r.height, Screen.fullScreenMode, r.refreshRateRatio);
        }
    }

    public void ApplyMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
    }

    public void ApplyMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume == -50 ? -80 : volume);
    }

    public void ApplySfxVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", volume == -50 ? -80 : volume);
    }

    public void SetDisplayMode(int index)
    {
        CurrentSettings.displayModeIndex = index;
        ApplyDisplayMode(index);
        Save();
    }

    public void SetResolution(int index)
    {
        CurrentSettings.resolutionIndex = index;
        ApplyResolution(index);
        Save();
    }

    public void SetMasterVolume(float volume)
    {
        CurrentSettings.masterVolume = volume;
        ApplyMasterVolume(volume);
        Save();
    }

    public void SetMusicVolume(float volume)
    {
        CurrentSettings.musicVolume = volume;
        ApplyMusicVolume(volume);
        Save();
    }

    public void SetSfxVolume(float volume)
    {
        CurrentSettings.sfxVolume = volume;
        ApplySfxVolume(volume);
        Save();
    }

    public List<Resolution> GetResolutionList()
    {
        return uniqueResolutions;
    }

    public int GetResolution()
    {
        return CurrentSettings.resolutionIndex;
    }

    public void Save()
    {
        File.WriteAllText(saveLocation, JsonUtility.ToJson(CurrentSettings, true));
    }

    public void Load()
    {
        if (!File.Exists(saveLocation))
        {
            // Setting highest res by default
            CurrentSettings.resolutionIndex = uniqueResolutions.Count - 1;
            Save();
            return;
        }
        else
        {
            CurrentSettings = JsonUtility.FromJson<SettingsData>(File.ReadAllText(saveLocation));
        }
    }
}
