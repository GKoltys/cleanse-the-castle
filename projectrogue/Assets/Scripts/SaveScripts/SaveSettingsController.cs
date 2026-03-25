using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class SaveSettingsController : MonoBehaviour
{
    private string saveLocation;
    [SerializeField] private AudioMixer audioMixer;
    
    public SettingsData CurrentSettings {  get; private set; } = new SettingsData();

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

        saveLocation = Path.Combine(Application.persistentDataPath, "settings.json");
        Load();
    }

    private void Start()
    {
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
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
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
        var resolutions = Screen.resolutions;

        if (index >= 0 && index < resolutions.Length)
        {
            var r = resolutions[index];
            Screen.SetResolution(r.width, r.height, Screen.fullScreenMode);
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
