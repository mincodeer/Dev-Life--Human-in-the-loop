using UnityEngine;
using TMPro;

/// <summary>
/// Updates the Money and Fandom UI using the player's
/// permanent resources from ResourceManager.
/// </summary>
public class ResourceUI : MonoBehaviour
{
    [Header("Resource Text")]

    // Displays the player's current money.
    [SerializeField]
    private TMP_Text moneyText;

    // Displays the player's current fandom.
    [SerializeField]
    private TMP_Text fandomText;

    /// <summary>
    /// Starts listening for resource changes when
    /// the Resource UI becomes active.
    /// </summary>
    private void OnEnable()
    {
        SubscribeToResourceManager();

        // Refresh the UI immediately when it becomes active.
        UpdateResourceUI();
    }

    /// <summary>
    /// Makes sure the ResourceUI is subscribed after
    /// the ResourceManager has finished initializing.
    /// </summary>
    private void Start()
    {
        // ResourceManager may not have existed when OnEnable()
        // was called, so try subscribing again here.
        SubscribeToResourceManager();

        // Make sure the UI displays the latest values.
        UpdateResourceUI();
    }

    /// <summary>
    /// Subscribes the UI to the ResourceManager event.
    /// </summary>
    private void SubscribeToResourceManager()
    {
        if (ResourceManager.Instance != null)
        {
            // Remove first to prevent duplicate subscriptions.
            ResourceManager.Instance.OnResourcesChanged
                -= UpdateResourceUI;

            // Subscribe to resource changes.
            ResourceManager.Instance.OnResourcesChanged
                += UpdateResourceUI;
        }
    }

    /// <summary>
    /// Stops listening for resource changes when
    /// the Resource UI is disabled.
    /// </summary>
    private void OnDisable()
    {
        UnsubscribeFromResourceManager();
    }

    /// <summary>
    /// Stops listening for resource changes when
    /// the UI is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        UnsubscribeFromResourceManager();
    }

    /// <summary>
    /// Removes the UI from the ResourceManager event.
    /// </summary>
    private void UnsubscribeFromResourceManager()
    {
        if (ResourceManager.Instance != null)
        {
            ResourceManager.Instance.OnResourcesChanged
                -= UpdateResourceUI;
        }
    }

    /// <summary>
    /// Updates the displayed Money and Fandom values.
    /// </summary>
    public void UpdateResourceUI()
    {
        // Make sure the ResourceManager exists before
        // accessing the player's resources.
        if (ResourceManager.Instance == null)
        {
            Debug.LogWarning(
                "ResourceManager not found.");

            return;
        }

        // Display the current Money value.
        if (moneyText != null)
        {
            moneyText.text =
                "Money: $"
                + ResourceManager.Instance.Money;
        }

        // Display the current Fandom value.
        if (fandomText != null)
        {
            fandomText.text =
                "Fandom: "
                + ResourceManager.Instance.Fandom;
        }
    }
}