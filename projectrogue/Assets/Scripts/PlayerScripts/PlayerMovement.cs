using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;

    private Vector2 moveInput;
    private Vector2 lastMoveDirection;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        if (moveInput.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = moveInput.normalized;
        }
    }

    private void FixedUpdate()
    {
        // Movement
        Vector2 direction = moveInput.normalized; // "normalized" prevents faster diagonal movement
        rb.linearVelocity = direction * moveSpeed;

        // Animation
        bool isMoving = moveInput != Vector2.zero;
        animator.SetBool("IsRunning", isMoving);

        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);

        animator.SetFloat("LastInputX", lastMoveDirection.x);
        animator.SetFloat("LastInputY", lastMoveDirection.y);
    }
}