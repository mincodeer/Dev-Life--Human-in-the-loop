using TMPro;
using UnityEngine;

// Put this on an always-active scene object, outside the panel it toggles.
public class UnlockNotificationUI : MonoBehaviour
{
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private TMP_Text notificationText;
    [SerializeField, Min(0.1f)] private float displaySeconds = 5f;
    [SerializeField, TextArea] private string sciFiMessage = "Theme unlocked: SciFi!";
    [SerializeField, TextArea] private string horrorMessage = "Theme unlocked: Horror!";
    [SerializeField, TextArea] private string actionMessage = "Genre unlocked: Action!";
    [SerializeField, TextArea] private string simulationMessage = "Genre unlocked: Simulation!";
    private float hideAt;
    private bool showing;

    private void Awake()
    {
        if (notificationPanel != null) notificationPanel.SetActive(false);
        if (notificationPanel == null || notificationText == null)
            Debug.LogWarning("UnlockNotificationUI: connect Notification Panel and Notification Text in the Inspector.", this);
    }

    private void Update()
    {
        if (notificationPanel == null || notificationText == null) return;
        if (showing && Time.unscaledTime < hideAt) return;
        showing = false;
        notificationPanel.SetActive(false);
        if (ResourceManager.Instance == null) return;
        ProjectUnlockProgression progression = ResourceManager.Instance.GetComponent<ProjectUnlockProgression>();
        if (progression == null || !progression.TryTakeNotification(out var rule)) return;
        switch (rule)
        {
            case ProjectUnlockProgression.Rule.TwoGames: notificationText.text = sciFiMessage; break;
            case ProjectUnlockProgression.Rule.TwoRoomUpgrades: notificationText.text = horrorMessage; break;
            case ProjectUnlockProgression.Rule.Sleep: notificationText.text = actionMessage; break;
            case ProjectUnlockProgression.Rule.Developer: notificationText.text = simulationMessage; break;
        }
        notificationPanel.SetActive(true);
        hideAt = Time.unscaledTime + displaySeconds;
        showing = true;
    }

    private void OnDisable()
    {
        if (notificationPanel != null) notificationPanel.SetActive(false);
        showing = false;
    }
}
