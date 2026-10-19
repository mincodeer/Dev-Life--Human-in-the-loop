
using UnityEngine;
using TMPro;

public class AICreditDisplay : MonoBehaviour
{
    public TMP_Text aiCreditText;

    private ResourceManager resourceManager;

    private void Start()
    {
        ConnectToResourceManager();
        UpdateAICreditDisplay();
    }

    private void OnEnable()
    {
        ConnectToResourceManager();
        UpdateAICreditDisplay();
    }

    private void OnDisable()
    {
        if (resourceManager != null)
        {
            resourceManager.OnResourcesChanged -= UpdateAICreditDisplay;
        }
    }

    private void ConnectToResourceManager()
    {
        ResourceManager currentManager = ResourceManager.Instance;

        if (currentManager == null)
        {
            return;
        }

        if (resourceManager != null)
        {
            resourceManager.OnResourcesChanged -= UpdateAICreditDisplay;
        }

        resourceManager = currentManager;

        resourceManager.OnResourcesChanged += UpdateAICreditDisplay;
    }

    private void UpdateAICreditDisplay()
    {
        if (ResourceManager.Instance == null || aiCreditText == null)
        {
            return;
        }

        aiCreditText.text =
            "AI Credits: " + ResourceManager.Instance.AICredits;
    }
}
