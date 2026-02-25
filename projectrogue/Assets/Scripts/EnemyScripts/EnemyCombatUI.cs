using System.Collections;
using UnityEngine;

// https://www.youtube.com/watch?v=6gAmI5fOELY
public class EnemyCombatUI : MonoBehaviour
{
    [SerializeField] private float animationSpeed = 10f;
    private EnemyBase enemy;
    private float MaxValue;
    private float Value;

    [SerializeField] private RectTransform topBar;
    [SerializeField] private RectTransform bottomBar;

    private float fullWidth;
    private float TargetWidth => Value * fullWidth / MaxValue;
    private Coroutine adjustBarWidthCoroutine;

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyBase>();
        MaxValue = enemy.MaxHealth;
        Value = MaxValue;
    }

    private void Start()
    {
        fullWidth = topBar.rect.width;
    }

    public void UpdateHealthBarOnTakeDamage(float amount)
    {
        Value = Mathf.Clamp(Value - amount, 0f, MaxValue);
        if (adjustBarWidthCoroutine != null)
        {
            StopCoroutine(adjustBarWidthCoroutine);
        }
        adjustBarWidthCoroutine = StartCoroutine(AdjustBarWidth(-amount));
    }

    private IEnumerator AdjustBarWidth(float amount)
    {
        var suddenChangeBar = amount >= 0 ? bottomBar : topBar;
        var slowChangeBar = amount >= 0 ? topBar : bottomBar;
        suddenChangeBar.SetWidth(TargetWidth);
        while (Mathf.Abs(suddenChangeBar.rect.width - slowChangeBar.rect.width) > 1f) // 1f is threshold
        {
            slowChangeBar.SetWidth(Mathf.Lerp(slowChangeBar.rect.width, TargetWidth, Time.deltaTime * animationSpeed));
            yield return null;
        }
        slowChangeBar.SetWidth(TargetWidth);
    }

    public void ShowDamagePopUp(float damageAmount)
    {
        DamagePopUp.Create(transform.position, damageAmount);
    }
}
