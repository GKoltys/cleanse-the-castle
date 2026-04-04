using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private TMP_Text continueText;
    [SerializeField] private GameObject settingsMenu;

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            settingsMenu.SetActive(!settingsMenu.activeSelf);
        }
    }

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
        int lastFloor = SaveController.Instance.GetLastFloor();
        if (lastFloor > 0)
        {
            SaveController.Instance.RequestLoad();
            SceneManager.LoadSceneAsync(3);
        }
        else
        {
            SaveController.Instance.RequestLoad();
            SceneManager.LoadSceneAsync(2);
        }
    }

    public void Settings()
    {
        settingsMenu.SetActive(!settingsMenu.activeSelf);
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
