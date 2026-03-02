using UnityEngine;

public class KeyController : MonoBehaviour, IMapGenInit
{
    public bool isCollected;
    public GameObject interactionIcon;
    private Animator animator;
    private MapGenerator dungeon;
    private PlayerBase playerBase;
    public GameObject lastInteractor;

    private void Start()
    {
        playerBase = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBase>();
    }
    public void Init(MapGenerator controller)
    {
        dungeon = controller;
        animator = GetComponent<Animator>();
    }

    // set the chest to closed on spawn
    private void Awake()
    {
        interactionIcon.SetActive(false);
    }

    public bool CanInteract()
    {
        return !isCollected;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dungeon == null) return;

        if (!other.CompareTag("Player")) return;

        CollectKey();
        // Trigger animations
    }

    private void CollectKey()
    {
        SetIsCollected(true);
        // add functionality for collecting key
        Despawn();
    }

    public void SetIsCollected(bool opened)
    {
        isCollected = opened;
    }


    public void Despawn()
    {
        // destroy for now, if it affects performance move to object pooling?
        Destroy(gameObject);
    }
}
