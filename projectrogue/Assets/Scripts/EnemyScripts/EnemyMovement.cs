using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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

    [Header("Patrol")]
    [SerializeField] private float minPatrolMoveTime = 1f;
    [SerializeField] private float maxPatrolMoveTime = 2.5f;
    [SerializeField] private float minPatrolIdleTime = 0.5f;
    [SerializeField] private float maxPatrolIdleTime = 1.5f;

    private float patrolTimer;
    private bool isPatrolling;

    [Header("Wall Detection")]
    [SerializeField] private float wallCheckDistance = 0.3f;
    [SerializeField] private LayerMask wallMask;

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
        target = GameObject.FindGameObjectWithTag("Player").transform;
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
        else
        {
            HandlePatrol();
        }
    }

    private void FixedUpdate()
    {
        if (canMove && enemy.IsAlive)
        {
            float distance = Vector2.Distance(target.position, transform.position);
            if ((distance <= agroRadius) && (Time.time >= attack.nextAttackTime) && (!playerHealth.GetIsDead))
            {
                rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
            }
            else if (distance >  agroRadius)
            {
                if (isPatrolling)
                {
                    rb.linearVelocity = moveDirection * moveSpeed;
                }
                else
                {
                    rb.linearVelocity = Vector2.zero;
                }
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

    private void HandlePatrol()
    {
        patrolTimer -= Time.deltaTime;

        if (isPatrolling && isWallAhead())
        {
            PickNewPatrolState();
            return;
        }

        if (patrolTimer <= 0f)
        {
            PickNewPatrolState();
        }

        if (isPatrolling)
        {
            lastMoveDirection = moveDirection;
        }
    }

    private void PickNewPatrolState()
    {
        isPatrolling = Random.value > 0.5f; // 50% chance to move without resting

        if (isPatrolling)
        {
            moveDirection = GetRandomDirection();
            patrolTimer = Random.Range(minPatrolMoveTime, maxPatrolMoveTime);
        }
        else
        {
            moveDirection = Vector2.zero;
            patrolTimer = Random.Range(minPatrolIdleTime, maxPatrolIdleTime);
        }
    }

    // https://discussions.unity.com/t/creating-a-random-direction-vector/524496
    private Vector2 GetRandomDirection()
    {
        return Random.insideUnitCircle.normalized;
    }

    // https://discussions.unity.com/t/how-to-detect-wall/737443/2
    private bool isWallAhead()
    {
        if (moveDirection == Vector2.zero) return false;

        RaycastHit2D hit = Physics2D.Raycast(
            rb.position,
            moveDirection,
            wallCheckDistance,
            wallMask // Could add "| enemyMask" to avoid other enemies too
        );

        return hit.collider != null;
    }

    public void SetCanMove(bool flag)
    {
        canMove = flag;
    }
}
