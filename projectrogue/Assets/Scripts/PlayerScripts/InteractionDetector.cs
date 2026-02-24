using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
	private IInteractable interactableInRange = null;
	void Start()
	{
		// set detection icon to false
	}

	public void OnInteract(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			interactableInRange = interactable;
			interactableInRange?.Interact();
		}
	} 

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
		{
			interactableInRange = interactable;
            // set detection icon to true
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == interactableInRange)
        {
			interactableInRange = null;
            // set detection icon to false
        }
    }
}
