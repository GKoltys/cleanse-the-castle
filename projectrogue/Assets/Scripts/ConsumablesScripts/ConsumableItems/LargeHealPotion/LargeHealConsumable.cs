using UnityEngine;

public class LargeHealConsumable : ConsumableController
{
    // Here we can override any consumable item logic
    // but still inherit all of its functionality

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerPickUp")) return;
        if (col) col.enabled = false;

        SetIsCollected(true);
        itemData.effect.Apply(playerEffect);
        animator.SetTrigger("Collected");
    }
}
