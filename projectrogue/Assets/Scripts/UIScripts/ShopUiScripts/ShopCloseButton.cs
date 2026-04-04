using UnityEngine;
using UnityEngine.InputSystem;

public class ShopCloseButton : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    public void OnClose()
    {
        playerInput.actions["Attack"].Enable();
        playerInput.actions["Move"].Enable();
    }
}
