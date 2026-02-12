using System.Runtime.CompilerServices;
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

    public virtual bool TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log(health);

        animator.SetTrigger("Hurt");

        if (health <= 0f)
        {
            DieAnimation();
            return false;
        }

        return true;
    }

    protected virtual void DieAnimation()
    {
        movement.SetCanMove(false);
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
    public float Damge => damage;
    public float MoveSpeed => moveSpeed;
    public float AgroRadius => agroRadius;
}
