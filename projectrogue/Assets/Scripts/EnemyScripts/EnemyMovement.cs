using UnityEngine;
using UnityEngine.InputSystem;

// https://www.youtube.com/watch?v=m1x9YFzTX2A
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float agroRadius;
    private Rigidbody2D rb;
    private Transform target;

    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        float distance = Vector2.Distance(target.position, transform.position);
        if (distance <= agroRadius)
        {
            moveDirection = (target.position - transform.position).normalized;
            lastMoveDirection = moveDirection.normalized;
        }
    }

    private void FixedUpdate()
    {
        float distance = Vector2.Distance(target.position, transform.position);
        if (distance <= agroRadius)
        {
            rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        // Animation
        bool isMoving = rb.linearVelocity != Vector2.zero;
        animator.SetBool("IsRunning", isMoving);

        animator.SetFloat("InputX", moveDirection.x);
        animator.SetFloat("InputY", moveDirection.y);

        animator.SetFloat("LastInputX", lastMoveDirection.x);
        animator.SetFloat("LastInputY", lastMoveDirection.y);
    }
}
