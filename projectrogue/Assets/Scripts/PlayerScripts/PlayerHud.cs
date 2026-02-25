using UnityEngine;

public class PlayerHud : MonoBehaviour
{
    private float Health, MaxHealth;
    private int CoinCounter;

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
        MaxHealth = playerStats.GetMaxHealth;
        Health = playerStats.GetHealth;
        CoinCounter = playerStats.CoinCount;

        healthBar.SetMaxHealth(MaxHealth);
        healthBar.SetHealth(Health);

        coinCounterObj.SetCoins(CoinCounter);
    }

    public void UpdateHealth(float newHealth)
    {
        Health = Mathf.Clamp(newHealth, 0, MaxHealth);

        healthBar.SetHealth(Health);
    }

    // TODO:
    // Here we need to add conditions for receiving damage/gaining health
    // Need to add functionality for picking up coins using coinCounter.UpdateCounter()
}
