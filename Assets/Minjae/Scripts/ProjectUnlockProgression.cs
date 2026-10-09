using System.Collections.Generic;
using UnityEngine;

// Progression only: the scene's UnlockNotificationUI owns presentation.
public class ProjectUnlockProgression : MonoBehaviour
{
    public enum Rule { Manual, TwoGames, TwoRoomUpgrades, Sleep, Developer }
    private readonly HashSet<Rule> unlocked = new HashSet<Rule>();
    private readonly Queue<Rule> notifications = new Queue<Rule>();
    private bool hasSlept;

    public bool IsUnlocked(Rule rule) => unlocked.Contains(rule);

    public bool TryTakeNotification(out Rule rule)
    {
        if (notifications.Count > 0)
        {
            rule = notifications.Dequeue();
            return true;
        }
        rule = Rule.Manual;
        return false;
    }

    // Call only after sleeping successfully completes.
    [ContextMenu("TEST - Record Sleep Completed")]
    public void RecordSleep()
    {
        hasSlept = true;
        Evaluate();
    }

    private void Update() => Evaluate();

    private void Evaluate()
    {
        if (ProjectDataManager.Instance != null && ProjectDataManager.Instance.CompletedProjects.Count >= 2)
            Unlock(Rule.TwoGames);
        if (RoomUpgradeManager.Instance != null && RoomUpgradeManager.Instance.PurchasedUpgradeCount >= 2)
            Unlock(Rule.TwoRoomUpgrades);
        if (hasSlept) Unlock(Rule.Sleep);
        if (ResourceManager.Instance != null && ResourceManager.Instance.DevelopmentSkill >= 41)
            Unlock(Rule.Developer);
    }

    private void Unlock(Rule rule)
    {
        if (unlocked.Add(rule)) notifications.Enqueue(rule);
    }
}
