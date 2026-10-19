
using System;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    // Allows other scripts to access the same clock.
    public static GameTimeManager Instance { get; private set; }

    // The current in-game day.
    public int currentDay = 1;

    // The current in-game hour and minute.
    public int currentHour = 8;
    public int currentMinute = 0;

    // How many in-game minutes pass every real-world second.
    public float gameMinutesPerSecond = 10f;

    // Keeps track of real-world time.
    private float timer = 0f;

    // Tells other systems when one in-game hour has passed.
    public event Action OnHourPassed;

    // Tells other systems how many in-game minutes were added.
    public event Action<int> OnMinutesPassed;

    // Tells other systems when one in-game day has passed.
    public event Action OnDayPassed;

    private void Awake()
    {
        // Prevent multiple game clocks from running.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Keep the clock running when changing scenes.
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // Add the time passed since the previous frame.
        timer += Time.deltaTime;

        // Advance the game clock every real-world second.
        if (timer >= 1f)
        {
            timer -= 1f;

            int minutesPassed =
                Mathf.RoundToInt(gameMinutesPerSecond);

            currentMinute += minutesPassed;

            // Notify systems such as Market Trends.
            OnMinutesPassed?.Invoke(minutesPassed);

            // Advance the hour.
            if (currentMinute >= 60)
            {
                currentMinute = 0;
                currentHour++;

                OnHourPassed?.Invoke();
            }

            // Advance the day.
            if (currentHour >= 24)
            {
                currentHour = 0;
                currentDay++;

                OnDayPassed?.Invoke();
            }
        }
    }

    // Advances time during sleep.
    // Notifies other systems as minutes, hours and days pass.
    public void AdvanceMinutes(int minutes)
    {
        if (minutes <= 0)
            return;

        while (minutes > 0)
        {
            // Advance only as far as the next hour.
            int step = Mathf.Min(minutes, 60 - currentMinute);

            currentMinute += step;
            minutes -= step;

            bool hourPassed = currentMinute >= 60;
            bool dayPassed = false;

            if (hourPassed)
            {
                currentMinute = 0;
                currentHour++;

                // Move into the next day when crossing midnight.
                if (currentHour >= 24)
                {
                    currentHour = 0;
                    currentDay++;
                    dayPassed = true;
                }
            }

            OnMinutesPassed?.Invoke(step);

            if (hourPassed)
                OnHourPassed?.Invoke();

            if (dayPassed)
                OnDayPassed?.Invoke();
        }
    }
    // Restores the clock without replaying hourly or daily events.
    public void RestoreSavedTime(int day, int hour, int minute)
    {
        currentDay = Mathf.Max(1, day);
        currentHour = Mathf.Clamp(hour, 0, 23);
        currentMinute = Mathf.Clamp(minute, 0, 59);

        // Discard the unfinished real-world second from before loading.
        timer = 0f;
    }
}
