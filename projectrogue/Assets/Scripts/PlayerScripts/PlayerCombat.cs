using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackCooldown = 0.5f; // should probably be set to the duration of the animation

    private Camera cam;
    private Vector2 lastFacing = Vector2.down;
    private float nextAttackTime;

    private PlayerMovement movement;
    private Animator animator;

    private void Awake()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }

    public void OnAttack()
    {
        if (Time.time < nextAttackTime) return;
        nextAttackTime = Time.time + attackCooldown;

        Vector2 direction = GetMouseDirection();
        lastFacing = direction;

        animator.SetFloat("AttackDirX", direction.x);
        animator.SetFloat("AttackDirY", direction.y);
        animator.SetTrigger("Attack");

        animator.SetFloat("LastInputX", direction.x);
        animator.SetFloat("LastInputY", direction.y);

        movement.SetCanMove(false);
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

    // Triggered by attack animation events
    public void OnAttackFinished()
    {
        movement.SetCanMove(true);
    }
}
