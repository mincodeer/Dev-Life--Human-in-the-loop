using UnityEngine;

/// <summary>
/// Generates the consequences of Manual or AI decisions
/// during each development stage.
/// </summary>
public class ProjectResultsCalculator : MonoBehaviour
{
    [SerializeField]
    private ProjectResourceController projectResourceController;
    /// <summary>
    /// Generates a StageResult based on the selected
    /// development stage and work method.
    /// </summary>
    public StageResult GenerateStageResult(
        DevelopmentStage stage,
        WorkMethod method)
    {
        StageResult result = new StageResult();

        switch (stage)
        {
            case DevelopmentStage.Coding:
                GenerateCodingResult(result, method);
                break;

            case DevelopmentStage.Design:
                GenerateDesignResult(result, method);
                break;

            case DevelopmentStage.Sound:
                GenerateSoundResult(result, method);
                break;

            case DevelopmentStage.Debugging:
                GenerateDebuggingResult(result, method);
                break;

            default:
                Debug.LogWarning(
                    "Cannot generate a result for stage: "
                    + stage);
                break;
        }

        return result;
    }

    // ==============================
    // Coding
    // ==============================

    private void GenerateCodingResult(
        StageResult result,
        WorkMethod method)
    {
        if (method == WorkMethod.Manual)
        {
            result.time = Random.Range(5, 8);
            result.qualityContribution = Random.Range(10, 19);
            result.bugsCreated = Random.Range(0, 3);
            result.workload = Random.Range(12, 21);
            result.skillGained = Random.Range(4, 8);
            result.technicalDebtChange = Random.Range(0, 3);
        }
        else if (method == WorkMethod.AI)
        {
            result.time = Random.Range(2, 5);
            result.qualityContribution = Random.Range(5, 16);
            result.bugsCreated = Random.Range(2, 9);
            result.workload = Random.Range(4, 11);
            result.skillGained = Random.Range(1, 4);
            result.technicalDebtChange = Random.Range(8, 21);
        }
    }

    // ==============================
    // Design
    // ==============================

    private void GenerateDesignResult(
        StageResult result,
        WorkMethod method)
    {
        if (method == WorkMethod.Manual)
        {
            result.time = Random.Range(4, 7);
            result.qualityContribution = Random.Range(10, 19);
            result.bugsCreated = Random.Range(0, 2);
            result.workload = Random.Range(10, 19);
            result.skillGained = Random.Range(3, 7);
            result.technicalDebtChange = Random.Range(0, 2);
        }
        else if (method == WorkMethod.AI)
        {
            result.time = Random.Range(2, 4);
            result.qualityContribution = Random.Range(4, 15);
            result.bugsCreated = Random.Range(1, 5);
            result.workload = Random.Range(3, 9);
            result.skillGained = Random.Range(1, 3);
            result.technicalDebtChange = Random.Range(4, 11);
        }
    }

    // ==============================
    // Sound
    // ==============================

    private void GenerateSoundResult(
        StageResult result,
        WorkMethod method)
    {
        if (method == WorkMethod.Manual)
        {
            result.time = Random.Range(3, 6);
            result.qualityContribution = Random.Range(8, 16);
            result.bugsCreated = Random.Range(0, 2);
            result.workload = Random.Range(6, 13);
            result.skillGained = Random.Range(2, 5);
            result.technicalDebtChange = Random.Range(0, 2);
        }
        else if (method == WorkMethod.AI)
        {
            result.time = Random.Range(1, 4);
            result.qualityContribution = Random.Range(3, 13);
            result.bugsCreated = Random.Range(0, 4);
            result.workload = Random.Range(2, 7);
            result.skillGained = Random.Range(0, 3);
            result.technicalDebtChange = Random.Range(2, 7);
        }
    }

    // ==============================
    // Debugging
    // ==============================

    private void GenerateDebuggingResult(
    StageResult result,
    WorkMethod method)
    {
        if (method == WorkMethod.Manual)
        {
            // Manual debugging takes longer but removes
            // a larger percentage of existing bugs.
            result.time = Random.Range(4, 7);

            result.qualityContribution = Random.Range(3, 9);

            result.bugRemovalPercentage =
                Random.Range(0.60f, 0.91f);

            result.workload = Random.Range(10, 21);

            result.skillGained = Random.Range(3, 7);

            // Manual debugging can slightly reduce
            // or only minimally increase technical debt.
            result.technicalDebtChange =
                Random.Range(-2, 2);
        }
        else if (method == WorkMethod.AI)
        {
            // AI debugging is faster but generally removes
            // fewer bugs and may add technical debt.
            result.time = Random.Range(2, 5);

            result.qualityContribution = Random.Range(1, 6);

            result.bugRemovalPercentage =
                Random.Range(0.30f, 0.71f);

            result.workload = Random.Range(4, 11);

            result.skillGained = Random.Range(1, 4);

            result.technicalDebtChange =
                Random.Range(2, 7);
        }
    }
    /// <summary>
    /// Generates and immediately applies the consequences
    /// of a development decision.
    /// </summary>
    public void ProcessStageDecision(
        DevelopmentStage stage,
        WorkMethod method)
    {
        if (projectResourceController == null)
        {
            Debug.LogError(
                "ProjectResourceController has not been assigned.");

            return;
        }

        StageResult result =
            GenerateStageResult(stage, method);

        projectResourceController.ApplyStageResult(
        stage,
        result);

        Debug.Log(
            "Processed decision: "
            + stage
            + " using "
            + method);
    }

    [ContextMenu("Process AI Coding")]
    private void ProcessAICoding()
    {
        ProcessStageDecision(
            DevelopmentStage.Coding,
            WorkMethod.AI);
    }

    [ContextMenu("Process Manual Coding")]
    private void ProcessManualCoding()
    {
        ProcessStageDecision(
            DevelopmentStage.Coding,
            WorkMethod.Manual);
    }
    [ContextMenu("Process AI Design")]
    private void ProcessAIDesign()
    {
        ProcessStageDecision(
            DevelopmentStage.Design,
            WorkMethod.AI);
    }

    [ContextMenu("Process Manual Design")]
    private void ProcessManualDesign()
    {
        ProcessStageDecision(
            DevelopmentStage.Design,
            WorkMethod.Manual);
    }

    [ContextMenu("Process AI Sound")]
    private void ProcessAISound()
    {
        ProcessStageDecision(
            DevelopmentStage.Sound,
            WorkMethod.AI);
    }

    [ContextMenu("Process Manual Sound")]
    private void ProcessManualSound()
    {
        ProcessStageDecision(
            DevelopmentStage.Sound,
            WorkMethod.Manual);
    }
    [ContextMenu("Process AI Debugging")]
    private void ProcessAIDebugging()
    {
        ProcessStageDecision(
            DevelopmentStage.Debugging,
            WorkMethod.AI);
    }

    [ContextMenu("Process Manual Debugging")]
    private void ProcessManualDebugging()
    {
        ProcessStageDecision(
            DevelopmentStage.Debugging,
            WorkMethod.Manual);
    }
}