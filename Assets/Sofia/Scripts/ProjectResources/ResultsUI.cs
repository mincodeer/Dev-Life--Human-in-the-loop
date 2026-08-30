// Student ID: 23208000

using TMPro;
using UnityEngine;

/// <summary>
/// Displays the final results of a completed project.
/// This takes the calculated result and shows it
/// on the Results panel for the player.
/// </summary>
public class ResultsUI : MonoBehaviour
{
    [Header("Result Calculator")]
    [SerializeField]
    private FinalResultsCalculator finalResultsCalculator;

    [Header("Result Text")]
    [SerializeField]
    private TMP_Text resultText;

    /// <summary>
    /// Displays the final project result on the UI.
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

        resultText.text =
            "PROJECT COMPLETE!\n\n"
            + "Final Score: "
            + result.finalScore.ToString("0.0")
            + " / 100\n\n"

            + "Quality: "
            + result.quality.ToString("0")
            + " / 100\n"

            + "Workload: "
            + result.workload.ToString("0")
            + " / 100\n"

            + "Technical Debt: "
            + result.technicalDebt
            + " / 100\n"

            + "Bugs: "
            + result.bugs
            + "\n"

            + "Development Time: "
            + result.developmentTime.ToString("0")
            + " days\n\n"

            + "Money Earned: +$"
            + result.moneyEarned
            + "\n"

            + "Fandom Gained: +"
            + result.fandomGained;
    }
}