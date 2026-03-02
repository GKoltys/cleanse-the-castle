using UnityEngine;

public class KeyController : MonoBehaviour, IMapGenInit
{
    public bool isCollected;
    private Animator animator;
    private BoxCollider2D col;
    private MapGenerator dungeon;
    private PlayerBase playerBase;

    private void Start()
    {
        playerBase = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBase>();
    }

    public void Init(MapGenerator controller)
    {
        dungeon = controller;
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dungeon == null) return;

        if (!other.CompareTag("Player")) return;
        if (col) col.enabled = false;

        SetIsCollected(true);
        playerBase.KeyCollected();
        animator.SetTrigger("KeyCollected");
    }

    public void SetIsCollected(bool opened)
    {
        isCollected = opened;
    }

    // Called using KeyCollected animation event
    public void Despawn()
    {
        // destroy for now, if it affects performance move to object pooling?
        Destroy(gameObject);
    }
}
