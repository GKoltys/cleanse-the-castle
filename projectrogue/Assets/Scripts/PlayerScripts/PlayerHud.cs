using System.Collections.Generic;
using UnityEngine;

public class PlayerHud : MonoBehaviour
{
    private PlayerStats playerStats;
    private PlayerBase playerBase;
    [SerializeField] private HealthBarUI healthBar;
    [SerializeField] private CoinCounterUI coinCounterObj;
    [SerializeField] private KeyCounterUI keyCounterObj;
    [SerializeField] private Transform buffContainer;
    [SerializeField] private BuffIconUI buffIconPrefab;
    [SerializeField] private BuffToolTipUI buffToolTip;
    private readonly List<BuffIconUI> buffIconList = new();

    [SerializeField] private List<ConsumableItemData> buffList;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerBase = GetComponent<PlayerBase>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar.SetMaxHealth(playerStats.GetMaxHealth);
        healthBar.SetHealth(playerStats.GetHealth);
        coinCounterObj.UpdateCoinCounter(playerStats.GetCoinCount);
        keyCounterObj.UpdateKeyCounter(playerStats.GetKeyCount);
    }

    public void SetHudOnLoad(float maxHealth, float health, int coinCounter, int keyCounter)
    {
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(health);
        coinCounterObj.UpdateCoinCounter(coinCounter);
        keyCounterObj.UpdateKeyCounter(keyCounter);

        foreach (ConsumableItemData buff in buffList)
        {
            float current = GetStatValue(buff.statType);
            if (current != buff.baseStat) AddBuffIcon(buff);
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
        newIcon.Setup(itemData.icon, itemData.itemName, itemData.statDescription, currentStat, buffToolTip);

        buffIconList.Add(newIcon);
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
}
