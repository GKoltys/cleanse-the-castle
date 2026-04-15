using Unity.VisualScripting;
using UnityEngine;

public class BlueKnightBossEnemy : EnemyBase, IMapGenInit
{
    private MapGenerator mapGenerator;
    private float timeAlive;

    private readonly string gameEvent = "Boss Defeated by Player";
    private readonly string boss = "Blue Knight";
    private string performance;
    private string playerHealth;
    private string duration;

    protected override void Awake()
    {
        base.Awake();
        timeAlive = Time.time;
    }

    public void Init(MapGenerator generator)
    {
        mapGenerator = generator;
    }

    public override bool TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log(health);
        playerRelics?.TriggerLifeSteal(amount);

        animator.SetTrigger("Hurt");
        enemyCombatUI.UpdateHealthBarOnTakeDamage(amount);
        enemyCombatUI.ShowDamagePopUp(amount);

        if (health <= 0f)
        {
            isAlive = false;
            timeAlive = Time.time - timeAlive;
            CalculatePromptInputs();

            string bossPrompt = AiController.Instance.BuildBossPrompt(gameEvent, boss, performance, playerHealth, duration);
            Debug.Log("Prompt to AI:\n\n" + bossPrompt);

            AiController.Instance.AskAi(bossPrompt);
            DeathAnimation();
            return false;
        }

        return true;
    }

    private void CalculatePromptInputs()
    {
        float healthPercentage = playerBase.GetHealth / playerBase.GetMaxHealth;

        switch (healthPercentage)
        {
            case > 0.8f:
                performance = "dominating";
                playerHealth = "high";
                break;

            case > 0.6f:
                performance = "confortable";
                playerHealth = "above half";
                break;

            case > 0.4f:
                performance = "struggling";
                playerHealth = "around half";
                break;

            case > 0.2f:
                performance = "barely survived";
                playerHealth = "low";
                break;

            default:
                performance = "near death";
                playerHealth = "critical";
                break;
        }

        switch (timeAlive)
        {
            case < 10f:
                duration = "very fast";
                break;

            case < 30f:
                duration = "moderate";
                break;

            default:
                duration = "slow";
                break;
        }
    }

    public override void Despawn()
    {
        mapGenerator?.OnBossDied();
        base.Despawn();
    }
}