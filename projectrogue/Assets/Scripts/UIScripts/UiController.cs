using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    public GameObject menuCanvas;
    [SerializeField] private PlayerInput playerInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuCanvas.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            bool isActive = !menuCanvas.activeSelf;

            menuCanvas.SetActive(!menuCanvas.activeSelf);

            playerInput.enabled = !isActive;
        }
    }
}
