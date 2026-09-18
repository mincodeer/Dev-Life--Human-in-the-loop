using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StoreUIController : MonoBehaviour
{
    public GameObject storePanel;

    public TMP_InputField searchInput;

    public GameObject[] productItems;

    public Button pcUpgradeButton;
    public Button lightUpgradeButton;
    public Button cleanUpButton;
    public Button windowUpgradeButton;
    public Button decorButton;

    private void Start()
    {
        UpdatePurchaseButtons();
    }

    public void CloseStore()
    {
        storePanel.SetActive(false);
    }

    public void OpenStore()
    {
        storePanel.SetActive(true);
        UpdatePurchaseButtons();
    }

    public void PurchasePCUpgrade()
    {
        if (PurchaseItem(800, pcUpgradeButton))
        {
            if (RoomUpgradeManager.Instance != null)
            {
                RoomUpgradeManager.Instance.PurchasePCUpgrade();
            }
        }
    }

    public void PurchaseLightUpgrade()
    {
        if (PurchaseItem(800, lightUpgradeButton))
        {
            if (RoomUpgradeManager.Instance != null)
            {
                RoomUpgradeManager.Instance.PurchaseLightUpgrade();
            }
        }
    }

    public void PurchaseCleanUp()
    {
        if (PurchaseItem(300, cleanUpButton))
        {
            if (RoomUpgradeManager.Instance != null)
            {
                RoomUpgradeManager.Instance.PurchaseCleanUp();
            }
        }
    }

    public void PurchaseWindowUpgrade()
    {
        if (PurchaseItem(300, windowUpgradeButton))
        {
            if (RoomUpgradeManager.Instance != null)
            {
                RoomUpgradeManager.Instance.PurchaseWindowUpgrade();
            }
        }
    }

    public void PurchaseDecor()
    {
        if (PurchaseItem(300, decorButton))
        {
            if (RoomUpgradeManager.Instance != null)
            {
                RoomUpgradeManager.Instance.PurchaseDecorUpgrade();
            }
        }
    }

    private bool PurchaseItem(int price, Button purchaseButton)
    {
        if (ResourceManager.Instance == null)
        {
            Debug.LogWarning("ResourceManager not found.");
            return false;
        }

        if (RoomUpgradeManager.Instance == null)
        {
            Debug.LogWarning("RoomUpgradeManager not found.");
            return false;
        }

        if (purchaseButton == null)
        {
            Debug.LogWarning("Purchase button not connected.");
            return false;
        }

        if (!purchaseButton.interactable)
        {
            return false;
        }

        if (ResourceManager.Instance.Money < price)
        {
            Debug.Log("Not enough money.");
            return false;
        }

        ResourceManager.Instance.ChangeMoney(-price);

        SetButtonPurchased(purchaseButton);

        return true;
    }

    private void UpdatePurchaseButtons()
    {
        if (RoomUpgradeManager.Instance == null)
        {
            return;
        }

        if (RoomUpgradeManager.Instance.IsPCUpgradePurchased())
        {
            SetButtonPurchased(pcUpgradeButton);
        }

        if (RoomUpgradeManager.Instance.IsLightUpgradePurchased())
        {
            SetButtonPurchased(lightUpgradeButton);
        }

        if (RoomUpgradeManager.Instance.IsCleanUpPurchased())
        {
            SetButtonPurchased(cleanUpButton);
        }

        if (RoomUpgradeManager.Instance.IsWindowUpgradePurchased())
        {
            SetButtonPurchased(windowUpgradeButton);
        }

        if (RoomUpgradeManager.Instance.IsDecorUpgradePurchased())
        {
            SetButtonPurchased(decorButton);
        }
    }

    private void SetButtonPurchased(Button purchaseButton)
    {
        if (purchaseButton == null)
        {
            return;
        }

        purchaseButton.interactable = false;

        TMP_Text buttonText =
            purchaseButton.GetComponentInChildren<TMP_Text>();

        if (buttonText != null)
        {
            buttonText.text = "PURCHASED";
        }
    }

    public void ExitStore()
    {
        SceneManager.LoadScene("Minjae's Scene");
    }

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