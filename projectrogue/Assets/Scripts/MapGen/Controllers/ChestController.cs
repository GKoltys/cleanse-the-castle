using UnityEngine;

public class ChestController : MonoBehaviour, IMapGenInit, IInteractable
{
    public bool IsOpened;
    public GameObject interactionIcon;
    private Animator animator;
    private MapGenerator dungeon;

    public GameObject lastInteractor;
    [SerializeField] private EnemyDropTable chestLootTable; // items that can come from chest
    private GameObject dropPrefab;

    public void Init(MapGenerator controller)
    {
        dungeon = controller;
        animator = GetComponent<Animator>();
    }
    
    // set the chest to closed on spawn
    private void Awake()
    {
        interactionIcon.SetActive(false);
        SetIsOpened(false);
    }

    public bool CanInteract()
    {
        return !IsOpened;
    }

    public void Interact(GameObject interactor)
    {
        if (!CanInteract()) return;
        if (dungeon == null) return;
        lastInteractor = interactor;

        if (chestLootTable != null)
        {
            dropPrefab = chestLootTable.GetRandomLootItem();
            Debug.Log($"dropPrefab = {dropPrefab}");
        }

        OpenChest();

        if (interactionIcon != null)
            interactionIcon.SetActive(false);


    }

    public void ShowCanInteract(bool show)
    {
        if (interactionIcon != null)
            interactionIcon.SetActive(show && CanInteract());
    }

    private void OpenChest()
    {
        SetIsOpened(true);
        animator.SetTrigger("OpenChest");
    }

    public void SetIsOpened(bool opened)
    {
        IsOpened = opened;
    }

    public void SetDrop(GameObject prefab)
    {
        dropPrefab = prefab;
    }

    // run as an animation event, so after chest opens it drops the item
    public void DropItem()
    {
        if (dropPrefab != null)
        {
            Vector3 dropPos = transform.position + Vector3.up * 0.2f;

            // spawn prefab as part of dungeon map to handle clearing on next build floor
            Transform parent = dungeon != null ? dungeon.EntitiesRoot : null;
            var go = Instantiate(dropPrefab, dropPos, Quaternion.identity, parent);

            // run init so interaction works
            var initializables = go.GetComponentsInChildren<IMapGenInit>();
            foreach (var init in initializables)
                init.Init(dungeon);
        }
        // despawn after opening
        Destroy(gameObject);
    }
}
