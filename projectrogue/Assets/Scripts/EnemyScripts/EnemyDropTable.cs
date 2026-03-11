using System.Collections.Generic;
using UnityEngine;

// https://outscal.com/blog/unity-weighted-random-system-loot-drops?srsltid=AfmBOooGqh7wFpC_9jCsCRqs2V-xG4JB6K64rVO8mLeemL8V1NSVbeMY
[CreateAssetMenu(fileName = "EnemyDropTable", menuName = "Enemies/EnemyDropTable")]
public class EnemyDropTable : ScriptableObject
{
    [SerializeField] private List<DropEntry> lootTable = new();
    [SerializeField] private float noDropWeight = 0f;

    public GameObject GetRandomLootItem()
    {
        float totalWeight = noDropWeight;

        foreach (DropEntry entry in lootTable)
        {
            if (entry.itemPrefab == null) continue;
            totalWeight += entry.weight;
        }

        if (totalWeight <= 0f) return null;

        float randomPoint = UnityEngine.Random.Range(0, totalWeight);

        if (randomPoint < noDropWeight) return null;

        randomPoint -= noDropWeight;

        foreach (DropEntry entry in lootTable)
        {
            if (entry.itemPrefab == null) continue;

            if (randomPoint < entry.weight)
            {
                return entry.itemPrefab;
            }
            else
            {
                randomPoint -= entry.weight;
            }
        }
        return null; // Should not happen if weights are positive
    }
}
