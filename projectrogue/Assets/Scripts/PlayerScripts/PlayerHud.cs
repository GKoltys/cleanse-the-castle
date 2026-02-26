using UnityEngine;

public class PlayerHud : MonoBehaviour
{
    private PlayerStats playerStats;
    [SerializeField]  private HealthBarUI healthBar;
    [SerializeField]  private CoinCounterUI coinCounterObj;

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
    }

    public void UpdateHealth(float newHealth)
    {
        healthBar.SetHealth(newHealth);
    }

    public void UpdateCoinCounter(int coinCounter)
    {
        coinCounterObj.UpdateCoinCounter(coinCounter);
    }
}
