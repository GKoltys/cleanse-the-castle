using UnityEngine;

public class SmallHealController : ConsumableController
{
    // Here we can override any consumable item logic
    // but still inherit all of its functionality

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerPickUp")) return;
        if (col) col.enabled = false;

        SetIsCollected(true);
        itemData.effect.Apply(playerEffect);
        playerHud.AddBuffIcon(itemData, playerBase.GetHealth);
        animator.SetTrigger("Collected");
    }
}
