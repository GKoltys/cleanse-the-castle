using UnityEngine;
using UnityEngine.SceneManagement;

// Health stats will be saved from here to JSON
public class PlayerBase : MonoBehaviour
{
    [Header("Stats and Equipment (Current)")]
    [SerializeField] private int floorCount;
    [SerializeField] private float speed;
    [SerializeField] private float iFrameSeconds;
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
    [SerializeField] private int coinCount;
    [SerializeField] private int keyCount;
    [SerializeField] private MeleeWeapon weapon;
    [SerializeField] private float damageMultiplier;

    [SerializeField] private WeaponDatabase weaponDatabase;

    private float nextDamageTime;
    private bool isDead = false;

    private Animator animator;
    private PlayerMovement movement;
    private PlayerHud playerHud;
    private PlayerCombat combat;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        playerHud = GetComponent<PlayerHud>();
        weapon = GetComponentInChildren<MeleeWeapon>();
        combat = GetComponent<PlayerCombat>();

        PlayerStats stats = GetComponent<PlayerStats>();
        floorCount = stats.GetFloorCount;
        speed = stats.GetSpeed;
        iFrameSeconds = stats.GetIFrameSeconds;
        maxHealth = stats.GetMaxHealth;
        health = stats.GetHealth;
        coinCount = stats.GetCoinCount;
        keyCount = stats.GetKeyCount;
        weapon.SetWeaponData(weaponDatabase.GetWeaponById(stats.GetWeapon));
        damageMultiplier = stats.GetDamageMultiplier;
    }

    public void ApplyLoadedStats(PlayerStats stats)
    {
        floorCount = stats.GetFloorCount;
        speed = stats.GetSpeed;
        iFrameSeconds = stats.GetIFrameSeconds;
        maxHealth = stats.GetMaxHealth;
        health = stats.GetHealth;
        coinCount = stats.GetCoinCount;
        keyCount = stats.GetKeyCount;
        weapon.SetWeaponData(weaponDatabase.GetWeaponById(stats.GetWeapon));
        SetDamageMultiplier(stats.GetDamageMultiplier);

        // Update defaulted values from before load
        movement.SetMoveSpeed(speed);
        playerHud.SetHudOnLoad(maxHealth, health, coinCount, keyCount);
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

    public void CoinCollected(int value)
    {
        coinCount += value;
        Debug.Log("Current coins: " +  coinCount);
        playerHud.UpdateCoinCounter(coinCount);
    }

    public void CoinSpent(int value)
    {
        coinCount -= value;
        playerHud.UpdateCoinCounter(coinCount);
    }

    public bool SpendGold(int amount)
    {
        if (coinCount >= amount)
        {
            CoinSpent(amount);
            return true; // spent gold
        }
        return false; // not enough gold
    }

    public void KeyCollected()
    {
        keyCount += 1;
        Debug.Log("Current keys: " + keyCount);
        playerHud.UpdateKeyCounter(keyCount);
    }

    public void KeyUsed()
    {
        keyCount -= 1;
        playerHud.UpdateKeyCounter(keyCount);
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
        SceneManager.LoadScene(2);
    }

    // Setters
    public void SetFlootCount(int floorCount) { this.floorCount = floorCount; }
    public void SetSpeed(float speed)
    {
        this.speed =  speed;
        movement.SetMoveSpeed(speed);
    }
    public void SetIFrameSeconds(float seconds) { this.iFrameSeconds = seconds; }
    public void SetMaxHealth(float maxHealth) { this.maxHealth = maxHealth; } // might need to update hud from here
    public void SetHealth(float health) { this.health = health; }
    public void SetCointCount(int coinCount)
    {
        this.coinCount = coinCount;
        playerHud.UpdateCoinCounter(coinCount);
    }
    public void SetKeyCount(int keyCount)
    {
        this.keyCount = keyCount;
        playerHud.UpdateKeyCounter(keyCount);
    }
    public void SetWeapon(WeaponData weaponData) { weapon.SetWeaponData(weaponData); }
    public void SetDamageMultiplier(float damageMultiplier)
    {
        this.damageMultiplier = damageMultiplier;
        combat.SetDamageMultiplier(damageMultiplier);
    }

    // Getter
    public bool GetIsDead => isDead;
    public int GetFloorCount => floorCount;
    public float GetSpeed => speed;
    public float GetIFrameSeconds => iFrameSeconds;
    public float GetMaxHealth => maxHealth;
    public float GetHealth => health;
    public int GetCoinCount => coinCount;
    public int GetKeyCount => keyCount;
    public MeleeWeapon GetWeapon => weapon;
    public int GetWeaponId => weapon.WeaponId;
    public float GetDamageMultiplier => damageMultiplier;
}
