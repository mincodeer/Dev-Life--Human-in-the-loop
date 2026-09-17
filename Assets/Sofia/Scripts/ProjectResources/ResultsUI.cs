// Student ID: 23208000

using TMPro;
using UnityEngine;

/// <summary>
/// Displays the final results of a completed project
/// on the Results panel.
/// </summary>
public class ResultsUI : MonoBehaviour
{
    [Header("Result Calculator")]
    [SerializeField]
    private FinalResultsCalculator finalResultsCalculator;

    [Header("Final Score")]
    [SerializeField]
    private TMP_Text finalScoreText;

    [Header("Project Results")]
    [SerializeField]
    private TMP_Text qualityValue;

    [SerializeField]
    private TMP_Text workloadValue;

    [SerializeField]
    private TMP_Text technicalDebtValue;

    [SerializeField]
    private TMP_Text bugsValue;

    /// <summary>
    /// Gets the calculated final result and
    /// displays the values on the Results panel.
    /// </summary>
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
                result.finalScore.ToString("0.0")
                + " / 100";
        }

        // Quality
        if (qualityValue != null)
        {
            qualityValue.text =
                result.quality.ToString("0")
                + " / 100";
        }

        // Workload
        if (workloadValue != null)
        {
            workloadValue.text =
                result.workload.ToString("0")
                + " / 100";
        }

        // Technical Debt
        if (technicalDebtValue != null)
        {
            technicalDebtValue.text =
                result.technicalDebt
                + " / 100";
        }

        // Bugs
        if (bugsValue != null)
        {
            bugsValue.text =
                result.bugs.ToString();
        }

        Debug.Log(
            "Results UI updated successfully."
            + "\nFinal Score: "
            + result.finalScore
            + "\nQuality: "
            + result.quality
            + "\nWorkload: "
            + result.workload
            + "\nTechnical Debt: "
            + result.technicalDebt
            + "\nBugs: "
            + result.bugs);
    }
}