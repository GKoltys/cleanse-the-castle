using UnityEngine;

public abstract class KnightAttack: EnemyAttack
{
    [SerializeField] protected float attackRadius = 0.8f;
    [SerializeField] protected float attackDistance = 0.8f;
    [SerializeField] protected LayerMask playerLayer;

    protected Rigidbody2D rb;
    protected Vector2 attackDirection;
    protected Vector2 dashDirection;
    [SerializeField] protected int dashChainCount = 1;
    [SerializeField] protected float dashForce = 10f;
    protected int remainingDashes;
    protected bool isDashing;
    protected bool isAttacking;

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
                return;
            }

            DashChainController();
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

    // Called only when player is in swipe range
    protected virtual void RollAttack()
    {
        int attackChoice = UnityEngine.Random.Range(1, 4);

        // Prioritising swipes up close with a chance of a dash
        if (attackChoice == 1)
        {
            DashChainController();
        }
        else
        {
            attackDirection = (playerTransform.position - transform.position).normalized;
            animator.SetTrigger("AttackSwipe");
        }
    }

    // Called by animation event
    protected virtual void StopUpdatingMovement()
    {
        enemy.movement.SetCanMove(false);
    }

    protected virtual void UpdateMovement()
    {
        rb.linearVelocity = Vector2.zero;
        DoNextDash();
    }

    // Called by animation event
    protected virtual void SwipeAttack()
    {
        Vector2 attackPoint = (Vector2)transform.position + attackDirection * attackDistance;

        Collider2D hit = Physics2D.OverlapBox(attackPoint, new Vector2(1, 1), 0f, playerLayer);

        if (hit != null)
        {
            playerHealth.TakeDamage(30, enemy);
        }
    }

    protected virtual void SwipeAttackEnd()
    {
        isAttacking = false;
        animator.SetTrigger("EndAttack");
        enemy.movement.SetCanMove(true);
        nextAttackTime = Time.time + attackCooldown;
    }

    // Called by animation event
    protected virtual void DashAttackStart()
    {
        isDashing = true;
        rb.linearVelocity = dashDirection * dashForce;
    }

    protected virtual void DashChainController()
    {
        remainingDashes = dashChainCount;

        DoNextDash();
    }

    protected void DoNextDash()
    {
        if (remainingDashes <= 0)
        {
            DashAttackEnd();
            return;
        }

        Debug.Log("Remaining Dashes: " + remainingDashes);
        remainingDashes--;

        dashDirection = (playerTransform.position - transform.position).normalized;
        animator.SetTrigger("AttackDash");
    }

    protected void EndChainDash()
    {
        isDashing = false;

        DoNextDash();
    }

    // Called by animation event
    protected virtual void DashAttackEnd()
    {
        isAttacking = false;
        animator.SetTrigger("EndAttack");
        enemy.movement.SetCanMove(true);
        nextAttackTime = Time.time + attackCooldown;
    }
}
