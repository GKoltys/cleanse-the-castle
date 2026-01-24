using UnityEngine;

public class SpikeController : MonoBehaviour, IMapGenInit
{
    private Animator animator;
    private Collider2D col;
    private MapGenerator dungeon;

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

        if (animator != null)
        {
            // animation transitions - https://docs.unity3d.com/6000.3/Documentation/Manual/class-Transition.html
            animator.SetBool("OnSpike", true);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (animator != null)
        {
            animator.SetBool("OnSpike", false);
        }
    }

}
