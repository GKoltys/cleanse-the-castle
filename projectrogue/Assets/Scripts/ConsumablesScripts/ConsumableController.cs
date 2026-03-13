using UnityEngine;

public abstract class ConsumableController : MonoBehaviour
{
    [SerializeField] private ConsumableItemData itemData;

    protected bool isCollected;
    protected Animator animator;
    protected BoxCollider2D col;
    protected PlayerApplyEffect player;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
    }
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerApplyEffect>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerPickUp")) return;
        if (col) col.enabled = false;

        SetIsCollected(true);
        itemData.effect.Apply(player);
        animator.SetTrigger("Collected");
    }

    protected virtual void SetIsCollected(bool opened)
    {
        isCollected = opened;
    }

    // Called using animation event
    protected virtual void Despawn()
    {
        // destroy for now, if it affects performance move to object pooling?
        Destroy(gameObject);
    }

}
