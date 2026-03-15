using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeUIController : MonoBehaviour
{
    public static FadeUIController Instance { get; private set; }

    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // automatically fade in when switching scenes
    // https://docs.unity3d.com/6000.3/Documentation/ScriptReference/SceneManagement.SceneManager-sceneLoaded.html
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIn());
    }

    // guide on fade transitions https://www.youtube.com/watch?v=Ox0JCbVIMCQ
    public IEnumerator FadeOut()
    {
        if (fadeCanvasGroup == null) yield break;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f;
    }

    public IEnumerator FadeIn()
    {
        if (fadeCanvasGroup == null) yield break;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = 1f - Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 0f;
    }

    public void FadeAndLoadScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        yield return FadeOut();
        SaveController.Instance.RequestLoad();
        SceneManager.LoadSceneAsync(sceneName);

    }

}
