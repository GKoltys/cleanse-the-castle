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
    [SerializeField] private MeleeWeapon weapon;

    void Start()
    {
        // Load saved data to fields
    }

    // void savePlayerStats()

    // Getters
    public float GetSpeed => speed;
    public float GetIFrameSeconds => iFrameSeconds;
    public float GetMaxHealth => maxHealth;
    public float GetHealth => health;
    public int GetCoinCount => coinCount;
    public MeleeWeapon GetWeapon => weapon;
}
