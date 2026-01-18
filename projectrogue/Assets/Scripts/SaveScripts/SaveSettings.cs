using System.IO;
using UnityEngine;

public class SaveSettings : MonoBehaviour
{
    private string saveLocation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveSettings.json");
    }
}
