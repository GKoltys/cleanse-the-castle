using UnityEngine;

public class DamageBuffContoller : ConsumableController
{
    // Here we can override any consumable item logic
    // but still inherit all of its functionality

    [SerializeField] private float baseDamageMultiplier = 1f; // Hardcoded for now

    protected override void AddBuffIconOnLoad()
    {
        float damageMultiplier = playerBase.GetDamageMultiplier;
        if (baseDamageMultiplier != damageMultiplier) playerHud.AddBuffIcon(itemData, damageMultiplier);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerPickUp")) return;
        if (col) col.enabled = false;

        SetIsCollected(true);
        itemData.effect.Apply(playerEffect);
        playerHud.AddBuffIcon(itemData, playerBase.GetDamageMultiplier);
        animator.SetTrigger("Collected");
    }
}
