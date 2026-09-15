using UnityEngine;
using System.Collections;

public class ComputerInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private ComputerInteractionIn interactionIn;

    [SerializeField]
    private DevelopmentTutorialPopup tutorialPopup;

    [SerializeField]
    private GameObject computerVisual;


    public void Interact()
    {
        if (interactionIn != null)
            interactionIn.InteractIn();
    }
}