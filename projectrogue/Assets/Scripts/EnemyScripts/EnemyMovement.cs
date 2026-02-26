using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// https://www.youtube.com/watch?v=m1x9YFzTX2A
public class EnemyMovement : MonoBehaviour
{
    private EnemyBase enemy;
    private EnemyAttack attack;
    private Rigidbody2D rb;
    private Transform target;
    private PlayerBase playerHealth;

    private float moveSpeed;
    private float agroRadius;

    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;
    private Animator animator;

    private bool canMove = true;

    private void Awake()
    {
        enemy = GetComponent<EnemyBase>();
        attack = GetComponent<EnemyAttack>();

        moveSpeed = enemy.MoveSpeed;
        agroRadius = enemy.AgroRadius;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        target = GameObject.Find("Player").transform;
        playerHealth = target.GetComponentInChildren<PlayerBase>();
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
        if (canMove)
        {
            float distance = Vector2.Distance(target.position, transform.position);
            if ((distance <= agroRadius) && (Time.time >= attack.nextAttackTime) && (!playerHealth.GetIsDead))
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
        else if (!enemy.IsAlive)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void SetCanMove(bool flag)
    {
        canMove = flag;
    }
}
