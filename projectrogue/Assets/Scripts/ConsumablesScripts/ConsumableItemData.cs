using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItemData", menuName = "Items/ConsumableItemData")]
public class ConsumableItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public string description;
    [Range(0, 1)] public float dropChance;
    public ConsumableEffect effect;
}
