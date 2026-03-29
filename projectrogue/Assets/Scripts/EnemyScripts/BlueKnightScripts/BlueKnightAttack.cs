using System;
using UnityEngine;

public class BlueKnightAttack : EnemyAttack
{
    protected override void Update()
    {
        if (playerTransform == null || enemy == null) return;

        float dist = Vector2.Distance(transform.position, playerTransform.position);
        bool inRange = (dist <= attackRange) && !(playerHealth.GetIsDead);

        animator.SetBool("InMeleeRange", inRange);

        if (inRange && Time.time >= nextAttackTime && enemy.IsAlive)
        {
            nextAttackTime = Time.time + attackCooldown;
            RollAttack();
        }
    }

    private void RollAttack()
    {
        int attackChoice = UnityEngine.Random.Range(1, 2);

        if (attackChoice == 1)
        {
            animator.SetTrigger("Attack1");
        }
        else if (attackChoice == 2)
        {
            animator.SetTrigger("Attack2");
        }
    }

    private void Attack1()
    {

    }

    private void Attack2()
    {

    }
}
