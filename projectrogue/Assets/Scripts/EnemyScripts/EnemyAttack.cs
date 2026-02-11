using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof(EnemyBase))]
public abstract class EnemyAttack : MonoBehaviour
{
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float attackRange;

    protected EnemyBase enemy;
    protected Transform playerTransform;
    [DoNotSerialize] public float nextAttackTime;

    protected virtual void Awake()
    {
        enemy = GetComponent<EnemyBase>();
    }

    protected virtual void Start()
    {
        var player = GameObject.FindWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

    protected virtual void Update()
    {
        if (playerTransform == null || enemy == null) return;

        float dist = Vector2.Distance(transform.position, playerTransform.position);
        bool inRange = dist <= attackRange;

        enemy.animator.SetBool("InMeleeRange", inRange);

        if (inRange && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            Attack();
        }
    }

    protected virtual void Attack()
    {
        // Player.takeDamage();
    }
}
