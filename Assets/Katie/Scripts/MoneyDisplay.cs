using System.Globalization;
using TMPro;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyValueText;
    [SerializeField] private string currencySymbol = "$";

    [Header("Money Colours")]
    [SerializeField]
    private Color normalMoneyColor =
        new Color32(1, 251, 0, 255);       

    [SerializeField]
    private Color negativeMoneyColor =
        new Color32(255, 0, 2, 255);       

    private MoneyManager moneyManager;

    private void Start()
    {
        moneyManager = MoneyManager.Instance;

        if (moneyManager == null)
        {
            Debug.LogError(
                "MoneyDisplay could not find MoneyManager."
            );

            return;
        }

        moneyManager.MoneyChanged += UpdateDisplay;
        UpdateDisplay(moneyManager.CurrentMoney);
    }

    private void OnDestroy()
    {
        if (moneyManager != null)
        {
            moneyManager.MoneyChanged -= UpdateDisplay;
        }
    }

    private void UpdateDisplay(int amount)
    {
        if (moneyValueText == null)
        {
            return;
        }

        string formattedAmount =
            Mathf.Abs(amount).ToString(
                "N0",
                CultureInfo.InvariantCulture
            );

        if (amount < 0)
        {
            moneyValueText.text =
                "-" + currencySymbol + formattedAmount;

            moneyValueText.color = negativeMoneyColor;
        }
        else
        {
            moneyValueText.text =
                currencySymbol + formattedAmount;

            moneyValueText.color = normalMoneyColor;
        }
    }
}
