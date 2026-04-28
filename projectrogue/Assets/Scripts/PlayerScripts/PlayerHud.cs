using System.Collections.Generic;
using UnityEngine;

public class PlayerHud : MonoBehaviour, IShopHud
{
    private PlayerStats playerStats;
    private PlayerBase playerBase;
    [SerializeField] private HealthBarUI healthBar;
    [SerializeField] private CoinCounterUI coinCounterObj;
    [SerializeField] private KeyCounterUI keyCounterObj;
    [SerializeField] private FloorCounterUI floorCounterObj;
    [SerializeField] private Transform buffContainer;
    [SerializeField] private Transform relicContainer;
    [SerializeField] private BuffIconUI buffIconPrefab;
    [SerializeField] private BuffToolTipUI buffToolTip;
    [SerializeField] private BuffToolTipUI relicToolTip;
    private readonly List<BuffIconUI> buffIconList = new();
    private readonly List<BuffIconUI> relicIconList = new();

    [SerializeField] private List<ConsumableItemData> buffList;
    private PlayerRelics playerRelics;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerBase = GetComponent<PlayerBase>();
        playerRelics = GetComponent<PlayerRelics>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar.SetMaxHealth(playerStats.GetMaxHealth);
        healthBar.SetHealth(playerStats.GetHealth);
        coinCounterObj.UpdateCoinCounter(playerStats.GetCoinCount);
        keyCounterObj.UpdateKeyCounter(playerStats.GetKeyCount);
        floorCounterObj.UpdateFloorCounter(playerStats.GetFloorCount);
    }

    public void SetHudOnLoad(float maxHealth, float health, int coinCounter, int keyCounter, int floorCounter)
    {
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(health);
        coinCounterObj.UpdateCoinCounter(coinCounter);
        keyCounterObj.UpdateKeyCounter(keyCounter);
        floorCounterObj.UpdateFloorCounter(floorCounter);

        foreach (ConsumableItemData buff in buffList)
        {
            float current = GetStatValue(buff.statType);
            if (current != buff.baseStat) AddBuffIcon(buff);
        }

        if (playerRelics != null)
        {
            foreach (ConsumableItemData relic in playerRelics.GetRelics())
            {
                AddRelicIcon(relic);
            }
        }
    }

    private float GetStatValue(StatType statType)
    {
        switch (statType)
        {
            case StatType.SPEED:
                return playerBase.GetSpeed;

            case StatType.DAMAGEMULTIPLIER:
                return playerBase.GetDamageMultiplier;

            case StatType.MAXHEALTH:
                return playerBase.GetMaxHealth;
        }
        Debug.LogWarning("PlayerHud: unhandled StatType: " + statType);
        return 0f;
    }

    public void AddBuffIcon(ConsumableItemData itemData)
    {
        if (itemData == null) return;

        float currentStat = GetStatValue(itemData.statType);

        foreach (BuffIconUI icon in buffIconList)
        {
            if (icon.GetName == itemData.itemName)
            {
                icon.SetStat(currentStat);
                return;
            }
        }

        BuffIconUI newIcon = Instantiate(buffIconPrefab, buffContainer);
        newIcon.Setup(itemData.icon, itemData.itemName, itemData.statDescription, currentStat, buffToolTip, false);

        buffIconList.Add(newIcon);
    }


    public void AddRelicIcon(ConsumableItemData itemData)
    {
        if (itemData == null) return;

        float currentStat = GetStatValue(itemData.statType);
        Debug.Log($"AddRelicIcon: adding icon for {itemData.itemName}");

        foreach (BuffIconUI icon in relicIconList)
        {
            if (icon.GetName == itemData.itemName)
            {
                icon.SetStat(currentStat);
                return;
            }
        }

        BuffIconUI newIcon = Instantiate(buffIconPrefab, relicContainer);

        newIcon.Setup(
            itemData.icon,
            itemData.itemShopName,
            itemData.description,
            currentStat,
            relicToolTip,
            true
        );

        relicIconList.Add(newIcon);
    }

    public void RemoveRelicIcon(ConsumableItemData itemData)
    {
        if (itemData == null) return;

        for (int i = relicIconList.Count - 1; i >= 0; i--)
        {
            if (relicIconList[i] != null && relicIconList[i].GetName == itemData.itemShopName)
            {
                Destroy(relicIconList[i].gameObject);
                relicIconList.RemoveAt(i);
                return;
            }
        }

        Debug.LogWarning("RemoveRelicIcon: could not find icon for " + itemData.itemName);
    }

    public void UpdateHealth(float newHealth)
    {
        healthBar.SetHealth(newHealth);
    }

    public void UpdateMaxHealth(float newMaxHealth)
    {
        healthBar.SetMaxHealth(newMaxHealth);
    }

    public void UpdateCoinCounter(int coinCounter)
    {
        coinCounterObj.UpdateCoinCounter(coinCounter);
    }

    public void UpdateKeyCounter(int keyCounter)
    {
        keyCounterObj.UpdateKeyCounter(keyCounter);
    }

    public void UpdateFloorCounter(int floorCounter)
    {
        floorCounterObj.UpdateFloorCounter(floorCounter);
    }
}
