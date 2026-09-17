using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreUIController : MonoBehaviour
{
    // Main store panel
    public GameObject storePanel;

    // Search bar
    public TMP_InputField searchInput;

    // Product cards in the store
    // Drag StoreItemBox1, StoreItemBox2, etc. into this list in Unity
    public GameObject[] productItems;


    // ------------------------------------------------------------
    // STORE OPEN / CLOSE
    // ------------------------------------------------------------

    public void CloseStore()
    {
        storePanel.SetActive(false);
    }

    public void OpenStore()
    {
        storePanel.SetActive(true);
    }


    // ------------------------------------------------------------
    // PURCHASE ITEMS
    // ------------------------------------------------------------

    public void PurchasePCUpgrade()
    {
        PurchaseItem(800);
    }

    public void PurchaseLightUpgrade()
    {
        PurchaseItem(800);
    }

    public void PurchaseCleanUp()
    {
        PurchaseItem(300);
    }

    public void PurchaseWindowUpgrade()
    {
        PurchaseItem(300);
    }

    public void PurchaseDecor()
    {
        PurchaseItem(300);
    }

    private void PurchaseItem(int price)
    {
        if (ResourceManager.Instance == null)
        {
            Debug.LogWarning("ResourceManager not found.");
            return;
        }

        if (ResourceManager.Instance.Money < price)
        {
            Debug.Log("Not enough money.");
            return;
        }

        Button purchaseButton =
            UnityEngine.EventSystems.EventSystem.current
            .currentSelectedGameObject
            .GetComponent<Button>();

        ResourceManager.Instance.ChangeMoney(-price);

        purchaseButton.interactable = false;

        TMP_Text buttonText =
            purchaseButton.GetComponentInChildren<TMP_Text>();

        if (buttonText != null)
        {
            buttonText.text = "PURCHASED";
        }
    }


    // ------------------------------------------------------------
    // SEARCH
    // ------------------------------------------------------------

    public void SearchProducts()
    {
        string searchText = searchInput.text.ToLower();

        foreach (GameObject product in productItems)
        {
            TMP_Text[] textComponents =
                product.GetComponentsInChildren<TMP_Text>();

            bool productFound = false;

            foreach (TMP_Text text in textComponents)
            {
                if (text.text.ToLower().Contains(searchText))
                {
                    productFound = true;
                    break;
                }
            }

            product.SetActive(productFound);
        }
    }
}