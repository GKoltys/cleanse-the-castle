using UnityEngine;

// https://www.youtube.com/watch?v=lYZayXViTN8

public class HealthBarUI : MonoBehaviour
{
    public float Health, MaxHealth, Width, Height;

    [SerializeField]
    private RectTransform HealthBar;

    public void SetMaxHealth(float maxHealth)
    {
        MaxHealth = maxHealth;
    }

    public void SetHealth(float health)
    {
        Health = health;
        float newWidth = (Health / MaxHealth) * Width;

        HealthBar.sizeDelta = new Vector2(newWidth, Height);
    }
}
