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
    // PURCHASE ITEM
    // ------------------------------------------------------------

    public void PurchaseItem(Button purchaseButton)
    {
        // Disable the button so the player cannot purchase
        // the same item multiple times
        purchaseButton.interactable = false;

        // Change the button text to show that it was purchased
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
        // Get whatever the player typed into the search bar
        string searchText = searchInput.text.ToLower();

        // Check every product card
        foreach (GameObject product in productItems)
        {
            // Look for the product's name
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

            // Show products that match the search
            // Hide products that don't
            product.SetActive(productFound);
        }
    }
}