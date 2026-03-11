using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItemData", menuName = "Items/ConsumableItemData")]
public class ConsumableItemData : ScriptableObject
{
    public string itemName;
    public string itemShopName;
    public Sprite icon;
    public string description;
    public ConsumableEffect effect;
    public int buyPrice;

    public GameObject itemPrefab;
}
