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
        moveSpeed = GetComponent<PlayerStats>().GetSpeed;
    }

    private void OnMove(InputValue value)
    {
        if (canMove)
        {
            moveInput = value.Get<Vector2>();

            if (moveInput.sqrMagnitude > 0.01f)
            {
                lastMoveDirection = moveInput.normalized;

                animator.SetFloat("LastInputX", lastMoveDirection.x);
                animator.SetFloat("LastInputY", lastMoveDirection.y);
            }
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            // Movement
            Vector2 direction = moveInput.normalized; // "normalized" prevents faster diagonal movement
            rb.linearVelocity = direction * moveSpeed;

            // Animation
            bool isMoving = moveInput.sqrMagnitude > 0.01f;
            animator.SetBool("IsRunning", isMoving);

            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;

            animator.SetBool("IsRunning", false);
            animator.SetFloat("InputX", 0f);
            animator.SetFloat("InputY", 0f);
        }
    }

    public void SetCanMove(bool flag)
    {
        canMove = flag;
        moveInput = Vector2.zero;
    }

    public void SetMoveSpeed(float speed) { moveSpeed = speed; }
}