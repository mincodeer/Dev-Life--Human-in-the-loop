using System;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    // The current in-game day.
    public int currentDay = 1;

    // The current in-game hour and minute.
    public int currentHour = 8;
    public int currentMinute = 0;

    // How many in-game minutes pass every real-world second.
    // 10 makes the clock move quickly for testing.
    public float gameMinutesPerSecond = 10f;

    // Keeps track of real-world time.
    private float timer = 0f;

    // Tells other systems when one in-game hour has passed.
    // Market Trends uses this event for its hourly countdown.
    public event Action OnHourPassed;

    // Tells other systems how many in-game minutes were added.
    // Market Trends uses this event for its minute countdown.
    public event Action<int> OnMinutesPassed;

    // Tells other systems when one in-game day has passed.
    public event Action OnDayPassed;

    void Update()
    {
        // Add the time that has passed since the previous frame.
        timer += Time.deltaTime;

        // Advance the game clock every real-world second.
        if (timer >= 1f)
        {
            timer -= 1f;

            // Store the number so it can also be sent to Market Trends.
            int minutesPassed =
                Mathf.RoundToInt(gameMinutesPerSecond);

            // This still increases the clock in the same way.
            currentMinute += minutesPassed;

            // Tell Market Trends how many minutes passed.
            OnMinutesPassed?.Invoke(minutesPassed);

            // Move to the next hour when 60 minutes are reached.
            if (currentMinute >= 60)
            {
                currentMinute = 0;
                currentHour++;

                // Tell other systems that one hour passed.
                OnHourPassed?.Invoke();
            }

            // Move to the next day when 24 hours are reached.
            if (currentHour >= 24)
            {
                currentHour = 0;
                currentDay++;

                // Notify Market Trends and other systems
                // that one in-game day has passed.
                OnDayPassed?.Invoke();
            }
        }
    }
}