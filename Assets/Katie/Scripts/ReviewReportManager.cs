using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReviewReportManager : MonoBehaviour
{
    [Serializable]
    private class ReviewRowUI
    {
        public TMP_Text commentText;
        public Image[] stars;
    }

    [Header("Project Summary")]
    [SerializeField] private TMP_Text themeText;
    [SerializeField] private TMP_Text genreText;
    [SerializeField] private TMP_Text scopeText;

    [Header("Player Review Rows")]
    [SerializeField] private ReviewRowUI[] reviewRows;

    [Header("Final Report")]
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text verdictText;
    [SerializeField] private Image[] ratingStars;

    [Header("Star Colours")]
    [SerializeField]
    private Color filledStarColor =
        Color.white;

    [SerializeField]
    private Color emptyStarColor =
        new Color32(43, 48, 72, 255);

    [Header("Player Feedback")]
    [SerializeField] private TMP_Text playerLikedText;
    [SerializeField] private TMP_Text playerDislikedText;

    private readonly int[] reviewScores =
        new int[3];

    private readonly string[] categoryNames =
    {
        "GAMEPLAY",
        "ART STYLE",
        "AUDIO"
    };

    private readonly string[] positiveComments =
    {
        "The gameplay was really fun!",
        "Beautiful art and atmosphere.",
        "The audio worked really well."
    };

    private readonly string[] mixedComments =
    {
        "The gameplay was enjoyable.",
        "The art style was interesting.",
        "The audio was acceptable."
    };

    private readonly string[] negativeComments =
    {
        "The gameplay needs more variety.",
        "The art needs more improvement.",
        "The audio was repetitive."
    };

    private void Start()
    {
        if (ProjectHistoryManager.Instance != null &&
            ProjectHistoryManager.Instance.SelectedProject != null)
        {
            DisplaySelectedProject(
                ProjectHistoryManager.Instance.SelectedProject
            );
        }
        else
        {
            Debug.LogWarning(
                "No project was selected. " +
                "Showing a sample review."
            );

            GenerateSampleReview();
        }
    }

    private void DisplaySelectedProject(
        CompletedProjectRecord project)
    {
        UpdateProjectInformation(project);

        GenerateReviewScores(
            project.finalScore
        );

        UpdateReviewRows();

        UpdateFinalReport(
            project.finalScore
        );

        UpdatePlayerFeedback();
    }

    private void UpdateProjectInformation(
        CompletedProjectRecord project)
    {
        string theme =
            project.theme.ToString();

        string genre =
            project.genre.ToString();

        if (themeText != null)
        {
            themeText.text =
                theme.ToUpper();

            themeText.color =
                GetThemeColor(theme);
        }

        if (genreText != null)
        {
            genreText.text =
                genre.ToUpper();

            genreText.color =
                GetGenreColor(genre);
        }

        // Scope is not currently stored in
        // CompletedProjectRecord.
        if (scopeText != null)
        {
            scopeText.text = "N/A";

            scopeText.color =
                new Color32(190, 168, 81, 255);
        }
    }

    private void GenerateReviewScores(
        float finalScore)
    {
        for (int i = 0;
             i < reviewScores.Length;
             i++)
        {
            reviewScores[i] = Mathf.Clamp(
                Mathf.RoundToInt(finalScore) +
                UnityEngine.Random.Range(-15, 16),
                0,
                100
            );
        }
    }

    private void UpdateReviewRows()
    {
        if (reviewRows == null)
        {
            return;
        }

        int rowCount = Mathf.Min(
            reviewRows.Length,
            reviewScores.Length
        );

        for (int i = 0;
             i < rowCount;
             i++)
        {
            ReviewRowUI row =
                reviewRows[i];

            if (row == null)
            {
                continue;
            }

            int starRating =
                ConvertScoreToStars(
                    reviewScores[i]
                );

            UpdateStarImages(
                row.stars,
                starRating
            );

            if (row.commentText != null)
            {
                row.commentText.text =
                    GetReviewComment(
                        i,
                        reviewScores[i]
                    );
            }
        }
    }

    private string GetReviewComment(
        int reviewIndex,
        int score)
    {
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

    private void UpdateFinalReport(
        float finalScore)
    {
        int starRating =
            ConvertScoreToStars(finalScore);

        if (finalScoreText != null)
        {
            finalScoreText.text =
                starRating + " / 5";
        }

        UpdateStarImages(
            ratingStars,
            starRating
        );

        UpdateVerdict(finalScore);
    }

    private int ConvertScoreToStars(
        float score)
    {
        return Mathf.Clamp(
            Mathf.RoundToInt(score / 20f),
            1,
            5
        );
    }

    private void UpdateStarImages(
        Image[] stars,
        int starRating)
    {
        if (stars == null)
        {
            return;
        }

        for (int i = 0;
             i < stars.Length;
             i++)
        {
            if (stars[i] == null)
            {
                continue;
            }

            stars[i].color =
                i < starRating
                ? filledStarColor
                : emptyStarColor;
        }
    }

    private void UpdateVerdict(
        float score)
    {
        if (verdictText == null)
        {
            return;
        }

        if (score >= 85f)
        {
            verdictText.text =
                "EXCELLENT";

            verdictText.color =
                new Color32(35, 247, 55, 255);
        }
        else if (score >= 70f)
        {
            verdictText.text =
                "VERY POSITIVE";

            verdictText.color =
                new Color32(88, 245, 203, 255);
        }
        else if (score >= 55f)
        {
            verdictText.text =
                "POSITIVE";

            verdictText.color =
                new Color32(147, 255, 107, 255);
        }
        else if (score >= 40f)
        {
            verdictText.text =
                "MIXED";

            verdictText.color =
                new Color32(254, 242, 0, 255);
        }
        else
        {
            verdictText.text =
                "NEGATIVE";

            verdictText.color =
                new Color32(254, 80, 80, 255);
        }
    }

    private void UpdatePlayerFeedback()
    {
        int highestIndex = 0;
        int secondHighestIndex = 1;
        int lowestIndex = 0;

        for (int i = 1;
             i < reviewScores.Length;
             i++)
        {
            if (reviewScores[i] >
                reviewScores[highestIndex])
            {
                highestIndex = i;
            }

            if (reviewScores[i] <
                reviewScores[lowestIndex])
            {
                lowestIndex = i;
            }
        }

        if (secondHighestIndex ==
            highestIndex)
        {
            secondHighestIndex = 0;
        }

        for (int i = 0;
             i < reviewScores.Length;
             i++)
        {
            if (i == highestIndex)
            {
                continue;
            }

            if (reviewScores[i] >
                reviewScores[secondHighestIndex])
            {
                secondHighestIndex = i;
            }
        }

        if (playerLikedText != null)
        {
            playerLikedText.text =
                categoryNames[highestIndex] +
                " - " +
                categoryNames[secondHighestIndex];
        }

        if (playerDislikedText != null)
        {
            playerDislikedText.text =
                categoryNames[lowestIndex];
        }
    }

    public void GenerateSampleReview()
    {
        GenerateSampleProjectInformation();

        float sampleFinalScore =
            UnityEngine.Random.Range(40f, 96f);

        GenerateReviewScores(
            sampleFinalScore
        );

        UpdateReviewRows();

        UpdateFinalReport(
            sampleFinalScore
        );

        UpdatePlayerFeedback();
    }

    private void GenerateSampleProjectInformation()
    {
        string[] themes =
        {
            "Fantasy",
            "Sci-Fi",
            "Modern",
            "Dystopia",
            "Pixel"
        };

        string[] genres =
        {
            "RPG",
            "Action",
            "Horror",
            "Adventure",
            "Simulation",
            "Strategy"
        };

        string[] scopes =
        {
            "Large",
            "Medium",
            "Small"
        };

        string theme =
            themes[
                UnityEngine.Random.Range(
                    0,
                    themes.Length
                )
            ];

        string genre =
            genres[
                UnityEngine.Random.Range(
                    0,
                    genres.Length
                )
            ];

        string scope =
            scopes[
                UnityEngine.Random.Range(
                    0,
                    scopes.Length
                )
            ];

        if (themeText != null)
        {
            themeText.text =
                theme.ToUpper();

            themeText.color =
                GetThemeColor(theme);
        }

        if (genreText != null)
        {
            genreText.text =
                genre.ToUpper();

            genreText.color =
                GetGenreColor(genre);
        }

        if (scopeText != null)
        {
            scopeText.text =
                scope.ToUpper();

            scopeText.color =
                GetScopeColor(scope);
        }
    }

    private Color GetThemeColor(
        string theme)
    {
        switch (theme.ToUpper())
        {
            case "FANTASY":
                return new Color32(
                    255, 186, 254, 255);

            case "SCI-FI":
            case "SCIFI":
                return new Color32(
                    186, 255, 253, 255);

            case "MODERN":
                return new Color32(
                    250, 255, 186, 255);

            case "DYSTOPIA":
                return new Color32(
                    255, 214, 186, 255);

            case "PIXEL":
                return new Color32(
                    186, 255, 187, 255);

            default:
                return Color.white;
        }
    }

    private Color GetGenreColor(
        string genre)
    {
        switch (genre.ToUpper())
        {
            case "RPG":
                return new Color32(
                    107, 204, 255, 255);

            case "ACTION":
                return new Color32(
                    255, 107, 117, 255);

            case "HORROR":
                return new Color32(
                    191, 107, 255, 255);

            case "ADVENTURE":
                return new Color32(
                    147, 255, 107, 255);

            case "SIMULATION":
                return new Color32(
                    255, 208, 107, 255);

            case "STRATEGY":
                return new Color32(
                    255, 107, 217, 255);

            default:
                return Color.white;
        }
    }

    private Color GetScopeColor(
        string scope)
    {
        switch (scope.ToUpper())
        {
            case "LARGE":
                return new Color32(
                    190, 81, 104, 255);

            case "MEDIUM":
                return new Color32(
                    190, 119, 81, 255);

            case "SMALL":
                return new Color32(
                    190, 168, 81, 255);

            default:
                return Color.white;
        }
    }

#if UNITY_EDITOR

    [ContextMenu("TEST - Generate Sample Review")]
    private void TestGenerateSampleReview()
    {
        GenerateSampleReview();
    }

#endif
}