using UnityEngine;

/// <summary>
/// Saves a calculated project result in the project history.
/// </summary>
public class ProjectCompletionBridge : MonoBehaviour
{
    [Header("Result Calculator")]
    [SerializeField]
    private FinalResultsCalculator finalResultsCalculator;

    private FinalProjectResult lastSavedResult;

    /// <summary>
    /// Call this from the same Results button after
    /// DevelopmentFlowManager.GotoResult().
    /// </summary>
    public void SaveCompletedProject()
    {
        if (finalResultsCalculator == null)
        {
            finalResultsCalculator =
                FindFirstObjectByType<FinalResultsCalculator>();
        }

        if (finalResultsCalculator == null)
        {
            Debug.LogError(
                "FinalResultsCalculator was not found.");
            return;
        }

        FinalProjectResult finalResult =
            finalResultsCalculator.GetFinalResult();

        if (finalResult == null)
        {
            Debug.LogWarning(
                "The final result has not been calculated yet. " +
                "Place SaveCompletedProject after GotoResult " +
                "in the Button On Click list.");
            return;
        }

        if (ReferenceEquals(finalResult, lastSavedResult))
        {
            return;
        }

        if (ProjectDataManager.Instance == null)
        {
            Debug.LogError(
                "ProjectDataManager was not found.");
            return;
        }

        if (ProjectHistoryManager.Instance == null)
        {
            Debug.LogError(
                "ProjectHistoryManager was not found.");
            return;
        }

        ProjectHistoryManager.Instance
            .AddCompletedProjectFromResult(
                ProjectDataManager.Instance.CurrentProject,
                finalResult);

        lastSavedResult = finalResult;
    }
}
