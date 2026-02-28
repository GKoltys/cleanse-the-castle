using UnityEngine;

public class PickUpWeapon : MonoBehaviour, IInteractable
{
    private WeaponChoiceController choiceController;
    private SpriteRenderer spriteRenderer;
    private Sprite baseSprite;
    [SerializeField] private Sprite interactableSprite;

    public WeaponData weapon;

    private void Awake()
    {
        choiceController = GetComponentInParent<WeaponChoiceController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseSprite = spriteRenderer.sprite;
    }

    public bool CanInteract()
    {
        return true;
    }

    public void Interact(GameObject interactor)
    {
        choiceController.UpdateWeapon(this);
    }

    public void ShowCanInteract(bool show)
    {
        if (show) spriteRenderer.sprite = interactableSprite;
        else spriteRenderer.sprite = baseSprite;
    }
}
