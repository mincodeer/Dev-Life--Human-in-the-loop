using UnityEngine;

public class ComputerInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private DevelopmentFlowManager developmentFlowManager;

    public void Interact()
    {
        Debug.Log("Computer Clicked");

        developmentFlowManager.OpenComputer();
    }
}
