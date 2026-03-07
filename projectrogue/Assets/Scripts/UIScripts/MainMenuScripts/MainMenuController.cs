using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;



#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private TMP_Text continueText;

    private void Start()
    {
        if (!File.Exists(SaveController.Instance.GetSaveLocation()))
        {
            continueButton.interactable = false;
            continueText.color = new Color32(115, 115, 115, 255);
            continueText.GetComponent<TmpHoverHighlight>().SetActive(false);
        }
    }

    public void NewGame()
    {
        SaveController.Instance.RequestLoad();
        SceneManager.LoadSceneAsync(1);
    }
    public void ContinueGame()
    {
        SaveController.Instance.RequestLoad();
        SceneManager.LoadSceneAsync(2);
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
