using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays one completed project inside the dashboard list.
///
/// Each generated row receives one CompletedProjectRecord.
/// Clicking the row selects that project.
/// </summary>
public class ProjectHistoryRowUI : MonoBehaviour
{
    [Header("Row Button")]
    [SerializeField]
    private Button rowButton;

    [Header("Project Text")]
    [SerializeField]
    private TMP_Text projectNumberText;

    [SerializeField]
    private TMP_Text projectNameText;

    [SerializeField]
    private TMP_Text themeText;

    [SerializeField]
    private TMP_Text genreText;

    [Header("Result Text")]
    [SerializeField]
    private TMP_Text reviewText;

    [SerializeField]
    private TMP_Text moneyEarnedText;

    [SerializeField]
    private TMP_Text completedWeekText;

    // The real data represented by this row.
    private CompletedProjectRecord projectData;

    private void Awake()
    {
        // Automatically find the Button on this object
        // if it was not assigned through the Inspector.
        if (rowButton == null)
        {
            rowButton = GetComponent<Button>();
        }
    }

    /// <summary>
    /// Gives this row its project and displays every value.
    /// </summary>
    public void Setup(
        CompletedProjectRecord project,
        int projectNumber)
    {
        projectData = project;

        if (projectData == null)
        {
            Debug.LogWarning(
                "ProjectHistoryRowUI received null project data.");

            return;
        }

        SetText(
            projectNumberText,
            projectNumber.ToString());

        SetText(
            projectNameText,
            projectData.projectName);

        SetText(
            themeText,
            projectData.theme.ToString().ToUpperInvariant());

        SetText(
            genreText,
            projectData.genre.ToString().ToUpperInvariant());

        // Convert the 0–100 final score into 0–5 stars.
        int starRating =
            ConvertScoreToStars(projectData.finalScore);

        SetText(
            reviewText,
            starRating + " / 5");

        SetText(
            moneyEarnedText,
            "$" + projectData.moneyEarned.ToString("N0"));

        SetText(
            completedWeekText,
            projectData.completedWeek.ToString());

        // Prevent the same listener from being added twice.
        if (rowButton != null)
        {
            rowButton.onClick.RemoveListener(
                SelectThisProject);

            rowButton.onClick.AddListener(
                SelectThisProject);
        }
    }

    /// <summary>
    /// Converts a percentage score into the five-star scale.
    /// </summary>
    private int ConvertScoreToStars(float score)
    {
        return Mathf.Clamp(
            Mathf.RoundToInt(score / 20f),
            0,
            5);
    }

    /// <summary>
    /// Runs when the player clicks this project row.
    /// </summary>
    private void SelectThisProject()
    {
        if (projectData == null)
        {
            return;
        }

        if (ProjectHistoryManager.Instance == null)
        {
            Debug.LogError(
                "ProjectHistoryManager could not be found.");

            return;
        }

        ProjectHistoryManager.Instance.SelectProject(
            projectData);
    }

    /// <summary>
    /// Safely changes a TMP text field.
    /// </summary>
    private void SetText(
        TMP_Text textField,
        string value)
    {
        if (textField != null)
        {
            textField.text = value;
        }
    }

    private void OnDestroy()
    {
        // Remove the listener when Unity destroys the row.
        if (rowButton != null)
        {
            rowButton.onClick.RemoveListener(
                SelectThisProject);
        }
    }
}