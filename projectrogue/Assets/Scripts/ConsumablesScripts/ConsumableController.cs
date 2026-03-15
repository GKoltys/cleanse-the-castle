using UnityEngine;

public abstract class ConsumableController : MonoBehaviour
{
    [SerializeField] protected ConsumableItemData itemData;

    protected bool isCollected;
    protected Animator animator;
    protected BoxCollider2D col;
    protected PlayerBase playerBase; 
    protected PlayerApplyEffect playerEffect;
    protected PlayerHud playerHud;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
    }
    protected virtual void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerBase = player.GetComponent<PlayerBase>();
        playerEffect = player.GetComponent<PlayerApplyEffect>();
        playerHud = player.GetComponent <PlayerHud>();

        AddBuffIconOnLoad();
    }

    // You need to override this for any buff consumable
    protected virtual void AddBuffIconOnLoad()
    {
        return;
    }

    protected abstract void OnTriggerEnter2D(Collider2D other);

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
