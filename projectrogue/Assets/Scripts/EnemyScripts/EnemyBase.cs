using UnityEngine;

public abstract class EnemyBase: MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float damage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float agroRadius;

    protected bool isAlive = true;
    protected float health;
    protected Animator animator;
    protected EnemyMovement movement;
    protected Rigidbody2D rb;

    protected EnemyCombatUI enemyCombatUI;

    protected virtual void Awake()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        movement = GetComponent<EnemyMovement>();
        rb = GetComponent<Rigidbody2D>();

        enemyCombatUI = GetComponentInChildren<EnemyCombatUI>();
    }

    public virtual bool TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log(health);

        animator.SetTrigger("Hurt");
        enemyCombatUI.UpdateHealthBarOnTakeDamage(amount);
        enemyCombatUI.ShowDamagePopUp(amount);

        if (health <= 0f)
        {
            isAlive = false;
            DeathAnimation();
            return false;
        }

        return true;
    }

    protected virtual void DeathAnimation()
    {
        movement.SetCanMove(false);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetTrigger("Dying");
    }

    protected virtual void DestroySelf()
    {
        Destroy(gameObject);
    }

    public virtual void OnHurtFinished()
    {
        movement.SetCanMove(true);
    }

    public float MaxHealth => maxHealth;
    public float Damage => damage;
    public float MoveSpeed => moveSpeed;
    public float AgroRadius => agroRadius;
    public bool IsAlive => isAlive;
}
