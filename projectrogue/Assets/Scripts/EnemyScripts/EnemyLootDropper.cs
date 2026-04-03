using UnityEngine;

public class EnemyLootDropper : MonoBehaviour
{
    [SerializeField] private EnemyDropTable dropTable;

    public void DropLoot()
    {
        if (dropTable == null) return;

        GameObject itemToDrop = dropTable.GetRandomLootItem();
        if (itemToDrop == null) return;
        Debug.Log("Should be dropping " + itemToDrop.name);
        Instantiate(itemToDrop, transform.position, Quaternion.identity);
    }
}
