using UnityEngine;
using UnityEngine.SceneManagement;

// Health stats will be saved from here to JSON
public class PlayerHealth : MonoBehaviour
{
    private float iFrameSeconds;
    private float maxHealth;
    private float health;

    private float nextDamageTime;
    private bool isDead = false;

    private Animator animator;
    private PlayerMovement movement;
    private PlayerHud playerHud;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        playerHud = GetComponent<PlayerHud>();

        PlayerStats stats = GetComponent<PlayerStats>();
        iFrameSeconds = stats.GetIFrameSeconds;
        maxHealth = stats.GetMaxHealth;
        health = stats.GetHealth;
    }

    public void TakeDamage(float amount)
    {
        if (Time.time < nextDamageTime) return;

        nextDamageTime = Time.time + iFrameSeconds;
        health -= amount;
        playerHud.UpdateHealth(health);

        Debug.Log("Hurt " + health);

        animator.SetTrigger("Hurt");



        if (health <= 0)
        {
            isDead = true;
            Die();
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        playerHud.UpdateHealth(health);

        Debug.Log("Healed " + health);

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    private void Die()
    {
        movement.SetCanMove(false);
        animator.SetTrigger("Dying");
    }

    public void TriggerSceneReload()
    {
        animator.SetTrigger("FinishAnimations");
        // This should wipe save file to default in SaveController
        // SaveController.Instance.StartNewRun();
        SceneManager.LoadScene(1);
    }

    // Getter
    public bool GetIsDead => isDead;
}
