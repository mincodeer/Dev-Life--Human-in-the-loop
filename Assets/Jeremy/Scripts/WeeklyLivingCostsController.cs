using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeeklyLivingCostsController : MonoBehaviour
{
    // Amount the player must pay every second week
    public int livingCost = 300;

    // Reference to the game's time system
    public GameTimeManager gameTime;

    // The next day that rent is due
    private int nextRentDay = 14;

    // Popup shown when rent needs to be paid
    public GameObject rentPopup;

    // Popup shown when the player goes bankrupt
    public GameObject gameOverPopup;

    // UI text
    public TMP_Text rentText;

    // Pay button
    public Button payRentButton;

    // Stores the visual and text data for the Rent Game Over ending.
    public EndingData rentEnding;

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

        if (gameTime != null)
        {
            gameTime.OnDayPassed += CheckForRentDay;
        }
    }


    // ------------------------------------------------------------
    // CHECK FOR RENT DAY
    // ------------------------------------------------------------

    private void CheckForRentDay()
    {
        if (gameTime == null)
        {
            return;
        }

        if (gameTime.currentDay >= nextRentDay)
        {
            CheckRent();
        }
    }


    // ------------------------------------------------------------
    // CHECK RENT
    // ------------------------------------------------------------

    private void CheckRent()
    {
        if (ResourceManager.Instance == null)
        {
            Debug.LogWarning("ResourceManager not found.");
            return;
        }

        if (ResourceManager.Instance.Money < livingCost)
        {
            TriggerGameOver();
            return;
        }

        ShowRentPopup();
    }


    // ------------------------------------------------------------
    // RENT POPUP
    // ------------------------------------------------------------

    private void ShowRentPopup()
    {
        if (rentPopup != null)
        {
            rentPopup.SetActive(true);
        }

        if (rentText != null)
        {
            rentText.text = "$" + livingCost;
        }
    }


    // ------------------------------------------------------------
    // PAY RENT
    // ------------------------------------------------------------

    public void PayRent()
    {
        if (ResourceManager.Instance == null)
        {
            Debug.LogWarning("ResourceManager not found.");
            return;
        }

        if (ResourceManager.Instance.Money < livingCost)
        {
            TriggerGameOver();
            return;
        }

        ResourceManager.Instance.ChangeMoney(-livingCost);

        nextRentDay += 14;

        if (rentPopup != null)
        {
            rentPopup.SetActive(false);
        }
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

        // Find the Ending Controller so the full-screen
        // Rent ending can be displayed.
        EndingController endingController =
            FindFirstObjectByType<EndingController>(
                FindObjectsInactive.Include);

        if (endingController != null
            && rentEnding != null)
        {
            // Show the Rent ending using the EndingData
            // asset assigned in the Inspector.
            endingController.ShowEnding(
                rentEnding);
        }
        else
        {
            Debug.LogWarning(
                "EndingController or Rent EndingData is missing.");
        }
    }


    // ------------------------------------------------------------
    // CLEAN UP
    // ------------------------------------------------------------

    private void OnDestroy()
    {
        if (gameTime != null)
        {
            gameTime.OnDayPassed -= CheckForRentDay;
        }
    }
}