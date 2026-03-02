using UnityEngine;

public class PlayerHud : MonoBehaviour
{
    private PlayerStats playerStats;
    [SerializeField]  private HealthBarUI healthBar;
    [SerializeField]  private CoinCounterUI coinCounterObj;
    [SerializeField]  private KeyCounterUI keyCounterObj;

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

    public void UpdateHealth(float newHealth)
    {
        healthBar.SetHealth(newHealth);
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
