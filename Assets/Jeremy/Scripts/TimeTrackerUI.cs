using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimeTrackerUI : MonoBehaviour
{
    // Text that displays the current day.
    public TMP_Text dayText;

    // Text that displays the current time.
    public TMP_Text timeText;

    // Text that displays the time of day.
    public TMP_Text timeOfDayText;

    // Image that displays the sun or moon icon.
    public Image timeOfDayIcon;

    // Sprite used for daytime.
    public Sprite sunIcon;

    // Sprite used for nighttime.
    public Sprite moonIcon;

    // Reference to the GameTimeManager.
    public GameTimeManager gameTime;

    void Update()
    {
        // Make sure the time manager is connected.
        if (gameTime == null)
            return;

        // Update the day displayed on the screen.
        dayText.text = "DAY " + gameTime.currentDay;

        // Get the current hour.
        int hour = gameTime.currentHour;

        // Decide whether the time is AM or PM.
        string period = hour >= 12 ? "PM" : "AM";

        // Convert the 24-hour clock to a 12-hour clock.
        int displayHour = hour % 12;

        // Display midnight as 12 instead of 0.
        if (displayHour == 0)
            displayHour = 12;

        // Update the time displayed on the screen.
        timeText.text = displayHour.ToString("00") + ":" +
                        gameTime.currentMinute.ToString("00") +
                        " " + period;

        // Update the time-of-day label and icon.
        if (hour >= 6 && hour < 12)
        {
            // 6 AM - 11:59 AM
            timeOfDayText.text = "MORNING";
            timeOfDayIcon.sprite = sunIcon;
        }
        else if (hour >= 12 && hour < 18)
        {
            // 12 PM - 5:59 PM
            timeOfDayText.text = "AFTERNOON";
            timeOfDayIcon.sprite = sunIcon;
        }
        else if (hour >= 18 && hour < 22)
        {
            // 6 PM - 9:59 PM
            timeOfDayText.text = "EVENING";
            timeOfDayIcon.sprite = moonIcon;
        }
        else
        {
            // 10 PM - 5:59 AM
            timeOfDayText.text = "NIGHT";
            timeOfDayIcon.sprite = moonIcon;
        }
    }
}