using System.Runtime.CompilerServices;
using UnityEngine;

public class BlueKnightAttack : EnemyAttack
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private LayerMask playerLayer;

    private Rigidbody2D rb;
    private Vector2 dashDirection;
    [SerializeField] private float dashForce = 10f;
    private bool isDashing;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        if (playerTransform == null || enemy == null) return;

        float dist = Vector2.Distance(transform.position, playerTransform.position);
        bool inRange = (dist <= attackRange) && !(playerHealth.GetIsDead);

        if (inRange && Time.time >= nextAttackTime && enemy.IsAlive)
        {
            nextAttackTime = Time.time + attackCooldown;
            RollAttack();
        }
    }

    private void RollAttack()
    {
        int attackChoice = UnityEngine.Random.Range(1, 3);

        if (attackChoice == 1)
        {
            animator.SetTrigger("AttackSwipe");
        }
        else if (attackChoice == 2)
        {
            animator.SetTrigger("AttackDash");
        }
    }

    // Called by animation event
    private void SwipeAttack()
    {
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRadius, playerLayer);

        if (hit != null)
        {
            playerHealth.TakeDamage(enemy.Damage, enemy);
        }
    }

    // Called by animation event
    private void DashAttackStart()
    {
        isDashing = true;
        dashDirection = (playerTransform.position - transform.position).normalized;

        rb.linearVelocity = dashDirection * dashForce;

        while (isDashing)
        {
            Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRadius, playerLayer);

            if (hit != null)
            {
                playerHealth.TakeDamage(enemy.Damage, enemy);
            }
        }
    }

    private void DashAttackEnd()
    {
        isDashing = false;
    }
}
