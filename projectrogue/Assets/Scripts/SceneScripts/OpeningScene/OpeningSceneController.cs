using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

// https://docs.unity3d.com/ScriptReference/Playables.PlayableDirector-stopped.html
public class OpeningSceneController : MonoBehaviour
{
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
}
