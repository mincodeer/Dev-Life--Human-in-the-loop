//Student ID: 23208000

using UnityEngine;

// Applies development stage results to the current project's
// conditions and the player's permanent resources.
public class ProjectResourceController : MonoBehaviour
{
    [Header("Project Conditions")]
    [SerializeField]
    private ProjectConditions projectConditions =
        new ProjectConditions();

    // Allows other systems to read the current project conditions.
    public ProjectConditions CurrentProjectConditions
    {
        get
        {
            return projectConditions;
        }
    }

    // Applies the consequences of one completed development stage.
    public void ApplyStageResult(
    DevelopmentStage stage,
    StageResult result)
    {
        if (result == null)
        {
            Debug.LogWarning(
                "Cannot apply a null StageResult.");

            return;
        }

        // Update the current project's conditions.
        projectConditions.ChangeQuality(
            result.qualityContribution);

        projectConditions.ChangeWorkload(
            result.workload);

        if (stage == DevelopmentStage.Debugging)
        {
            int bugsToRemove = Mathf.RoundToInt(
                projectConditions.bugs
                * result.bugRemovalPercentage);

            projectConditions.ChangeBugs(
                -bugsToRemove);

            Debug.Log(
                "Debugging removed "
                + bugsToRemove
                + " bugs.");
        }
        else
        {
            projectConditions.ChangeBugs(
                result.bugsCreated);
        }

        projectConditions.ChangeTime(
            result.time);

        // Update permanent player resources.
        if (ResourceManager.Instance != null)
        {
            ResourceManager.Instance.ChangeDevelopmentSkill(
                result.skillGained);

            ResourceManager.Instance.ChangeTechnicalDebt(
                result.technicalDebtChange);
        }
        else
        {
            Debug.LogWarning(
                "ResourceManager is missing.");
        }

        Debug.Log(
            "Stage result applied."
            + "\nQuality: " + projectConditions.quality
            + "\nWorkload: " + projectConditions.workload
            + "\nBugs: " + projectConditions.bugs
            + "\nTotal Time: " + projectConditions.totalTime);
    }

    /// <summary>
    /// Resets the current project's conditions when a new
    /// project begins.
    /// </summary>
    public void StartNewProject()
    {
        projectConditions.ResetConditions();

        Debug.Log(
            "New project conditions started.");
    }

    [ContextMenu("Print Project Conditions")]
    public void PrintProjectConditions()
    {
        Debug.Log(
            "=== CURRENT PROJECT CONDITIONS ==="
            + "\nQuality: " + projectConditions.quality
            + "\nWorkload: " + projectConditions.workload
            + "\nBugs: " + projectConditions.bugs
            + "\nTotal Time: " + projectConditions.totalTime);
    }
}