using UnityEngine;

public class KeyController : MonoBehaviour, IMapGenInit, IInteractable
{
    public bool isCollected;
    public GameObject interactionIcon;
    private Animator animator;
    private MapGenerator dungeon;

    public GameObject lastInteractor;

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

    public void Interact(GameObject interactor)
    {
        if (!CanInteract()) return;
        if (dungeon == null) return;
        // add need key functionality
        lastInteractor = interactor;
        CollectKey();
        if (interactionIcon != null)
            interactionIcon.SetActive(false);

    }

    public void ShowCanInteract(bool show)
    {
        if (interactionIcon != null)
            interactionIcon.SetActive(show && CanInteract());
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
