using UnityEngine;

public class SpeedBuffController : ConsumableController
{
    // Here we can override any consumable item logic
    // but still inherit all of its functionality

    [SerializeField] private float baseSpeed = 5f; // Hardcoded for now

    protected override void AddBuffIconOnLoad()
    {
        float speed = playerBase.GetSpeed;
        if (baseSpeed != speed) playerHud.AddBuffIcon(itemData, speed);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerPickUp")) return;
        if (col) col.enabled = false;

        SetIsCollected(true);
        itemData.effect.Apply(playerEffect);
        playerHud.AddBuffIcon(itemData, playerBase.GetSpeed);
        animator.SetTrigger("Collected");
    }
}
