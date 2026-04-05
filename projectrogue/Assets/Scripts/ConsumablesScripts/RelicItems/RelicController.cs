using UnityEngine;

// when a relic item is received by player it is added to playerrelics list and despawned
public class RelicController : ConsumableController
{
    private PlayerRelics playerRelics;
    private PickupController pickupController;

    protected override void Start()
    {
        base.Start();
        playerRelics = playerBase.GetComponent<PlayerRelics>();
        pickupController = playerBase.GetComponent<PickupController>();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerPickUp")) return;
        if (col) col.enabled = false;

        SetIsCollected(true);

        if (playerRelics != null)
        {
            playerRelics.AddRelic(itemData);
        }

        if (pickupController != null && itemData != null && itemData.icon != null)
        {
            UIController.Instance.SetUiListener(false);
            // play pickup animation of relic
            pickupController.PlayPickupAnimation(itemData.icon, itemData.itemShopName);
        }

        Despawn();
    }
}