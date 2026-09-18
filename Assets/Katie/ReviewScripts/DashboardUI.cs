using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the Game Dashboard screen.
///
/// It creates the completed-project rows, displays studio totals,
/// shows the selected project and calculates overall progress.
/// </summary>
public class DashboardUI : MonoBehaviour
{
    // ==============================
    // Completed Project List
    // ==============================

    [Header("Completed Project List")]
    [SerializeField]
    private Transform projectRowsContent;

    [SerializeField]
    private ProjectHistoryRowUI projectRowPrefab;

    // ==============================
    // Studio Status
    // ==============================

    [Header("Studio Status")]
    [SerializeField]
    private TMP_Text projectsCompletedValue;

    [SerializeField]
    private TMP_Text totalMoneyEarnedValue;

    [SerializeField]
    private TMP_Text bestReviewValue;

    [SerializeField]
    private TMP_Text totalFansValue;

    // ==============================
    // Selected Project
    // ==============================

    [Header("Selected Project")]
    [SerializeField]
    private TMP_Text selectedProjectNameText;

    [SerializeField]
    private TMP_Text selectedThemeGenreText;

    [SerializeField]
    private TMP_Text selectedReleaseText;

    [SerializeField]
    private TMP_Text selectedReviewText;

    [SerializeField]
    private TMP_Text selectedQualityText;

    [SerializeField]
    private TMP_Text selectedSalesText;

    [SerializeField]
    private TMP_Text selectedSuccessText;

    // ==============================
    // Overall Progress
    // ==============================

    [Header("Overall Progress Display")]
    [SerializeField]
    private TMP_Text overallProgressPercentText;

    [SerializeField]
    private Image overallProgressFill;

    [SerializeField]
    private TMP_Text projectProgressText;

    [SerializeField]
    private TMP_Text moneyProgressText;

    [SerializeField]
    private TMP_Text averageQualityText;

    [SerializeField]
    private TMP_Text fandomProgressText;

    [Header("Overall Progress Targets")]
    [SerializeField]
    private int targetProjectCount = 10;

    [SerializeField]
    private int targetMoney = 10000;

    [SerializeField]
    private int targetFandom = 500;

    // ==============================
    // Navigation
    // ==============================

    [Header("Navigation Buttons")]
    [SerializeField]
    private Button viewGameReviewButton;

    [SerializeField]
    private Button backToMenuButton;

    [SerializeField]
    private string gameReviewSceneName =
        "GameReviewsScene";

    [SerializeField]
    private string mainMenuSceneName =
        "MainMenu";

    // Stores a reference to the persistent history manager.
    private ProjectHistoryManager historyManager;

    private void Start()
    {
        // Navigation must work even if the history manager is unavailable.
        if (backToMenuButton != null)
            backToMenuButton.onClick.AddListener(BackToMenu);

        // Find the persistent project-history system.
        historyManager =
            ProjectHistoryManager.Instance;

        if (historyManager == null)
        {
            Debug.LogError(
                "DashboardUI could not find "
                + "ProjectHistoryManager.");

            return;
        }

        // Refresh the complete dashboard whenever
        // a new project is added.
        historyManager.HistoryChanged +=
            RefreshDashboard;

        // Update only the selected-project panel
        // whenever the player clicks a project row.
        historyManager.SelectedProjectChanged +=
            UpdateSelectedProject;

        // Connect the View Review button.
        if (viewGameReviewButton != null)
        {
            viewGameReviewButton.onClick.AddListener(
                OpenSelectedProjectReview);
        }


        // Draw the dashboard for the first time.
        RefreshDashboard();
    }

    /// <summary>
    /// Refreshes every section of the dashboard.
    /// </summary>
    private void RefreshDashboard()
    {
        RebuildProjectRows();
        UpdateStudioStatus();
        UpdateOverallProgress();

        // Select the first project when projects exist
        // but none is currently selected.
        if (historyManager.SelectedProject == null
            && historyManager.ProjectCount > 0)
        {
            historyManager.SelectProject(
                historyManager.CompletedProjects[0]);
        }
        else
        {
            UpdateSelectedProject();
        }
    }

