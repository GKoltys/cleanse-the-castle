using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
	private IInteractable interactableInRange = null;
	public GameObject interactionIcon;


	void Start()
	{
		// set detection icon to false
		interactionIcon.SetActive(false);
	}

    public void OnInteract(UnityEngine.InputSystem.InputValue value)
    {

        if (!value.isPressed) return;
        if (interactableInRange == null) return;

        interactionIcon.SetActive(false);

        // call the interact method on the interactlbe object in range
        interactableInRange?.Interact();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // look for IInteractable on the object 
        var interactable = other.GetComponentInParent<IInteractable>();

        if (interactable == null) return;

        interactableInRange = interactable;
        interactionIcon.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // look for IInteractable on the object 
        var interactable = other.GetComponentInParent<IInteractable>();

        if (interactable == null) return;

        if (interactableInRange != null && interactable == interactableInRange)
        {
            interactableInRange = null;

            if (interactionIcon != null)
                interactionIcon.SetActive(false);
        }
    }
}
