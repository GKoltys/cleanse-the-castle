using UnityEngine;
using System.Collections;

public class LockedChestController : MonoBehaviour, IMapGenInit, IInteractable
{
    public bool IsOpened = false;

    public GameObject interactionIcon; // normal interact icon
    public GameObject noKeyIcon;       // shown when player has no key

    private Animator animator;
    private MapGenerator dungeon;
    private PlayerBase player;

    public GameObject lastInteractor;

    [SerializeField] private EnemyDropTable chestLootTable; // items that can come from chest
    private GameObject dropPrefab;

    public void Init(MapGenerator controller)
    {
        dungeon = controller;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBase>();
    }

    // set the chest to closed on spawn
    private void Awake()
    {
        interactionIcon.SetActive(false);
        noKeyIcon.SetActive(false);
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
        if (player.GetKeyCount <= 0) return; // Maybe play a chest staying locked animation?
        lastInteractor = interactor;

        if (chestLootTable != null)
        {
            dropPrefab = chestLootTable.GetRandomLootItem();
            Debug.Log($"dropPrefab = {dropPrefab}");
        }

        player.KeyUsed();
        OpenChest();
        if (interactionIcon != null)
            interactionIcon.SetActive(false);

    }

    // sets which interaction icon to show depending on player key count
    public void ShowCanInteract(bool show)
    {
        HideAllIcons();

        if (!show || !CanInteract() || player == null)
            return;

        if (player.GetKeyCount > 0)
        {
            if (interactionIcon != null)
                interactionIcon.SetActive(true);
        }
        else
        {
            if (noKeyIcon != null)
                noKeyIcon.SetActive(true);
        }
    }

    private void HideAllIcons()
    {
        if (interactionIcon != null)
            interactionIcon.SetActive(false);

        if (noKeyIcon != null)
            noKeyIcon.SetActive(false);
    }

    private void OpenChest()
    {
        SetIsOpened(true);
        SoundEffectManager.Play(SoundGroupName.LOCKEDCHEST);
        animator.SetTrigger("OpenLockedChest");
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
