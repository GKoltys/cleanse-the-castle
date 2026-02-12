using System.Collections;
using UnityEngine;

// https://www.youtube.com/watch?v=ahadN8aGvXg
[RequireComponent (typeof(Rigidbody2D))]
public class EnemyKnockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private EnemyMovement movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<EnemyMovement>();
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        rb.linearVelocity = Vector2.zero;

        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);

        movement.SetCanMove(false);
    }
}
