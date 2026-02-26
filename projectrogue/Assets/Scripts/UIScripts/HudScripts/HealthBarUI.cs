using UnityEngine;

// https://www.youtube.com/watch?v=lYZayXViTN8

public class HealthBarUI : MonoBehaviour
{
    private float MaxHealth, Width, Height;
    private RectTransform HealthBar;
    private int mama;

    private void Awake()
    {
        HealthBar = GetComponent<RectTransform>();
        Width = HealthBar.rect.width;
        Height = HealthBar.rect.height;
    }

    public void SetMaxHealth(float maxHealth)
    {
        MaxHealth = maxHealth;
    }

    public void SetHealth(float health)
    {
        float newWidth = (health / MaxHealth) * Width;

        HealthBar.sizeDelta = new Vector2(newWidth, Height);
    }
}
