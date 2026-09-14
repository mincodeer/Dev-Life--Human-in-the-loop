using UnityEngine;

/// Contains all information needed to display one review.
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

/// Creates the review data.
/// This script does not directly control UI objects.
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

    /// Reads the currently selected project and creates a fresh review whenever the Review scene opens.
    [ContextMenu("Refresh Review")]
    public void RefreshReview()
    {
        if (reviewUI == null)
        {
            reviewUI = GetComponent<ReviewReportUI>();
        }

        if (reviewUI == null)
        {
            Debug.LogError(
                "ReviewReportUI is missing.");

            return;
        }

        CompletedProjectRecord selectedProject = null;

        if (ProjectHistoryManager.Instance != null)
        {
            selectedProject =
                ProjectHistoryManager.Instance.SelectedProject;
        }

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

    /// Creates a review using a real completed project.
    private ReviewReportData CreateProjectReview(
        CompletedProjectRecord project)
    {
        ReviewReportData report =
            new ReviewReportData();

        // Always read Theme and Genre from the latest
        // selected completed project.
        report.theme =
            project.theme.ToString();

        report.genre =
            project.genre.ToString();

        // Scope is not stored yet.
        report.scope = "N/A";

        report.finalScore =
            Mathf.Clamp(project.finalScore, 0f, 100f);

        report.reviewScores =
            CreateProjectReviewScores(project);

        report.reviewComments =
            CreateProjectComments(
                project,
                report.reviewScores);

        CreatePlayerFeedback(
            report,
            report.reviewScores);

        return report;
    }

    /// Creates category scores using the detailed
    /// values saved in CompletedProjectRecord.
    private int[] CreateProjectReviewScores(
        CompletedProjectRecord project)
    {
        int baseScore =
            Mathf.Clamp(
                Mathf.RoundToInt(project.finalScore),
                0,
                100);

        int gameplayScore =
            baseScore -
            Mathf.RoundToInt(project.bugs * 2f);

        int artScore =
            Mathf.RoundToInt(
                (baseScore + project.quality) / 2f);

        int audioScore =
            baseScore -
            Mathf.RoundToInt(
                project.technicalDebt * 0.25f);

        return new int[]
        {
            Mathf.Clamp(gameplayScore, 0, 100),
            Mathf.Clamp(artScore, 0, 100),
            Mathf.Clamp(audioScore, 0, 100)
        };
    }

    private string[] CreateProjectComments(
        CompletedProjectRecord project,
        int[] scores)
    {
        return new string[]
        {
            CreateGameplayComment(project, scores[0]),
            CreateArtComment(project, scores[1]),
            CreateAudioComment(project, scores[2])
        };
    }

    private string CreateGameplayComment(
        CompletedProjectRecord project,
        int score)
    {
        if (project.bugs >= 5)
        {
            return
                "Bugs frequently interrupted the gameplay.";
        }

        if (project.marketBonus > 0f &&
            score >= 70)
        {
            return
                "The gameplay matched current player interests.";
        }

        if (project.fandomGained >= 15 &&
            score >= 70)
        {
            return
                "The gameplay quickly built a loyal following.";
        }

        if (score >= 70)
        {
            return
                "The gameplay was engaging and enjoyable.";
        }

        if (score >= 45)
        {
            return
                "The gameplay had potential but felt uneven.";
        }

        return
            "The gameplay needed more testing and refinement.";
    }

    private string CreateArtComment(
        CompletedProjectRecord project,
        int score)
    {
        if (project.quality >= 80f)
        {
            return
                "The art felt polished and carefully produced.";
        }

        if (project.workload >= 75f &&
            score < 70)
        {
            return
                "The art direction felt rushed in several areas.";
        }

        if (score >= 70)
        {
            return
                "The visual style created a strong atmosphere.";
        }

        if (score >= 45)
        {
            return
                "The art style was interesting but inconsistent.";
        }

        return
            "The visuals needed more time and improvement.";
    }

    private string CreateAudioComment(
        CompletedProjectRecord project,
        int score)
    {
        if (project.technicalDebt >= 20 &&
            score < 70)
        {
            return
                "Technical issues reduced the audio quality.";
        }

        if (score >= 70)
        {
            return
                "The audio supported the game very well.";
        }

        if (score >= 45)
        {
            return
                "The audio was acceptable but lacked variety.";
        }

        return
            "The audio felt repetitive and unfinished.";
    }

    /// Finds the two highest categories and lowest category for player feedback.
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

        // Do not show a disliked category when
        // every category received a positive score.
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

    /// Creates a test review when the Review scene is opened without selecting a project.
    public ReviewReportData CreateSampleReview()
    {
        // These lists now match the actual project
        // Theme and Genre choices.
        string[] themes =
        {
            "Fantasy",
            "SciFi",
            "Horror"
        };

        string[] genres =
        {
            "RPG",
            "Action",
            "Simulation"
        };

        ReviewReportData report =
            new ReviewReportData();

        report.theme =
            themes[Random.Range(0, themes.Length)];

        report.genre =
            genres[Random.Range(0, genres.Length)];

        string[] scopeSizes =
        {
            "Small",
            "Medium",
            "Large"
        };

        report.scope =
            scopeSizes[
                Random.Range(0, scopeSizes.Length)
                ];

        report.finalScore =
            Random.Range(40f, 96f);

        report.reviewScores =
            new int[3];

        report.reviewComments =
            new string[3];

        for (int i = 0;
             i < report.reviewScores.Length;
             i++)
        {
            report.reviewScores[i] =
                Mathf.Clamp(
                    Mathf.RoundToInt(
                        report.finalScore) +
                    Random.Range(-15, 16),
                    0,
                    100);

            report.reviewComments[i] =
                GetSampleComment(
                    i,
                    report.reviewScores[i]);
        }

        CreatePlayerFeedback(
            report,
            report.reviewScores);

        return report;
    }

    private string GetSampleComment(
        int reviewIndex,
        int score)
    {
        string[] positiveComments =
        {
            "The gameplay was really fun!",
            "Beautiful art and atmosphere.",
            "The audio worked really well."
        };

        string[] mixedComments =
        {
            "The gameplay was enjoyable.",
            "The art style was interesting.",
            "The audio was acceptable."
        };

        string[] negativeComments =
        {
            "The gameplay needs more variety.",
            "The art needs more improvement.",
            "The audio was repetitive."
        };

        if (score >= 70)
        {
            return positiveComments[reviewIndex];
        }

        if (score >= 45)
        {
            return mixedComments[reviewIndex];
        }

        return negativeComments[reviewIndex];
    }
}