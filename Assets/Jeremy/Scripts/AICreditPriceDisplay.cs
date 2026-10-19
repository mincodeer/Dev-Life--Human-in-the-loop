
using UnityEngine;
using TMPro;

public class AICreditPriceDisplay : MonoBehaviour
{
    public TMP_Text priceText;

    private void OnEnable()
    {
        if (ResourceManager.Instance != null)
        {
            ResourceManager.Instance.OnResourcesChanged += UpdatePrice;
        }

        UpdatePrice();
    }

    private void Start()
    {
        UpdatePrice();
    }

    private void OnDisable()
    {
        if (ResourceManager.Instance != null)
        {
            ResourceManager.Instance.OnResourcesChanged -= UpdatePrice;
        }
    }

    private void UpdatePrice()
    {
        if (ResourceManager.Instance == null || priceText == null)
        {
            return;
        }

        priceText.text = "$" + ResourceManager.Instance.AICreditPackCost;
    }
}
