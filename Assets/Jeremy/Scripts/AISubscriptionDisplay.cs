
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AISubscriptionDisplay : MonoBehaviour
{
    public Button subscribeButton;
    public TMP_Text buttonText;
    public TMP_Text statusText;

    private ResourceManager resources;

    private void OnEnable()
    {
        resources = ResourceManager.Instance;

        if (resources != null)
        {
            resources.OnResourcesChanged += UpdateDisplay;
        }

        UpdateDisplay();
    }

    private void Start()
    {
        UpdateDisplay();
    }

    private void OnDisable()
    {
        if (resources != null)
        {
            resources.OnResourcesChanged -= UpdateDisplay;
        }
    }

    private void UpdateDisplay()
    {
        if (ResourceManager.Instance == null)
        {
            return;
        }

        ResourceManager manager = ResourceManager.Instance;

        if (manager.AISubscriptionActive)
        {
            if (buttonText != null)
            {
                buttonText.text = "SUBSCRIBED";
            }

            if (statusText != null)
            {
                statusText.text =
                    "Next payment: Day " + manager.NextAISubscriptionDay;
            }

            if (subscribeButton != null)
            {
                subscribeButton.interactable = false;
            }
        }
        else if (manager.AISubscriptionPaused)
        {
            if (buttonText != null)
            {
                buttonText.text = "REACTIVATE";
            }

            if (statusText != null)
            {
                statusText.text = "Subscription paused";
            }

            if (subscribeButton != null)
            {
                subscribeButton.interactable = true;
            }
        }
        else
        {
            if (buttonText != null)
            {
                buttonText.text = "SUBSCRIBE";
            }

            if (statusText != null)
            {
                statusText.text = "25 credits every 7 days";
            }

            if (subscribeButton != null)
            {
                subscribeButton.interactable = true;
            }
        }
    }
}
