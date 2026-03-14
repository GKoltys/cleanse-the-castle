using System.Collections.Generic;
using UnityEngine;

public class PlayerHud : MonoBehaviour
{
    private PlayerStats playerStats;
    [SerializeField]  private HealthBarUI healthBar;
    [SerializeField]  private CoinCounterUI coinCounterObj;
    [SerializeField]  private KeyCounterUI keyCounterObj;
    [SerializeField] private Transform buffContainer;
    [SerializeField] private BuffIconUI buffIconPrefab;
    private readonly List<BuffIconUI> buffIconList = new();

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
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
    }

    public void AddBuffIcon(ConsumableItemData itemData, float updatedStat)
    {
        if (itemData == null) return;

        foreach (BuffIconUI icon in buffIconList)
        {
            if (icon.name == itemData.name)
            {
                icon.SetStat(updatedStat);
                return;
            }
        }

        BuffIconUI newIcon = Instantiate(buffIconPrefab, buffContainer);

        newIcon.SetName(itemData.name);
        newIcon.SetIcon(itemData.icon);
        newIcon.SetStatDescription(itemData.statDescription);
        newIcon.SetStat(updatedStat);

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
