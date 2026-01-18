using System;

[System.Serializable]
public class SettingsData
{
    public DisplayOptions dispalyOption = DisplayOptions.Fullscreen;
    public Tuple<int, int> resolution = new(1920, 1080);
    public float musicVolume = 1;
    public float sfxVolume = 1;
}
