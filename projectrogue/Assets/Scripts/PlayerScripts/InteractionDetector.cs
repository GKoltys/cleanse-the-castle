using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
	private IInteractable interactableInRange = null;

    public void OnInteract(UnityEngine.InputSystem.InputValue value)
    {

        if (!value.isPressed) return;
        if (interactableInRange == null) return;

        // call the interact method on the interactlbe object in range
        interactableInRange?.Interact(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // look for IInteractable on the object 
        var interactable = other.GetComponentInParent<IInteractable>();

        if (interactable == null) return;

        interactableInRange = interactable;
        // set the interaction icon to appear on interactable object
        interactable.ShowCanInteract(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // look for IInteractable on the object 
        var interactable = other.GetComponentInParent<IInteractable>();

        if (interactable == null) return;

        if (interactableInRange != null && interactable == interactableInRange)
        {
            interactableInRange = null;
            interactable.ShowCanInteract(false);
        }
    }
}
