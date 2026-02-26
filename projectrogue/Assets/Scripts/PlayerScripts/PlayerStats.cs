using UnityEngine;

// Keep this as a data container
// Classes will read from here at Awake() and then save new values to themselves
// Their values will be saved to a JSON and read back to here on load
public class PlayerStats : MonoBehaviour
{
    // Defaults
    [Header("Stats and Equipment (Last saved)")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float iFrameSeconds = 0.5f;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float health = 100;
    [SerializeField] private int coinCount = 0;
    [SerializeField] private int weaponId;

    public void SetSpeed(float speed) { this.speed = speed; }
    public void SetIFrameSeconds(float iFrameSeconds) { this.iFrameSeconds = iFrameSeconds; }
    public void SetMaxHealth(float maxHealth) { this.maxHealth = maxHealth; }
    public void SetHealth(float  health) { this.health = health; }
    public void SetCoinCount(int coinCount) { this.coinCount = coinCount; }
    public void SetWeapon(int weapon) { this.weaponId = weapon; }

    // Getters
    public float GetSpeed => speed;
    public float GetIFrameSeconds => iFrameSeconds;
    public float GetMaxHealth => maxHealth;
    public float GetHealth => health;
    public int GetCoinCount => coinCount;
    public int GetWeapon => weaponId;
}
