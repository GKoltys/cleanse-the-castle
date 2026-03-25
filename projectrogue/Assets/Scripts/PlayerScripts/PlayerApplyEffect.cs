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

        SoundEffectManager.Play(SoundGroupName.HEAL);

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
        SoundEffectManager.Play(SoundGroupName.BUFF);
        Debug.Log("Speed changed to " + player.GetSpeed + " by +" +  speedChange);
    }

    public void ChangePlayerMaxHealth(float maxHealthChange)
    {
        float newMaxHealth = player.GetMaxHealth + maxHealthChange;

        player.SetMaxHealth(newMaxHealth);
        Heal(maxHealthChange);
        SoundEffectManager.Play(SoundGroupName.BUFF);

        playerHud.UpdateMaxHealth(newMaxHealth);
        playerHud.UpdateHealth(player.GetHealth);

        Debug.Log("MaxHealth changed to " + player.GetMaxHealth + " by +" + maxHealthChange);
    }

    public void ChangePlayerDamageMultiplier(float damageMultiplierChange)
    {
        player.SetDamageMultiplier(player.GetDamageMultiplier +  damageMultiplierChange);
        SoundEffectManager.Play(SoundGroupName.BUFF);
    }

    public void ChangePlayerDamageTakenMultiplier(float change)
    {
        player.SetDamageTakenMultiplier(player.GetDamageTakenMultiplier + change);
        Debug.Log("Damage taken multiplier changed to " + player.GetDamageTakenMultiplier);
    }
}
