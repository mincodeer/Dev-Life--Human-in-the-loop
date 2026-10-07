using System.Collections;
using UnityEngine;

public class SleepController : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] private GameTimeManager gameTime;
    [SerializeField] private ProjectResourceController projectResources;
    [SerializeField] private CanvasGroup fadePanel;

    [Header("Sleep Effects")]
    // Number of in-game hours that pass during sleep.
    [SerializeField, Min(1)] private int sleepHours = 7;

    // Amount of workload removed by sleeping.
    [SerializeField, Min(0f)] private float workloadRecovery = 20f;

    [Header("Fade")]
    // Time taken to dim or brighten the room.
    [SerializeField, Min(0.01f)] private float fadeDuration = 1f;

    // Real seconds used to advance the clock during sleep.
    [SerializeField, Min(0.1f)] private float blackScreenDuration = 5f;

    // Keeps the room visible beneath the black overlay.
    [SerializeField, Range(0f, 1f)] private float sleepDarkness = 0.65f;

    public bool IsSleeping { get; private set; }

    private Coroutine sleepRoutine;
    private bool clockWasEnabled;

    private void Awake()
    {
        // Keep the sleep overlay hidden at startup.
        if (fadePanel != null)
        {
            fadePanel.alpha = 0f;
            fadePanel.blocksRaycasts = false;
            fadePanel.interactable = false;
            fadePanel.gameObject.SetActive(false);
        }
    }

    public void TrySleep()
    {
        // Prevent starting another sleep while already sleeping.
        if (IsSleeping || !isActiveAndEnabled)
            return;

        // Do not sleep while menus are open or the game is paused.
        if (EscapeInputRouter.WorldInputBlocked || Time.timeScale <= 0f)
            return;

        // Check the required Inspector connections.
        if (gameTime == null ||
            projectResources == null ||
            projectResources.CurrentProjectConditions == null ||
            fadePanel == null)
        {
            Debug.LogError(
                "SleepController: connect the time manager, " +
                "project resources, and fade panel.", this);

            return;
        }

        if (!gameTime.enabled)
            return;

        // The controller must remain active when the overlay is hidden.
        if (transform.IsChildOf(fadePanel.transform))
        {
            Debug.LogError(
                "Put SleepController outside the fade panel.", this);

            return;
        }

        IsSleeping = true;
        sleepRoutine = StartCoroutine(Sleep());
    }

    private IEnumerator Sleep()
    {
        // Pause the normal clock while sleep advances it manually.
        clockWasEnabled = gameTime.enabled;
        gameTime.enabled = false;

        // Show the overlay and block clicks during sleep.
        fadePanel.gameObject.SetActive(true);
        fadePanel.blocksRaycasts = true;

        yield return FadeTo(sleepDarkness);

        var conditions = projectResources.CurrentProjectConditions;
        float workloadBefore = conditions.workload;

        // Reduce workload without letting it fall below zero.
        conditions.ChangeWorkload(-Mathf.Max(0f, workloadRecovery));

        // Show the clock moving quickly through the sleep hours.
        yield return AdvanceSleepTime();

        Debug.Log(
            "Sleep completed. Workload: " +
            workloadBefore + " -> " + conditions.workload +
            ". Day " + gameTime.currentDay +
            ", time " + gameTime.currentHour.ToString("00") +
            ":" + gameTime.currentMinute.ToString("00"));

        // Brighten the room when the player wakes up.
        yield return FadeTo(0f);

        FinishSleep();
        sleepRoutine = null;
    }

    private IEnumerator AdvanceSleepTime()
    {
        int totalMinutes = Mathf.Max(1, sleepHours) * 60;
        int minutesAdded = 0;

        float duration = Mathf.Max(0.1f, blackScreenDuration);
        float elapsed = 0f;

        while (minutesAdded < totalMinutes)
        {
            elapsed += Time.unscaledDeltaTime;

            // Calculate how many minutes should have passed so far.
            int targetMinutes = Mathf.FloorToInt(
                totalMinutes * Mathf.Clamp01(elapsed / duration));

            int minutesToAdd = targetMinutes - minutesAdded;

            if (minutesToAdd > 0)
            {
                // Notify the existing time systems as the clock advances.
                gameTime.AdvanceMinutes(minutesToAdd);
                minutesAdded = targetMinutes;
            }

            yield return null;
        }
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = fadePanel.alpha;
        float elapsed = 0f;
        float duration = Mathf.Max(0.01f, fadeDuration);

        // Smoothly change the overlay's opacity.
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            fadePanel.alpha = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                Mathf.Clamp01(elapsed / duration));

            yield return null;
        }

        fadePanel.alpha = targetAlpha;
    }

    private void FinishSleep()
    {
        // Hide the overlay and allow interaction again.
        if (fadePanel != null)
        {
            fadePanel.alpha = 0f;
            fadePanel.blocksRaycasts = false;
            fadePanel.gameObject.SetActive(false);
        }

        // Restore the normal clock.
        if (gameTime != null)
            gameTime.enabled = clockWasEnabled;

        IsSleeping = false;
    }

    private void OnDisable()
    {
        // Clean up if the controller is disabled during sleep.
        if (!IsSleeping)
            return;

        if (sleepRoutine != null)
            StopCoroutine(sleepRoutine);

        sleepRoutine = null;
        FinishSleep();
    }
}