    /// <summary>
    /// Deletes old rows and generates one row for
    /// every completed project.
    /// </summary>
    private void RebuildProjectRows()
    {
        if (projectRowsContent == null
            || projectRowPrefab == null)
        {
            Debug.LogWarning(
                "Dashboard project-row references "
                + "are missing.");

            return;
        }

        // Delete all previously generated rows.
        for (int i = projectRowsContent.childCount - 1;
             i >= 0;
             i--)
        {
            Destroy(
                projectRowsContent.GetChild(i).gameObject);
        }

        // Generate one row for every completed project.
        for (int i = 0;
             i < historyManager.CompletedProjects.Count;
             i++)
        {
            CompletedProjectRecord project =
                historyManager.CompletedProjects[i];

            ProjectHistoryRowUI newRow =
                Instantiate(
                    projectRowPrefab,
                    projectRowsContent);

            // Give the new row its project data.
            newRow.Setup(
                project,
                i + 1);
        }

        // Force Unity to calculate the new content height.
        // This allows the Scroll Rect to reach every row.
        Canvas.ForceUpdateCanvases();

        RectTransform contentRect =
            projectRowsContent as RectTransform;

        if (contentRect != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(
                contentRect);
        }
    }

    /// <summary>
    /// Updates Projects, Money, Best Review and Fans.
    /// </summary>
    private void UpdateStudioStatus()
    {
        int projectCount =
            historyManager.ProjectCount;

        int totalMoney =
            historyManager.GetTotalMoneyEarned();

        int totalFandom =
            historyManager.GetTotalFandomGained();

        int bestStars =
            ConvertScoreToStars(
                historyManager.GetBestFinalScore());

        SetText(
            projectsCompletedValue,
            projectCount.ToString());

        SetText(
            totalMoneyEarnedValue,
            "$" + totalMoney.ToString("N0"));

        SetText(
            bestReviewValue,
            bestStars + " / 5");

        SetText(
            totalFansValue,
            totalFandom.ToString("N0"));
    }

    /// <summary>
    /// Displays the project selected from the list.
    /// </summary>
    private void UpdateSelectedProject()
    {
        if (historyManager == null)
        {
            return;
        }

        CompletedProjectRecord project =
            historyManager.SelectedProject;

        // Show empty values when no project exists.
        if (project == null)
        {
            SetText(
                selectedProjectNameText,
                "NO PROJECT");

            SetText(
                selectedThemeGenreText,
                "---");

            SetText(
                selectedReleaseText,
                "RELEASE: ---");

            SetText(
                selectedReviewText,
                "REVIEW: 0 / 5");

            SetText(
                selectedQualityText,
                "QUALITY: 0%");

            SetText(
                selectedSalesText,
                "SALES: $0");

            SetText(
                selectedSuccessText,
                "SUCCESS: ---");

            UpdateReviewButton();
            return;
        }

        int starRating =
            ConvertScoreToStars(project.finalScore);

        SetText(
            selectedProjectNameText,
            project.projectName.ToUpperInvariant());

        SetText(
            selectedThemeGenreText,
            project.theme.ToString().ToUpperInvariant()
            + " / "
            + project.genre.ToString().ToUpperInvariant());

        SetText(
            selectedReleaseText,
            "RELEASE: WEEK "
            + project.completedWeek);

        SetText(
            selectedReviewText,
            "REVIEW: "
            + starRating
            + " / 5");

        SetText(
            selectedQualityText,
            "QUALITY: "
            + Mathf.RoundToInt(project.quality)
            + "%");

        // There is no separate sales field yet,
        // so Money Earned is displayed as Sales.
        SetText(
            selectedSalesText,
            "SALES: $"
            + project.moneyEarned.ToString("N0"));

        SetText(
            selectedSuccessText,
            "SUCCESS: "
            + GetSuccessDescription(
                project.finalScore));

        UpdateReviewButton();
    }

