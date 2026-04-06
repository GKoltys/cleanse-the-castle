using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// https://www.youtube.com/watch?v=YOaYQrN1oYQ

public class SettingsMenuController: MonoBehaviour
{
    public AudioMixer audioMixer;

    private List<Resolution> uniqueResolutions = new();

    [SerializeField] private TMP_Dropdown displayDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        resolutionDropdown.ClearOptions();
        var s = SaveSettingsController.Instance.CurrentSettings;

        uniqueResolutions = SaveSettingsController.Instance.GetResolutionList();

        List<string> resList = new();

        for (int i = 0; i < uniqueResolutions.Count; i++)
        {
            var r = uniqueResolutions[i];
            resList.Add($"{r.width} x {r.height}");
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

    public List<Resolution> GetUniqueResolutions()
    {
        return uniqueResolutions;
    }
}
