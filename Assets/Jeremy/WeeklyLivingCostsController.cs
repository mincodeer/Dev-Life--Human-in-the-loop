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

    // Popup shown when the player goes bankrupt
    public GameObject gameOverPopup;

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
        if (rentPopup != null)
        {
            rentPopup.SetActive(false);
        }

        if (gameOverPopup != null)
        {
            gameOverPopup.SetActive(false);
        }

        UpdateUI();
    }


    // ------------------------------------------------------------
    // ADVANCE WEEK
    // ------------------------------------------------------------

    public void AdvanceWeek()
    {
        // Do not allow time to continue while rent is unpaid
        if (rentPopup != null && rentPopup.activeSelf)
        {
            return;
        }

        // Do not continue after Game Over
        if (gameOverPopup != null && gameOverPopup.activeSelf)
        {
            return;
        }

        currentWeek++;

        UpdateUI();

        // Rent is due every second week
        if (currentWeek % 2 == 0)
        {
            CheckRent();
        }
    }


    // ------------------------------------------------------------
    // CHECK RENT
    // ------------------------------------------------------------

    private void CheckRent()
    {
        // Player cannot afford the rent
        if (money < livingCost)
        {
            TriggerGameOver();
            return;
        }

        // Player can afford the rent
        ShowRentPopup();
    }


    // ------------------------------------------------------------
    // RENT POPUP
    // ------------------------------------------------------------

    private void ShowRentPopup()
    {
        rentPopup.SetActive(true);

        // "Amount:" is already part of the popup PNG
        rentText.text = "$" + livingCost;
    }


    // ------------------------------------------------------------
    // PAY RENT
    // ------------------------------------------------------------

    public void PayRent()
    {
        // Make sure the player can still afford the rent
        if (money < livingCost)
        {
            TriggerGameOver();
            return;
        }

        // Remove the rent cost from the player's money
        money -= livingCost;

        // Close the popup after payment
        rentPopup.SetActive(false);

        UpdateUI();
    }


    // ------------------------------------------------------------
    // GAME OVER
    // ------------------------------------------------------------

    private void TriggerGameOver()
    {
        if (rentPopup != null)
        {
            rentPopup.SetActive(false);
        }

        if (gameOverPopup != null)
        {
            gameOverPopup.SetActive(true);
        }
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