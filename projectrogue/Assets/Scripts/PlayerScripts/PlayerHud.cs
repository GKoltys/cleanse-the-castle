using UnityEngine;

public class PlayerHud : MonoBehaviour
{
    public float Health, MaxHealth;
    public int CoinCounter = 10;

    [SerializeField]  private HealthBarUI healthBar;
    [SerializeField]  private CoinCounterUI coinCounterObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar.SetMaxHealth(MaxHealth);
        coinCounterObj.SetCoins(CoinCounter);
    }

    public void SetHealth(float healthChange)
    {
        Health += healthChange;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        healthBar.SetHealth(Health);
    }

    // TODO:
    // Here we need to add conditions for receiving damage/gaining health
    // Need to add functionality for picking up coins using coinCounter.UpdateCounter()
}
