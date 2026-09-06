using UnityEngine;
using System.Collections;

public class ComputerInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private DevelopmentFlowManager developmentFlowManager;

    [SerializeField]
    private DevelopmentTutorialPopup tutorialPopup;

    [SerializeField]
    private GameObject computerVisual;


    public void Interact()
    {
        Debug.Log("Computer Clicked");

        // Open the development computer UI.
        developmentFlowManager.OpenComputer();

        // Hide the computer visual.
        if (computerVisual != null)
        {
            computerVisual.SetActive(false);
        }

        // Wait one frame before showing the tutorial.
        // This allows the Project Setup UI and dropdowns
        // to finish opening first.
        if (tutorialPopup != null)
        {
            StartCoroutine(ShowThemeTutorial());
        }
    }


    private IEnumerator ShowThemeTutorial()
    {
        yield return null;

        tutorialPopup.ShowThemeExplanation();
    }
}