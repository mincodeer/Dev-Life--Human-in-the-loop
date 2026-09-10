using UnityEngine;

public class ComputerInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private ComputerInteractionIn interactionIn;

    public void Interact()
    {
        if (interactionIn != null)
            interactionIn.InteractIn();
    }
}