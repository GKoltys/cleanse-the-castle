using UnityEngine;

public class BlueKnightAttack : EnemyAttack
{
    [SerializeField] private float attackRadius = 0.8f;
    [SerializeField] private float attackDistance = 0.8f;
    [SerializeField] private LayerMask playerLayer;

    private Rigidbody2D rb;
    private Vector2 attackDirection;
    private Vector2 dashDirection;
    [SerializeField] private float dashForce = 10f;
    private bool isDashing;
    private bool isAttacking;

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

        if (inRange && Time.time >= nextAttackTime && enemy.IsAlive && !isAttacking)
        {
            isAttacking = true;

            nextAttackTime = Time.time + attackCooldown;

            if (dist <= attackRadius + attackDistance)
            {
                RollAttack();
                Debug.Log("Rolling attack");
                return;
            }

            Debug.Log("Dashing");
            dashDirection = (playerTransform.position - transform.position).normalized;
            animator.SetTrigger("AttackDash");
        }

        // Handles dash attack
        if (isDashing)
        {
            Vector2 attackPoint = (Vector2)transform.position + dashDirection * attackDistance;

            //Collider2D hit = Physics2D.OverlapCircle(attackPoint, attackRadius, playerLayer);
            Collider2D hit = Physics2D.OverlapBox(attackPoint, new Vector2(1, 1), 0f, playerLayer);

            if (hit != null)
            {
                playerHealth.TakeDamage(50, enemy);
            }
        }
    }

    private void RollAttack()
    {
        int attackChoice = UnityEngine.Random.Range(1, 4);

        if (attackChoice == 1)
        {
            dashDirection = (playerTransform.position - transform.position).normalized;
            animator.SetTrigger("AttackDash");
            
        }
        else
        {
            attackDirection = (playerTransform.position - transform.position).normalized;
            animator.SetTrigger("AttackSwipe");
        }
    }

    // Called by animation event
    private void StopUpdatingMovement()
    {
        enemy.movement.SetCanMove(false);
    }

    // Called by animation event
    private void SwipeAttack()
    {
        Vector2 attackPoint = (Vector2)transform.position + attackDirection * attackDistance;

        Collider2D hit = Physics2D.OverlapBox(attackPoint, new Vector2(1, 1), 0f, playerLayer);

        if (hit != null)
        {
            playerHealth.TakeDamage(30, enemy);
        }
    }

    private void SwipeAttackEnd()
    {
        isAttacking = false;
        animator.SetTrigger("EndAttack");
        enemy.movement.SetCanMove(true);
        nextAttackTime = Time.time + attackCooldown;
    }

    // Called by animation event
    private void DashAttackStart()
    {
        isDashing = true;
        rb.linearVelocity = dashDirection * dashForce;
    }

    private void DashAttackEnd()
    {
        isDashing = false;
        isAttacking = false;
        animator.SetTrigger("EndAttack");
        enemy.movement.SetCanMove(true);
        nextAttackTime = Time.time + attackCooldown;
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 attackPoint = (Vector2)transform.position + attackDirection * attackDistance;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint, attackRadius);
    }
}
