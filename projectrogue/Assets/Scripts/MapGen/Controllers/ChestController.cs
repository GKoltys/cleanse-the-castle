using UnityEngine;

public class ChestController : MonoBehaviour, IMapGenInit, IInteractable
{
    public bool IsOpened;
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
        OpenChest();
    }

    public void ShowCanInteract(bool show)
    {
        interactionIcon.SetActive(show);
    }

    private void OpenChest()
    {
        SetIsOpened(true);
        animator.SetTrigger("OpenChest");
        // add functionality later like dropping items
    }

    public void SetIsOpened(bool opened)
    {
        IsOpened = opened;
    }
}
