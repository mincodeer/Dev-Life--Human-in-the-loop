// Student ID: 23208000

using TMPro;
using UnityEngine;

/// <summary>
/// Displays the final project results on the Results panel.
/// </summary>
public class ResultsUI : MonoBehaviour
{
    // =========================================================
    // RESULT CALCULATOR
    // =========================================================

    [Header("Result Calculator")]

    [SerializeField]
    private FinalResultsCalculator finalResultsCalculator;


    // =========================================================
    // FINAL SCORE
    // =========================================================

    [Header("Final Score")]

    [SerializeField]
    private TMP_Text finalScoreText;


    // =========================================================
    // PROJECT RESULTS
    // =========================================================

    [Header("Project Results")]

    [SerializeField]
    private TMP_Text qualityValue;

    [SerializeField]
    private TMP_Text workloadValue;

    [SerializeField]
    private TMP_Text technicalDebtValue;

    [SerializeField]
    private TMP_Text bugsValue;

    [SerializeField]
    private TMP_Text developmentTimeValue;


    // =========================================================
    // REWARDS
    // =========================================================

    [Header("Rewards")]

    [SerializeField]
    private TMP_Text moneyEarnedValue;

    [SerializeField]
    private TMP_Text fandomGainedValue;


    // =========================================================
    // DISPLAY RESULTS
    // =========================================================

    public void DisplayResults()
    {
        if (finalResultsCalculator == null)
        {
            Debug.LogError(
                "FinalResultsCalculator is missing.");

            return;
        }

        FinalProjectResult result =
            finalResultsCalculator.GetFinalResult();

        if (result == null)
        {
            Debug.LogWarning(
                "No final project result is available.");

            return;
        }


        // Final Score
        if (finalScoreText != null)
        {
            finalScoreText.text =
                "Final Score: "
                + result.finalScore.ToString("0.0")
                + " / 100";
        }


        // Quality
        if (qualityValue != null)
        {
            qualityValue.text =
                "Quality: "
                + result.quality.ToString("0")
                + " / 100";
        }


        // Workload
        if (workloadValue != null)
        {
            workloadValue.text =
                "Workload: "
                + result.workload.ToString("0")
                + " / 100";
        }


        // Technical Debt
        if (technicalDebtValue != null)
        {
            technicalDebtValue.text =
                "Technical Debt: "
                + result.technicalDebt
                + " / 100";
        }


        // Bugs
        if (bugsValue != null)
        {
            bugsValue.text =
                "Bugs: "
                + result.bugs;
        }


        // Development Time
        if (developmentTimeValue != null)
        {
            developmentTimeValue.text =
                "Development Time: "
                + result.developmentTime.ToString("0")
                + " days";
        }


        // Money Earned
        if (moneyEarnedValue != null)
        {
            moneyEarnedValue.text =
                "$"
                + result.moneyEarned;
        }


        // Fandom Gained
        if (fandomGainedValue != null)
        {
            fandomGainedValue.text =
                result.fandomGained.ToString();
        }


        Debug.Log(
            "Results UI updated successfully."
            + "\nFinal Score: " + result.finalScore
            + "\nQuality: " + result.quality
            + "\nWorkload: " + result.workload
            + "\nTechnical Debt: " + result.technicalDebt
            + "\nBugs: " + result.bugs
            + "\nDevelopment Time: " + result.developmentTime
            + "\nMoney Earned: " + result.moneyEarned
            + "\nFandom Gained: " + result.fandomGained);
    }
}