using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItemData", menuName = "Items/ConsumableItemData")]
public class ConsumableItemData : ScriptableObject
{
    public ConsumableType consumableType;
    public string itemName;
    public string itemShopName;
    public Sprite icon;
    public StatType statType;
    public float baseStat;
    public string description;
    public string statDescription;
    public ConsumableEffect effect;
    public int buyPrice;

    public GameObject itemPrefab;
}

public enum ConsumableType
{
    BUFF,
    POTION
}

public enum StatType
{
    SPEED,
    DAMAGEMULTIPLIER,
    IFRAMESECONDS,
    MAXHEALTH
}
