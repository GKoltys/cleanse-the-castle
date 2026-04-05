using UnityEngine;

public class EnemyLootDropper : MonoBehaviour, IMapGenInit
{
    [SerializeField] private EnemyDropTable dropTable;

    private MapGenerator mapGenerator;

    // called automatically by MapGenerator when enemy is spawned
    public void Init(MapGenerator generator)
    {
        mapGenerator = generator;
    }

    public void DropLoot()
    {
        if (dropTable == null) return;

        GameObject itemToDrop = dropTable.GetRandomLootItem();
        if (itemToDrop == null) return;
        Debug.Log("Should be dropping " + itemToDrop.name);
        // spawn under mapgenerator so can be cleared on next floor
        GameObject spawnedItem = Instantiate(itemToDrop, transform.position, Quaternion.identity, mapGenerator.EntitiesRoot);

        // initialize item if needed
        var initializables = spawnedItem.GetComponentsInChildren<IMapGenInit>();

        foreach (var init in initializables)
        {
            init.Init(mapGenerator);
        }
    }
}
