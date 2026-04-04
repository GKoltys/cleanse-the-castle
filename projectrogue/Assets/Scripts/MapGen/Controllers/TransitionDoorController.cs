using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionDoorController : MonoBehaviour
{
    [SerializeField] PlayerBase playerBase; 
    private string sceneToLoad = "DungeonScene";

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") && playerBase.GetWeaponId != 0)
        {
            // https://discussions.unity.com/t/how-to-make-the-scene-change-on-a-collision/636855
            if (FadeUIController.Instance != null)
            {
                FadeUIController.Instance.FadeAndLoadScene(sceneToLoad);
            }
            else
            {
                SaveController.Instance.RequestLoad();
                SceneManager.LoadSceneAsync(sceneToLoad);
            }
        }
    }
}
