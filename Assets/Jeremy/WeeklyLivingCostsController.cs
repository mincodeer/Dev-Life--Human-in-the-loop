using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeeklyLivingCostsController : MonoBehaviour
{
    // Temporary money value used for testing
    public int money = 1000;

    // Amount the player must pay every second week
    public int livingCost = 300;

    // Keeps track of the current week
    public int currentWeek = 1;

    // Popup shown when rent needs to be paid
    public GameObject rentPopup;

    // UI text
    public TMP_Text moneyText;
    public TMP_Text weekText;
    public TMP_Text rentText;

    // Pay button
    public Button payRentButton;


    // ------------------------------------------------------------
    // START
    // ------------------------------------------------------------

    private void Start()
    {
        rentPopup.SetActive(false);
        UpdateUI();
    }


    // ------------------------------------------------------------
    // ADVANCE WEEK
    // ------------------------------------------------------------

    public void AdvanceWeek()
    {
        // Do not allow another week to pass while rent is unpaid
        if (rentPopup.activeSelf)
        {
            return;
        }

        currentWeek++;

        UpdateUI();

        // Rent is due every second week
        if (currentWeek % 2 == 0)
        {
            ShowRentPopup();
        }
    }


    // ------------------------------------------------------------
    // RENT POPUP
    // ------------------------------------------------------------

    private void ShowRentPopup()
    {
        rentPopup.SetActive(true);

        // Only display the amount because "Amount:" is on the PNG
        rentText.text = "$" + livingCost;
    }


    // ------------------------------------------------------------
    // PAY RENT
    // ------------------------------------------------------------

    public void PayRent()
    {
        // Rent is paid even if this makes the player's money negative
        money -= livingCost;

        // Close the popup after rent has been paid
        rentPopup.SetActive(false);

        UpdateUI();
    }


    // ------------------------------------------------------------
    // UPDATE UI
    // ------------------------------------------------------------

    private void UpdateUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "Money: $" + money;
        }

        if (weekText != null)
        {
            weekText.text = "Week: " + currentWeek;
        }
    }
}