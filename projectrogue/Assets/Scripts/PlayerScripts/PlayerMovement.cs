using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed;
    private Rigidbody2D rb;

    private Vector2 moveInput;
    private Vector2 lastMoveDirection;
    private Animator animator;

    private bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        PlayerStats stats = GetComponent<PlayerStats>();
        if (stats != null)
        {
            moveSpeed = stats.GetSpeed;
        }
    }

    private void OnMove(InputValue value)
    {
        if (canMove)
        {
            moveInput = value.Get<Vector2>();

            if (moveInput.sqrMagnitude > 0.01f)
            {
                lastMoveDirection = moveInput.normalized;

                if (animator != null && animator.runtimeAnimatorController != null)
                {
                    animator.SetFloat("LastInputX", lastMoveDirection.x);
                    animator.SetFloat("LastInputY", lastMoveDirection.y);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            // Movement
            Vector2 direction = moveInput.normalized; // "normalized" prevents faster diagonal movement
            if (rb != null)
                rb.linearVelocity = direction * moveSpeed;

            bool isMoving = moveInput.sqrMagnitude > 0.01f;

            if (animator != null && animator.runtimeAnimatorController != null)
            {
                animator.SetBool("IsRunning", isMoving);
                animator.SetFloat("InputX", moveInput.x);
                animator.SetFloat("InputY", moveInput.y);
            }
        }
        else
        {
            if (rb != null)
                rb.linearVelocity = Vector2.zero;

            if (animator != null && animator.runtimeAnimatorController != null)
            {
                animator.SetBool("IsRunning", false);
                animator.SetFloat("InputX", 0f);
                animator.SetFloat("InputY", 0f);
            }
        }
    }

    public void SetCanMove(bool flag)
    {
        canMove = flag;
        moveInput = Vector2.zero;
    }

    public bool CanMove()
    {
        return canMove;
    }

    public void SetMoveSpeed(float speed) { moveSpeed = speed; }
}