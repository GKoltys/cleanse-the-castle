using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class OpeningSceneController : MonoBehaviour
{
    // https://docs.unity3d.com/ScriptReference/Playables.PlayableDirector-stopped.html
    [SerializeField] private PlayableDirector director;

    void OnEnable()
    {
        director.stopped += OnPlayableDirectorStopped;
    }

    void OnDisable()
    {
        director.stopped -= OnPlayableDirectorStopped;
    }

    void OnPlayableDirectorStopped(PlayableDirector playableDirector)
    {
        Debug.Log("cutscene is over");
        SaveController.Instance.RequestLoad();
        SceneManager.LoadSceneAsync(2);
    }

    // To skip cutscene
    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("cutscene was skipped");
            SaveController.Instance.RequestLoad();
            SceneManager.LoadSceneAsync(2);
        }
    }
}
