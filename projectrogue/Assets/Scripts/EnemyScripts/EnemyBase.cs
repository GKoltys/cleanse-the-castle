using UnityEngine;

public abstract class EnemyBase: MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float damage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float agroRadius;

    protected float health;
    public Animator animator;
    public EnemyMovement movement;

    protected virtual void Awake()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        movement = GetComponent<EnemyMovement>();
    }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;

        animator.SetTrigger("Hurt");

        if (health <= 0f)
        {
            Die();
        }

        Debug.Log(health);
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public void OnHurtFinished()
    {
        movement.SetCanMove(true);
    }

    public float MaxHealth => maxHealth;
    public float Damge => damage;
    public float MoveSpeed => moveSpeed;
    public float AgroRadius => agroRadius;
}
