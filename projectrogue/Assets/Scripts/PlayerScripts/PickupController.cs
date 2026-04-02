using UnityEngine;

public class PickupController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer heldItemRenderer;
    [SerializeField] private string pickupTriggerName = "Pickup";
    [SerializeField] private GameObject pickupPanel;

    private Sprite pendingSprite;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (heldItemRenderer != null)
            heldItemRenderer.enabled = false;
    }

    public void PlayPickupAnimation(Sprite itemSprite)
    {
        if (itemSprite == null) return;

        pendingSprite = itemSprite;
        animator.SetTrigger(pickupTriggerName);
    }

    // Call from animation event
    public void ShowHeldItem()
    {
        if (heldItemRenderer == null || pendingSprite == null) return;

        heldItemRenderer.sprite = pendingSprite;
        heldItemRenderer.enabled = true;

        if (pickupPanel != null)
        {
            pickupPanel.SetActive(true);
        }
    }

    // Call from animation event
    public void HideHeldItem()
    {
        if (heldItemRenderer == null) return;

        heldItemRenderer.enabled = false;
        heldItemRenderer.sprite = null;
        pendingSprite = null;

        if (pickupPanel != null)
            pickupPanel.SetActive(false);
    }
}