using UnityEngine;

public class EnemyLootDropper : MonoBehaviour
{
    [SerializeField] private EnemyDropTable dropTable;

    public void DropLoot()
    {
        if (dropTable == null) return;

        GameObject itemToDrop = dropTable.GetRandomLootItem();
        if (itemToDrop == null) return;

        Instantiate(itemToDrop, transform.position, Quaternion.identity);
    }
}
