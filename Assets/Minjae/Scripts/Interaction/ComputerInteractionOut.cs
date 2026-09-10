using UnityEngine;

public class ComputerInteractionOut : MonoBehaviour
{
    [SerializeField] private ComputerInteractionIn computerView;

    public void InteractOut()
    {
        if (computerView != null)
            computerView.ReturnToRoom();
    }
}