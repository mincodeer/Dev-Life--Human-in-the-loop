using UnityEngine;

/// <summary>
/// Contains all information needed to display one review.
/// </summary>
public class ReviewReportData
{
    public string theme;
    public string genre;
    public string scope;

    public float finalScore;

    public int[] reviewScores;
    public string[] reviewComments;

    public string playerLiked;
    public string playerDisliked;
}

/// <summary>
/// Creates review data for ReviewReportUI.
///
/// This script only reads the selected CompletedProjectRecord.
/// It does not change any development or result scripts.
/// </summary>
public class ReviewReportManager : MonoBehaviour
{
    [Header("Review UI")]
    [SerializeField]
    private ReviewReportUI reviewUI;

    private readonly string[] categoryNames =
    {
        "GAMEPLAY",
        "ART STYLE",
        "AUDIO"
    };

    private void Start()
    {
        RefreshReview();
    }

    /// <summary>
    /// Displays the project selected from the Dashboard.
    /// </summary>
    [ContextMenu("Refresh Review")]
    public void RefreshReview()
    {
        FindReviewUI();

        if (reviewUI == null)
        {
            Debug.LogError(
                "ReviewReportUI is missing.");

            return;
        }

        CompletedProjectRecord selectedProject =
            GetSelectedProject();

        ReviewReportData report;

        if (selectedProject != null)
        {
            report =
                CreateProjectReview(selectedProject);
        }
        else
        {
            Debug.LogWarning(
                "No completed project was selected. " +
                "Showing a sample review.");

            report = CreateSampleReview();
        }

        reviewUI.DisplayReport(report);
    }

    /// <summary>
    /// Finds the ReviewReportUI component if it was not
    /// assigned through the Inspector.
    /// </summary>
    private void FindReviewUI()
    {
        if (reviewUI == null)
        {
            reviewUI =
                GetComponent<ReviewReportUI>();
        }

        if (reviewUI == null)
        {
            reviewUI =
                FindFirstObjectByType<ReviewReportUI>();
        }
    }

    /// <summary>
    /// Gets the project selected on the Dashboard.
    /// </summary>
    private CompletedProjectRecord GetSelectedProject()
    {
        if (ProjectHistoryManager.Instance == null)
        {
            Debug.LogWarning(
                "ProjectHistoryManager was not found.");

            return null;
        }

        return
            ProjectHistoryManager.Instance.SelectedProject;
    }

    /// <summary>
    /// Creates a review from a completed project.
    ///
    /// Only fields stored inside CompletedProjectRecord are read.
    /// </summary>
    private ReviewReportData CreateProjectReview(
        CompletedProjectRecord project)
    {
        ReviewReportData report =
            new ReviewReportData();

        report.theme =
            project.theme.ToString();

        report.genre =
            project.genre.ToString();

        // Scope is not included in the friend's original
        // ProjectData, so display N/A.
        report.scope = "N/A";

        report.finalScore =
            Mathf.Clamp(
                project.finalScore,
                0f,
                100f);

        report.reviewScores =
            CreateReviewScores(report.finalScore);

        report.reviewComments =
            CreateReviewComments(
                report.reviewScores);

        CreatePlayerFeedback(
            report,
            report.reviewScores);

        return report;
    }

    /// <summary>
    /// Creates three category scores using only the final score.
    ///
    /// This avoids requiring extra fields inside the friend's
    /// FinalProjectResult class.
    /// </summary>
    private int[] CreateReviewScores(
        float finalScore)
    {
        int baseScore =
            Mathf.Clamp(
                Mathf.RoundToInt(finalScore),
                0,
                100);

        return new int[]
        {
            Mathf.Clamp(baseScore + 3, 0, 100),
            Mathf.Clamp(baseScore, 0, 100),
            Mathf.Clamp(baseScore - 3, 0, 100)
        };
    }

    private string[] CreateReviewComments(
        int[] scores)
    {
        return new string[]
        {
            GetGameplayComment(scores[0]),
            GetArtComment(scores[1]),
            GetAudioComment(scores[2])
        };
    }

    private string GetGameplayComment(
        int score)
    {
        if (score >= 70)
        {
            return
                "The gameplay was really fun!";
        }

        if (score >= 45)
        {
            return
                "The gameplay was enjoyable.";
        }

        return
            "The gameplay needs more variety.";
    }

    private string GetArtComment(
        int score)
    {
        if (score >= 70)
        {
            return
                "Beautiful art and atmosphere.";
        }

        if (score >= 45)
        {
            return
                "The art style was interesting.";
        }

        return
            "The art needs more improvement.";
    }

    private string GetAudioComment(
        int score)
    {
        if (score >= 70)
        {
            return
                "The audio worked really well.";
        }

        if (score >= 45)
        {
            return
                "The audio was acceptable.";
        }

        return
            "The audio was repetitive.";
    }

    /// <summary>
    /// Displays the two strongest categories as liked and
    /// the weakest category as disliked.
    /// </summary>
    private void CreatePlayerFeedback(
        ReviewReportData report,
        int[] scores)
    {
        int highestIndex = 0;
        int lowestIndex = 0;

        for (int i = 1; i < scores.Length; i++)
        {
            if (scores[i] > scores[highestIndex])
            {
                highestIndex = i;
            }

            if (scores[i] < scores[lowestIndex])
            {
                lowestIndex = i;
            }
        }

        int secondHighestIndex = -1;

        for (int i = 0; i < scores.Length; i++)
        {
            if (i == highestIndex)
            {
                continue;
            }

            if (secondHighestIndex == -1 ||
                scores[i] >
                scores[secondHighestIndex])
            {
                secondHighestIndex = i;
            }
        }

        report.playerLiked =
            categoryNames[highestIndex];

        if (secondHighestIndex >= 0)
        {
            report.playerLiked +=
                " - " +
                categoryNames[secondHighestIndex];
        }

        if (scores[lowestIndex] >= 70)
        {
            report.playerDisliked = "NOTHING";
        }
        else
        {
            report.playerDisliked =
                categoryNames[lowestIndex];
        }
    }

    /// <summary>
    /// Creates sample data when the Review scene is opened
    /// without selecting a Dashboard project.
    /// </summary>
    private ReviewReportData CreateSampleReview()
    {
        ReviewReportData report =
            new ReviewReportData();

        report.theme = "Fantasy";
        report.genre = "RPG";
        report.scope = "Large";
        report.finalScore = 80f;

        report.reviewScores =
            CreateReviewScores(report.finalScore);

        report.reviewComments =
            CreateReviewComments(
                report.reviewScores);

        CreatePlayerFeedback(
            report,
            report.reviewScores);

        return report;
    }
}