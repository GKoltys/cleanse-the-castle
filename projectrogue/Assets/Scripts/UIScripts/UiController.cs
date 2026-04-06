using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    public GameObject menuCanvas;
    [SerializeField] private PlayerInput playerInput;
    private bool listenForInput = true;

    public static UIController Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuCanvas.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame && listenForInput)
        {
            bool isActive = !menuCanvas.activeSelf;

            menuCanvas.SetActive(!menuCanvas.activeSelf);

            playerInput.enabled = !isActive;
        }
    }

    public void SetPlayerInputs(bool condition)
    {
        playerInput.enabled = condition;
    }

    public void SetUiListener(bool flag)
    {
        listenForInput = flag;
    }
}
