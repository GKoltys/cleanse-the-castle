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

    [SerializeField] private TMP_Dropdown displayDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        resolutionDropdown.ClearOptions();
        var s = SaveSettingsController.Instance.CurrentSettings;

        // Filters out all resolution duplicates with different Hz values
        HashSet<(int w, int h)> resolutionSet = new();

        foreach (var r in Screen.resolutions)
        {
            var key = (r.width, r.height);
            if (resolutionSet.Add(key)) uniqueResolutions.Add(r);
        }

        List<string> resList = new();

        for (int i = 0; i < uniqueResolutions.Count; i++)
        {
            string res = uniqueResolutions[i].width + " x " + uniqueResolutions[i].height;
            resList.Add(res);
        }

        resolutionDropdown.AddOptions(resList);
        resolutionDropdown.value = s.resolutionIndex;
        resolutionDropdown.RefreshShownValue();

        displayDropdown.value = s.displayModeIndex;
        masterSlider.value = s.masterVolume;
        musicSlider.value = s.musicVolume;
        sfxSlider.value = s.sfxVolume;
    }

    public void SetDisplayMode(int index)
    {
        SaveSettingsController.Instance.SetDisplayMode(index);
    }

    public void SetResolution(int index)
    {
        SaveSettingsController.Instance.SetResolution(index);
    }

    public void SetMasterVolume(float volume)
    {
        SaveSettingsController.Instance.SetMasterVolume(volume);
    }

    public void SetMusicVolume(float volume)
    {
        SaveSettingsController.Instance.SetMusicVolume(volume);
    }

    public void SetSfxVolume(float volume)
    {
        SaveSettingsController.Instance.SetSfxVolume(volume);
    }
}
