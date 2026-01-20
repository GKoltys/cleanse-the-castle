using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

// https://www.youtube.com/watch?v=YOaYQrN1oYQ

public class SettingsMenuController: MonoBehaviour
{
    public AudioMixer audioMixer;

    public TMP_Dropdown resolutionDropdown;

    private List<Resolution> uniqueResolutions = new();
    private int currentResIndex;

    private void Start()
    {
        resolutionDropdown.ClearOptions();

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

        resolutionDropdown.AddOptions(resList);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetDisplayMode(int displayOption)
    {
        if (displayOption == 0)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

            // Keeps the mouse locked in the window
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else if (displayOption == 1)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;

            // Releases the mouse from game window
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (displayOption == 2)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

            // Releases the mouse from game window
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = uniqueResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
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
