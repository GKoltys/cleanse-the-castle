using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PickupController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private SpriteRenderer heldItemRenderer;
    [SerializeField] private string pickupTriggerName = "Pickup";
    [SerializeField] private GameObject pickupPanel;
    [SerializeField] private TMPro.TMP_Text itemNameText;
    [SerializeField] private Rigidbody2D rb;

    private Sprite pendingSprite;
    private string pendingItemName;
    private InputAction interactAction;
    private bool waitingForPickupConfirm;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (heldItemRenderer != null)
        {
            heldItemRenderer.enabled = false;
        }

        if (pickupPanel != null)
        {
            pickupPanel.SetActive(false);
        }

        interactAction = playerInput.actions["Interact"];
    }

    public void PlayPickupAnimation(Sprite itemSprite, string itemName)
    {
        if (itemSprite == null || animator == null) return;

        pendingSprite = itemSprite;
        pendingItemName = itemName;
        animator.SetTrigger(pickupTriggerName);
    }

    // Animation event
    public void ShowHeldItem()
    {
        if (heldItemRenderer == null || pendingSprite == null) return;

        heldItemRenderer.sprite = pendingSprite;
        heldItemRenderer.enabled = true;

        if (pickupPanel != null)
        {
            pickupPanel.SetActive(true);

            if (itemNameText != null)
            {
                itemNameText.text = pendingItemName;
            }
        }

        // disable movement and velocity so player is static during animation
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (movement != null)
        {
            movement.enabled = false;
        }

        animator.speed = 0f;
        waitingForPickupConfirm = true;

        if (interactAction != null)
        {
            interactAction.Enable();
            interactAction.performed -= OnInteractPressed;
            interactAction.performed += OnInteractPressed;
        }

        SoundEffectManager.Play(SoundGroupName.RELIC);
    }

    private void OnInteractPressed(InputAction.CallbackContext ctx)
    {
        if (!waitingForPickupConfirm) return;

        waitingForPickupConfirm = false;

        if (interactAction != null)
            interactAction.performed -= OnInteractPressed;

        if (movement != null)
            movement.enabled = true;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        animator.speed = 1f;
    }

    // Animation event
    public void HideHeldItem()
    {
        if (heldItemRenderer != null)
        {
            heldItemRenderer.enabled = false;
            heldItemRenderer.sprite = null;
        }

        pendingSprite = null;

        if (pickupPanel != null)
            pickupPanel.SetActive(false);

        if (movement != null)
            movement.enabled = true;

        animator.speed = 1f;
        waitingForPickupConfirm = false;

        if (interactAction != null)
            interactAction.performed -= OnInteractPressed;
    }

    private void OnDisable()
    {
        if (interactAction != null)
            interactAction.performed -= OnInteractPressed;
    }
}