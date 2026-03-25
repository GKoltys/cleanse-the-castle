using System.Collections.Generic;
using UnityEngine;

// create a list of relic type items that can be added and removed
public class PlayerRelics : MonoBehaviour
{
    private PlayerApplyEffect playerEffect;
    private PlayerHud playerHud;
    private PlayerBase playerBase;
    private readonly List<ConsumableItemData> relics = new();

    private float thornPercent = 0f;

    private void Awake()
    {
        playerEffect = GetComponent<PlayerApplyEffect>();
        playerHud = GetComponent<PlayerHud>();
        playerBase = GetComponent<PlayerBase>();
    }

    public void AddRelic(ConsumableItemData relicData)
    {
        if (relicData == null || relicData.effect == null || relicData.consumableType != ConsumableType.RELIC) return;
        if (relics.Contains(relicData)) return;

        // add relic to list and apply effect
        relics.Add(relicData);
        relicData.effect.Apply(playerEffect);
        playerHud?.AddRelicIcon(relicData);

        Debug.Log("Relic collected: " + relicData.itemName);
    }

    public void RemoveRelic(ConsumableItemData relicData)
    {
        if (relicData == null) return;
        if (!relics.Contains(relicData)) return;

        // remove effect of relic and remove from list
        relicData.effect.Remove(playerEffect);
        relics.Remove(relicData);
    }

    public bool HasRelic(ConsumableItemData relicData)
    {
        return relics.Contains(relicData);
    }

    public IReadOnlyList<ConsumableItemData> GetRelics()
    {
        return relics;
    }

    public List<string> GetRelicNames()
    {
        List<string> names = new();

        foreach (ConsumableItemData relic in relics)
        {
            if (relic != null && !string.IsNullOrEmpty(relic.itemName))
                names.Add(relic.itemName);
        }

        return names;
    }

    // thorns effect functionality
    public void AddThorns(float percent)
    {
        thornPercent += percent;
    }

    public void RemoveThorns(float percent)
    {
        thornPercent -= percent;
        if (thornPercent < 0f) thornPercent = 0f;
    }

    public void TriggerThorns(EnemyBase attacker)
    {
        if (attacker == null) return;
        if (!attacker.IsAlive) return;
        if (thornPercent <= 0f) return;

        float reflectedDamage = playerBase.GetMaxHealth * thornPercent;
        attacker.TakeDamage(reflectedDamage);

        Debug.Log($"Thorns dealt {reflectedDamage} damage to {attacker.name}");
    }
}