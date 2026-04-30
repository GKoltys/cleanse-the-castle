using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenuController : MonoBehaviour
{
    public GameObject menuCanvas;

    public void ResumeGame()
    {
        menuCanvas.SetActive(false);
        Time.timeScale = 1f;
        UIController.Instance.SetPlayerInputs(true);
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

    public void QuitMainMenu()
    {
        Time.timeScale = 1f;
        OnSavePressed();
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        OnSavePressed();
    #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
