// Student ID: 23208000

using UnityEngine;

/// <summary>
/// Calculates the final outcome of a completed game project.
/// This uses the project's conditions and the player's
/// development skill to calculate the final score and rewards.
/// </summary>
public class FinalResultsCalculator : MonoBehaviour
{
    [Header("Project Resources")]
    [SerializeField]
    private ProjectResourceController projectResourceController;

    [Header("Final Result")]
    [SerializeField]
    private FinalProjectResult finalResult;

    /// <summary>
    /// Calculates the final score of the completed project.
    /// </summary>
    public FinalProjectResult CalculateFinalResult()
    {
        if (projectResourceController == null)
        {
            Debug.LogError(
                "ProjectResourceController is missing.");

            return null;
        }

        ProjectConditions conditions =
            projectResourceController.CurrentProjectConditions;

        // Get the player's permanent Development Skill.
        int developmentSkill = 0;

        if (ResourceManager.Instance != null)
        {
            developmentSkill =
                ResourceManager.Instance.DevelopmentSkill;
        }

        // Quality is the main positive influence.
        float qualityScore =
            conditions.quality;

        // High workload creates a penalty.
        float workloadPenalty =
            conditions.workload * 0.20f;

        // Bugs create a stronger penalty.
        float bugPenalty =
            conditions.bugs * 3f;

        // Development Skill gives a small bonus.
        float skillBonus =
            developmentSkill * 0.20f;

        // Calculate the final project score.
        float score =
            qualityScore
            - workloadPenalty
            - bugPenalty
            + skillBonus;

        // Keep the final score between 0 and 100.
        score = Mathf.Clamp(
            score,
            0f,
            100f);

        finalResult =
            new FinalProjectResult();

        finalResult.finalScore = score;

        // Store the final project conditions.
        finalResult.quality =
            conditions.quality;

        finalResult.workload =
            conditions.workload;

        finalResult.bugs =
            conditions.bugs;

        finalResult.developmentTime =
            conditions.totalTime;

        if (ResourceManager.Instance != null)
        {
            finalResult.technicalDebt =
                ResourceManager.Instance.TechnicalDebt;
        }

        // Calculate the project rewards.
        CalculateMoneyReward(score);
        CalculateFandomReward(score);

        Debug.Log(
            "=== FINAL PROJECT RESULT ==="
            + "\nFinal Score: "
            + finalResult.finalScore
            + "\nMoney Earned: "
            + finalResult.moneyEarned
            + "\nFandom Gained: "
            + finalResult.fandomGained);

        return finalResult;
    }

    // Calculates the amount of Money earned
    // based on the final project score.
    private void CalculateMoneyReward(
        float score)
    {
        if (score < 20)
        {
            finalResult.moneyEarned =
                Random.Range(50, 101);
        }
        else if (score < 40)
        {
            finalResult.moneyEarned =
                Random.Range(100, 176);
        }
        else if (score < 60)
        {
            finalResult.moneyEarned =
                Random.Range(175, 276);
        }
        else if (score < 80)
        {
            finalResult.moneyEarned =
                Random.Range(275, 401);
        }
        else
        {
            finalResult.moneyEarned =
                Random.Range(400, 601);
        }
    }

    // Calculates the amount of Fandom gained
    // based on the final project score.
    private void CalculateFandomReward(
        float score)
    {
        if (score < 20)
        {
            finalResult.fandomGained =
                Random.Range(0, 3);
        }
        else if (score < 40)
        {
            finalResult.fandomGained =
                Random.Range(2, 6);
        }
        else if (score < 60)
        {
            finalResult.fandomGained =
                Random.Range(5, 11);
        }
        else if (score < 80)
        {
            finalResult.fandomGained =
                Random.Range(10, 18);
        }
        else
        {
            finalResult.fandomGained =
                Random.Range(17, 26);
        }
    }

    // Allows the final result calculation to be tested
    // directly from the Unity Inspector.
    [ContextMenu("Calculate Final Result")]
    private void TestFinalResult()
    {
        CalculateFinalResult();
    }
    /// <summary>
    /// Returns the most recently calculated final result.
    /// </summary>
    public FinalProjectResult GetFinalResult()
    {
        return finalResult;
    }
}