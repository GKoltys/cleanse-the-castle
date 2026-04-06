using UnityEngine;
using UnityEngine.InputSystem;

public class ShopCloseButton : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    public void Start()
    {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
    }

    public void OnClose()
    {
        ShopController.instance.CloseShop();
        playerInput.actions["Attack"].Enable();
        playerInput.actions["Move"].Enable();
    }
}
