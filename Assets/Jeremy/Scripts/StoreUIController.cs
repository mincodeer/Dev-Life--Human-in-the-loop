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

    // Temporary money system for testing
    public int money = 3000;

    // Displays the player's current money
    public TMP_Text moneyText;


    // ------------------------------------------------------------
    // START
    // ------------------------------------------------------------

    private void Start()
    {
        UpdateMoneyText();
    }


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
        // Get the button that was clicked
        Button purchaseButton =
            UnityEngine.EventSystems.EventSystem.current
            .currentSelectedGameObject
            .GetComponent<Button>();

        // Check if the player has enough money
        if (money < price)
        {
            Debug.Log("Not enough money.");
            return;
        }

        // Remove the item's price from the player's money
        money -= price;

        // Disable the button so the item cannot be purchased again
        purchaseButton.interactable = false;

        // Change the button text to PURCHASED
        TMP_Text buttonText =
            purchaseButton.GetComponentInChildren<TMP_Text>();

        if (buttonText != null)
        {
            buttonText.text = "PURCHASED";
        }

        UpdateMoneyText();
    }


    // ------------------------------------------------------------
    // MONEY DISPLAY
    // ------------------------------------------------------------

    private void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = "$" + money;
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