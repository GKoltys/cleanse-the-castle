using System;
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

        player.SetHealth(health);
        playerHud.UpdateHealth(health);
    }

    public void ChangePlayerSpeed(float speedChange)
    {
        player.SetSpeed(player.GetSpeed + speedChange);
        Debug.Log("Speed changed to " + player.GetSpeed + " by +" +  speedChange);
    }

    public void ChangePlayerMaxHealth(float maxHealthChange)
    {
        float newMaxHealth = player.GetMaxHealth + maxHealthChange;

        player.SetMaxHealth(newMaxHealth);
        Heal(maxHealthChange);

        playerHud.UpdateMaxHealth(newMaxHealth);
        playerHud.UpdateHealth(player.GetHealth);

        Debug.Log("MaxHealth changed to " + player.GetMaxHealth + " by +" + maxHealthChange);
    }

    public void ChangePlayerDamageMultiplier(float damageMultiplierChange)
    {
        player.SetDamageMultiplier(player.GetDamageMultiplier +  damageMultiplierChange);
    }
}
