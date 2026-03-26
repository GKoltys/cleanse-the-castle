using UnityEngine;

// when a relic item is received by player it is added to playerrelics list and despawned
public class RelicController : ConsumableController
{
    private PlayerRelics playerRelics;

    protected override void Start()
    {
        base.Start();
        playerRelics = playerBase.GetComponent<PlayerRelics>();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Relic hit by: {other.name}, tag: {other.tag}");
        if (!other.CompareTag("PlayerPickUp")) return;
        if (col) col.enabled = false;

        SetIsCollected(true);

        if (playerRelics != null)
        {
            playerRelics.AddRelic(itemData);
        }

        Despawn();
    }
}