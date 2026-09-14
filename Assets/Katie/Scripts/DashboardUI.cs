using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DashboardUI : MonoBehaviour
{
    [Header("Project History")]
    [SerializeField]
    private Transform projectRowsContent;

    [SerializeField]
    private ProjectHistoryRowUI projectRowPrefab;

    [Header("Summary Values")]
    [SerializeField]
    private TMP_Text projectsCompletedValue;

    [SerializeField]
    private TMP_Text totalMoneyEarnedValue;

    [SerializeField]
    private TMP_Text bestReviewValue;

    [SerializeField]
    private TMP_Text overallProgressValue;

    [Header("Overall Progress")]
    [SerializeField]
    private int targetProjectCount = 10;

    [Header("Review Navigation")]
    [SerializeField]
    private Button viewGameReviewButton;

    [SerializeField]
    private string gameReviewSceneName =
        "GameReviewsScene";

    private ProjectHistoryManager historyManager;

    private void Start()
    {
        historyManager =
            ProjectHistoryManager.Instance;

        if (historyManager == null)
        {
            Debug.LogError(
                "DashboardUI could not find "
                + "ProjectHistoryManager."
            );

            return;
        }

        historyManager.HistoryChanged +=
            RefreshDashboard;

        historyManager.SelectedProjectChanged +=
            UpdateReviewButton;

        if (viewGameReviewButton != null)
        {
            viewGameReviewButton.onClick.AddListener(
                OpenSelectedProjectReview
            );
        }

        RefreshDashboard();
        UpdateReviewButton();
    }

    private void RefreshDashboard()
    {
        ClearExistingRows();

        for (int i = 0;
             i < historyManager.CompletedProjects.Count;
             i++)
        {
            CompletedProjectRecord project =
                historyManager.CompletedProjects[i];

            ProjectHistoryRowUI newRow =
                Instantiate(
                    projectRowPrefab,
                    projectRowsContent
                );

            newRow.Setup(
                project,
                i + 1
            );
        }

        UpdateSummary();
    }

    private void ClearExistingRows()
    {
        if (projectRowsContent == null)
        {
            return;
        }

        for (int i =
                 projectRowsContent.childCount - 1;
             i >= 0;
             i--)
        {
            Destroy(
                projectRowsContent.GetChild(i).gameObject
            );
        }
    }

    private void UpdateSummary()
    {
        int projectCount =
            historyManager.ProjectCount;

        SetText(
            projectsCompletedValue,
            projectCount.ToString()
        );

        SetText(
            totalMoneyEarnedValue,
            "$"
            + historyManager
                .GetTotalMoneyEarned()
                .ToString("N0")
        );

        int bestStarRating = Mathf.Clamp(
            Mathf.FloorToInt(
                (historyManager.GetBestFinalScore()
                 / 20f)
                + 0.5f
            ),
            0,
            5
        );

        SetText(
            bestReviewValue,
            bestStarRating + " / 5"
        );

        int progressPercent = 0;

        if (targetProjectCount > 0)
        {
            progressPercent = Mathf.Clamp(
                Mathf.RoundToInt(
                    (float)projectCount
                    / targetProjectCount
                    * 100f
                ),
                0,
                100
            );
        }

        SetText(
            overallProgressValue,
            progressPercent + "%"
        );
    }

    private void UpdateReviewButton()
    {
        if (viewGameReviewButton == null)
        {
            return;
        }

        viewGameReviewButton.interactable =
            historyManager != null
            && historyManager.SelectedProject != null;
    }

    private void OpenSelectedProjectReview()
    {
        if (historyManager == null
            || historyManager.SelectedProject == null)
        {
            Debug.LogWarning(
                "Select a completed project first."
            );

            return;
        }

        SceneManager.LoadScene(
            gameReviewSceneName
        );
    }

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
        if (historyManager != null)
        {
            historyManager.HistoryChanged -=
                RefreshDashboard;

            historyManager.SelectedProjectChanged -=
                UpdateReviewButton;
        }

        if (viewGameReviewButton != null)
        {
            viewGameReviewButton.onClick.RemoveListener(
                OpenSelectedProjectReview
            );
        }
    }
}