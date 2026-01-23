using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// https://www.youtube.com/watch?v=YOaYQrN1oYQ

public class SettingsMenuController: MonoBehaviour
{
    public AudioMixer audioMixer;

    private readonly List<Resolution> uniqueResolutions = new();
    private int currentResIndex;

    [SerializeField] private TMP_Dropdown displayDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        resolutionDropdown.ClearOptions();
        var s = SaveSettings.Instance.CurrentSettings;

        // Filters out all resolution duplicates with different Hz values
        HashSet<(int w, int h)> resolutionSet = new();

        foreach (var r in Screen.resolutions)
        {
            var key = (r.width, r.height);
            if (resolutionSet.Add(key)) uniqueResolutions.Add(r);
        }

        List<string> resList = new();

        currentResIndex = 0;
        for (int i = 0; i < uniqueResolutions.Count; i++)
        {
            string res = uniqueResolutions[i].width + " x " + uniqueResolutions[i].height;
            resList.Add(res);

            if (uniqueResolutions[i].width == Screen.currentResolution.width &&
                uniqueResolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        SetDisplayMode(s.displayModeIndex);
        SetResolution(s.resolutionIndex);
        SetMasterVolume(s.masterVolume);
        SetMusicVolume(s.musicVolume);
        SetSfxVolume(s.sfxVolume);

        resolutionDropdown.AddOptions(resList);
        resolutionDropdown.value = SaveSettings.Instance.CurrentSettings.resolutionIndex;
        resolutionDropdown.RefreshShownValue();

        displayDropdown.value = s.displayModeIndex;
        masterSlider.value = s.masterVolume;
        musicSlider.value = s.musicVolume;
        sfxSlider.value = s.sfxVolume;
    }

    public void SetDisplayMode(int displayOption)
    {
        if (displayOption == 0)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

            // Keeps the mouse locked in the window
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            SaveSettings.Instance.CurrentSettings.displayModeIndex = displayOption;
            SaveSettings.Instance.Save();
        }
        else if (displayOption == 1)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;

            // Releases the mouse from game window
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SaveSettings.Instance.CurrentSettings.displayModeIndex = displayOption;
            SaveSettings.Instance.Save();
        }
        else if (displayOption == 2)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

            // Releases the mouse from game window
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SaveSettings.Instance.CurrentSettings.displayModeIndex = displayOption;
            SaveSettings.Instance.Save();
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = uniqueResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);

        SaveSettings.Instance.CurrentSettings.resolutionIndex = resolutionIndex;
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);

        SaveSettings.Instance.CurrentSettings.masterVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        if (volume == -50)
        {
            audioMixer.SetFloat("musicVolume", -80);
        }
        else
        {
            audioMixer.SetFloat("musicVolume", volume);
        }

        SaveSettings.Instance.CurrentSettings.musicVolume = volume;
    }

    public void SetSfxVolume(float volume)
    {
        if (volume == -50)
        {
            audioMixer.SetFloat("sfxVolume", -80);
        }
        else
        {
            audioMixer.SetFloat("sfxVolume", volume);
        }

        SaveSettings.Instance.CurrentSettings.sfxVolume = volume;
    }
}
