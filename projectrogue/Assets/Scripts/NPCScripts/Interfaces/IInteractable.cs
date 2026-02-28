using UnityEngine;

public interface IInteractable
{
	void Interact(GameObject interactor);
	bool CanInteract();
	void ShowCanInteract(bool show); // Show that this GameObject can be interacted with
}