    /// <summary>
    /// Calculates overall progress using only completed projects.
    /// Money, quality and fandom are display-only statistics.
    /// </summary>
    private void UpdateOverallProgress()
    {
        int projectCount =
            historyManager.ProjectCount;

        int totalMoney =
            historyManager.GetTotalMoneyEarned();

        int totalFandom =
            historyManager.GetTotalFandomGained();

        float averageQuality =
            historyManager.GetAverageQuality();

        // Ten projects (or the configured target) equals 100%.
        float overallProgress =
            GetProgressRatio(
                projectCount,
                targetProjectCount);

        int overallPercent =
            Mathf.RoundToInt(
                overallProgress * 100f);

        SetText(
            overallProgressPercentText,
            overallPercent + "%");

        SetText(
            projectProgressText,
            "PROJECTS: "
            + projectCount
            + " / "
            + targetProjectCount);

        SetText(
            moneyProgressText,
            "MONEY: $"
            + totalMoney.ToString("N0"));

        SetText(
            averageQualityText,
            "AVG QUALITY: "
            + Mathf.RoundToInt(averageQuality)
            + " / 100");

        SetText(
            fandomProgressText,
            "FANDOM: "
            + totalFandom.ToString("N0"));

        // The Fill Image must use:
        // Image Type = Filled
        // Fill Method = Horizontal
        // Fill Origin = Left
        if (overallProgressFill != null)
        {
            overallProgressFill.fillAmount =
                overallProgress;
        }
    }

    /// <summary>
    /// Returns progress as a value from zero to one.
    /// </summary>
    private float GetProgressRatio(
        float currentValue,
        float targetValue)
    {
        if (targetValue <= 0f)
        {
            return 0f;
        }

        return Mathf.Clamp01(
            currentValue / targetValue);
    }

    /// <summary>
    /// Converts a 0-100 score into 0-5 stars.
    /// </summary>
    private int ConvertScoreToStars(
        float score)
    {
        return Mathf.Clamp(
            Mathf.RoundToInt(score / 20f),
            0,
            5);
    }

    /// <summary>
    /// Returns the success description for a score.
    /// </summary>
    private string GetSuccessDescription(
        float score)
    {
        if (score >= 80f)
        {
            return "GREAT SUCCESS";
        }

        if (score >= 60f)
        {
            return "SUCCESS";
        }

        if (score >= 40f)
        {
            return "MIXED";
        }

        return "POOR";
    }

    /// <summary>
    /// Enables View Review only when a project
    /// is currently selected.
    /// </summary>
    private void UpdateReviewButton()
    {
        if (viewGameReviewButton != null)
        {
            viewGameReviewButton.interactable =
                historyManager != null
                && historyManager.SelectedProject != null;
        }
    }

    /// <summary>
    /// Opens the selected project's Review scene.
    /// </summary>
    private void OpenSelectedProjectReview()
    {
        if (historyManager == null
            || historyManager.SelectedProject == null)
        {
            Debug.LogWarning(
                "Select a completed project first.");

            return;
        }

        if (FeatureSceneNavigation.Instance != null)
            FeatureSceneNavigation.Instance.OpenScene(gameReviewSceneName);
        else
            Debug.LogWarning("Open Dashboard from the main room scene first.");
    }

    /// <summary>
    /// Returns to the already loaded gameplay room.
    /// </summary>
    private void BackToMenu()
    {
        if (FeatureSceneNavigation.Instance != null)
            FeatureSceneNavigation.Instance.BackToRoom();
        else
            Debug.LogWarning("Open Dashboard from the main room scene first.");
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
        // Remove history event subscriptions.
        if (historyManager != null)
        {
            historyManager.HistoryChanged -=
                RefreshDashboard;

            historyManager.SelectedProjectChanged -=
                UpdateSelectedProject;
        }

        // Remove button listeners.
        if (viewGameReviewButton != null)
        {
            viewGameReviewButton.onClick.RemoveListener(
                OpenSelectedProjectReview);
        }

        if (backToMenuButton != null)
        {
            backToMenuButton.onClick.RemoveListener(
                BackToMenu);
        }
    }
}
