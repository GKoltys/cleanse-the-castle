using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class EndingSceneController : MonoBehaviour
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
        Debug.Log("ending cutscene is over");
        SaveController.Instance.FinishGame();
    }

    // To skip cutscene
    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("ending cutscene was skipped");
            SaveController.Instance.FinishGame();
        }
    }
}
