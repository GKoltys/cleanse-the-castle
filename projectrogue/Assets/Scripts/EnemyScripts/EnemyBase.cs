using System;
using UnityEngine;

public abstract class EnemyBase: MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float damage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float agroRadius;

    protected float health;
    protected Animator animator;

    protected virtual void Awake()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
    }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0f)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public float MaxHealth => maxHealth;
    public float Damge => damage;
    public float MoveSpeed => moveSpeed;
    public float AgroRadius => agroRadius;
}
