using UnityEngine;
using UnityEngine.InputSystem;

public class ComputerInteractionOut : MonoBehaviour
{
    [SerializeField]
    private ComputerInteractionIn computerView;


    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            InteractOut();
        }
    }


    public void InteractOut()
    {
        if (computerView != null)
        {
            computerView.ExitOneStep();
        }
    }
}