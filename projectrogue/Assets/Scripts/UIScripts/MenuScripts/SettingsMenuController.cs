using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

// https://www.youtube.com/watch?v=YOaYQrN1oYQ

public class SettingsMenuController: MonoBehaviour
{
    public AudioMixer audioMixer;

    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private int currentResIndex;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> resList = new();

        currentResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string res = resolutions[i].width + " x " + resolutions[i].height;
            resList.Add(res);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resList);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetDisplayMode(int displayOption)
    {
        if (displayOption == 0)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else if (displayOption == 1)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else if (displayOption == 2)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }

        Resolution r = resolutions[currentResIndex];
        Screen.SetResolution(r.width, r.height, Screen.fullScreenMode);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
    }

    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", volume);
    }
}
