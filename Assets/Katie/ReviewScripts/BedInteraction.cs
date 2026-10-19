using UnityEngine;

public class BedInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private SleepController sleepController;

    public void Interact()
    {
        if (sleepController != null)
        {
            sleepController.TrySleep();
        }
        else
        {
            Debug.LogWarning(
                "BedInteraction: assign the SleepController.", this);
        }
    }
}