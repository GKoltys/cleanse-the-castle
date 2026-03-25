using System.Collections.Generic;
using UnityEngine;

// returns relic item data based on relic name
[CreateAssetMenu(fileName = "RelicLookup", menuName = "Items/RelicLookup")]
public class RelicLookup : ScriptableObject
{
    [SerializeField] private List<ConsumableItemData> relics = new();

    public ConsumableItemData GetRelicByName(string name)
    {
        foreach (ConsumableItemData relic in relics)
        {
            if (relic != null && relic.itemName == name)
                return relic;
        }

        Debug.LogWarning("RelicLookup: No relic found for id " + name);
        return null;
    }
}