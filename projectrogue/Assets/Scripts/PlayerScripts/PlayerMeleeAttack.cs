using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private float attackCooldown;

    private Camera cam;
    private Vector2 lastFacing = Vector2.down;
    private float nextAttackTime;

    private MeleeWeapon weapon;
    private Animator animator;

    private void Awake()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
        weapon = GetComponent<PlayerBase>().GetWeapon;

        attackCooldown = weapon.AttackCooldown;
    }

    public void OnAttack()
    {
        if (weapon == null) return;

        if (Time.time < nextAttackTime) return;
        nextAttackTime = Time.time + attackCooldown;

        Vector2 direction = GetMouseDirection();
        lastFacing = direction;

        animator.SetFloat("AttackDirX", direction.x);
        animator.SetFloat("AttackDirY", direction.y);
        animator.SetTrigger("Attack");

        animator.SetFloat("LastInputX", direction.x);
        animator.SetFloat("LastInputY", direction.y);
    }

    // https://discussions.unity.com/t/mouse-position-with-new-input-system/776798/14
    private Vector2 GetMouseDirection()
    {
        if (cam == null) cam = Camera.main;

        Vector3 world = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 raw = (Vector2)(world - transform.position);
       
        if (raw.sqrMagnitude < 0.0001f) return lastFacing; // if mouse is directly on character

        // forces direction to either be +-1 or 0 to work with animations
        if (Mathf.Abs(raw.x) > Mathf.Abs(raw.y))
            return new Vector2(Mathf.Sign(raw.x), 0f);
        else
            return new Vector2(0f, Mathf.Sign(raw.y));
    }

    // Called by attackAnimation event
    public void ApplyAttackHit()
    {
        weapon.Attack(lastFacing, transform.position);
    }
}
