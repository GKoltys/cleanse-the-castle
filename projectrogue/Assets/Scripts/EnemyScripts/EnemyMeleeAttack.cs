using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof(EnemyBase))]
public abstract class EnemyAttack : MonoBehaviour
{
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float attackRange;

    protected EnemyBase enemy;
    protected Animator animator;
    protected PlayerBase playerHealth;
    protected Transform playerTransform;
    [DoNotSerialize] public float nextAttackTime;

    protected virtual void Awake()
    {
        enemy = GetComponent<EnemyBase>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerHealth = player.GetComponent<PlayerBase>();
        }
    }

    protected virtual void Update()
    {
        if (playerTransform == null || enemy == null) return;

        float dist = Vector2.Distance(transform.position, playerTransform.position);
        bool inRange = (dist <= attackRange) && !(playerHealth.GetIsDead);

        animator.SetBool("InMeleeRange", inRange);

        if (inRange && Time.time >= nextAttackTime && enemy.IsAlive)
        {
            nextAttackTime = Time.time + attackCooldown;
            Attack();
        }
    }

    protected virtual void Attack()
    {
        playerHealth.TakeDamage(enemy.Damage);
    }
}
