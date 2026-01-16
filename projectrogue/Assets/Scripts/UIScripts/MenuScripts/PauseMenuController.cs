using UnityEditor;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject menuCanvas;

    public void ResumeGame()
    {
        menuCanvas.SetActive(false);
    }
    
    public void OnSavePressed()
    {
        if (SaveController.Instance == null)
        {
            Debug.LogError("SaveController not found on save");
            return;
        }

        SaveController.Instance.SaveGame();
    }

    public void OnLoadPressed()
    {
        if (SaveController.Instance == null)
        {
            Debug.LogError("SaveController not found on load");
            return;
        }

        SaveController.Instance.LoadGame();
        menuCanvas.SetActive(false);
    }

    public void QuitGame()
    {
        EditorApplication.isPlaying = false;
    }
}
