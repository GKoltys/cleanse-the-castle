using UnityEngine;

public class MaxHealthBuffController : ConsumableController
{
    // Here we can override any consumable item logic
    // but still inherit all of its functionality

    [SerializeField] private float baseMaxHealth = 100f; // Hardcoded for now

    protected override void AddBuffIconOnLoad()
    {
        float maxHealth = playerBase.GetMaxHealth;
        if (baseMaxHealth != maxHealth) playerHud.AddBuffIcon(itemData, maxHealth);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerPickUp")) return;
        if (col) col.enabled = false;

        SetIsCollected(true);
        itemData.effect.Apply(playerEffect);
        playerHud.AddBuffIcon(itemData, playerBase.GetMaxHealth);
        animator.SetTrigger("Collected");
    }
}
