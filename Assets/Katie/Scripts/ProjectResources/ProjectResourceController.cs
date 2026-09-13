// Student ID: 23208000

using UnityEngine;

// Applies development stage results to the current project
// and updates the player's permanent resources.
public class ProjectResourceController : MonoBehaviour
{
    [Header("Project Conditions")]
    [SerializeField]
    private ProjectConditions projectConditions =
        new ProjectConditions();

    // Stores the result for each completed development stage.
    [Header("Completed Stage Results")]
    private StageResult codingResult;
    private StageResult designResult;
    private StageResult soundResult;
    private StageResult debuggingResult;

    // Allows other systems to read the current project conditions.
    public ProjectConditions CurrentProjectConditions
    {
        get
        {
            return projectConditions;
        }
    }

    // Checks if a development stage has already been completed.
    public bool HasStageResult(
        DevelopmentStage stage)
    {
        switch (stage)
        {
            case DevelopmentStage.Coding:
                return codingResult != null;

            case DevelopmentStage.Design:
                return designResult != null;

            case DevelopmentStage.Sound:
                return soundResult != null;

            case DevelopmentStage.Debugging:
                return debuggingResult != null;

            default:
                return false;
        }
    }

    // Stores the generated result for the completed stage.
    private void StoreStageResult(
        DevelopmentStage stage,
        StageResult result)
    {
        switch (stage)
        {
            case DevelopmentStage.Coding:
                codingResult = result;
                break;

            case DevelopmentStage.Design:
                designResult = result;
                break;

            case DevelopmentStage.Sound:
                soundResult = result;
                break;

            case DevelopmentStage.Debugging:
                debuggingResult = result;
                break;

            default:
                Debug.LogWarning(
                    "Cannot store result for stage: "
                    + stage);
                break;
        }
    }

    // Applies the result from one completed development stage.
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

        // Stops the same stage from being applied twice.
        if (HasStageResult(stage))
        {
            Debug.LogWarning(
                "This stage has already been processed: "
                + stage);

            return;
        }

        // Update project quality.
        projectConditions.ChangeQuality(
            result.qualityContribution);

        // Update project workload.
        projectConditions.ChangeWorkload(
            result.workload);

        // Debugging removes existing bugs.
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
            // Other development stages can create bugs.
            projectConditions.ChangeBugs(
                result.bugsCreated);
        }

        // Add the time used for this stage.
        projectConditions.ChangeTime(
            result.time);

        // Update the player's permanent resources.
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

        // Store the result so this stage cannot be processed again.
        StoreStageResult(
            stage,
            result);

        Debug.Log(
            "Stage result applied."
            + "\nQuality: " + projectConditions.quality
            + "\nWorkload: " + projectConditions.workload
            + "\nBugs: " + projectConditions.bugs
            + "\nTotal Time: " + projectConditions.totalTime);
    }

    // Resets the project conditions and completed stage results
    // when the player starts a new project.
    public void StartNewProject()
    {
        projectConditions.ResetConditions();

        codingResult = null;
        designResult = null;
        soundResult = null;
        debuggingResult = null;

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