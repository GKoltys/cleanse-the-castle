using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionDoorController : MonoBehaviour
{
    private string sceneToLoad = "DungeonScene";

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            // https://discussions.unity.com/t/how-to-make-the-scene-change-on-a-collision/636855
            if (FadeUIController.Instance != null)
            {
                FadeUIController.Instance.FadeAndLoadScene(sceneToLoad);
            }
            else
                SceneManager.LoadScene(sceneToLoad);
        }
    }
}
