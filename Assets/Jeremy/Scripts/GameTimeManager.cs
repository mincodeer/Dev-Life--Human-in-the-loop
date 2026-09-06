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

    void Update()
    {
        // Add the time that has passed since the previous frame.
        timer += Time.deltaTime;

        // Advance the game clock every real-world second.
        if (timer >= 1f)
        {
            timer -= 1f;

            currentMinute += Mathf.RoundToInt(gameMinutesPerSecond);

            // Move to the next hour when 60 minutes are reached.
            if (currentMinute >= 60)
            {
                currentMinute = 0;
                currentHour++;
            }

            // Move to the next day when 24 hours are reached.
            if (currentHour >= 24)
            {
                currentHour = 0;
                currentDay++;
            }
        }
    }
}