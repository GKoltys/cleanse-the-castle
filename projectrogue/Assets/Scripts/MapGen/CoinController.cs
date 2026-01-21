using UnityEngine;

public class CoinController : MonoBehaviour, IMapGenInit
{
    private Animator animator;
    private Collider2D col;
    private MapGenerator dungeon;
    private bool collected = false;

    public void Init(MapGenerator controller)
    {
        dungeon = controller;
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dungeon == null) return;

        if (!other.CompareTag("Player")) return;

        collected = true;
        if (col) col.enabled = false;

        if (animator != null && collected)
        {
            // animation transitions - https://docs.unity3d.com/6000.3/Documentation/Manual/class-Transition.html
            animator.SetTrigger("CoinGet");
        }

    }

    public void Despawn()
    {
        // destory for now, if it affects performance move to object pooling?
        Destroy(gameObject);
    }
}
