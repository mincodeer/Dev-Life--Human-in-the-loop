using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreUIController : MonoBehaviour
{
    public GameObject storePanel;

    public void CloseStore()
    {
        storePanel.SetActive(false);
    }

    public void OpenStore()
    {
        storePanel.SetActive(true);
    }

    public void PurchaseItem(Button purchaseButton)
    {
        purchaseButton.interactable = false;

        TMP_Text buttonText = purchaseButton.GetComponentInChildren<TMP_Text>();

        if (buttonText != null)
        {
            buttonText.text = "PURCHASED";
        }
    }
}