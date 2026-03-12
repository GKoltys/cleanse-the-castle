using UnityEngine;

public class PlayerApplyEffect : MonoBehaviour
{
    private PlayerBase player;
    private PlayerHud playerHud;

    private void Awake()
    {
        player = GetComponent<PlayerBase>();
        playerHud = GetComponent<PlayerHud>();
    }
    public void Heal(float amount)
    {
        float maxHealth = player.GetMaxHealth;
        float health = player.GetHealth + amount;

        Debug.Log("Healed " + amount);

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        playerHud.UpdateHealth(health);
    }

    public void AddPermanentSpeedBoost(float speedIncrease)
    {
        player.SetSpeed(player.GetSpeed + speedIncrease);
        Debug.Log("Speed boosted to " + player.GetSpeed + " by +" +  speedIncrease);
    }
}
