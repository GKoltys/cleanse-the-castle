using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void ContinueGame()
    {
        SaveController.Instance.RequestLoad();
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
    #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }
}
