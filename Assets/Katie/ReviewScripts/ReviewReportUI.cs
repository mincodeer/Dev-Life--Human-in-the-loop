using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// Displays review information created by ReviewReportManager.
public class ReviewReportUI : MonoBehaviour
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

    /// Updates every visible part of the report.
    public void DisplayReport(
        ReviewReportData report)
    {
        if (report == null)
        {
            Debug.LogWarning(
                "ReviewReportData is missing.");

            return;
        }

        UpdateProjectInformation(report);
        UpdateReviewRows(report);
        UpdateFinalReport(report.finalScore);

        if (playerLikedText != null)
        {
            playerLikedText.text =
                report.playerLiked;
        }

        if (playerDislikedText != null)
        {
            playerDislikedText.text =
                report.playerDisliked;
            // "Nothing" is positive, so display it
            // using the Review screen's teal color.
            if (report.playerDisliked == "NOTHING")
            {
                playerDislikedText.color =
                    new Color32(88, 245, 203, 255);
            }
            else
            {
                playerDislikedText.color =
                    new Color32(255, 255, 255, 255);
            }
        }
    }

    private void UpdateProjectInformation(
        ReviewReportData report)
    {
        if (themeText != null)
        {
            themeText.text =
                report.theme.ToUpperInvariant();

            themeText.color =
                GetThemeColor(report.theme);
        }

        if (genreText != null)
        {
            genreText.text =
                report.genre.ToUpperInvariant();

            genreText.color =
                GetGenreColor(report.genre);
        }

        if (scopeText != null)
        {
            scopeText.text =
                report.scope.ToUpperInvariant();

            scopeText.color =
                GetScopeColor(report.scope);
        }
    }

    private void UpdateReviewRows(
        ReviewReportData report)
    {
        if (reviewRows == null ||
            report.reviewScores == null ||
            report.reviewComments == null)
        {
            return;
        }

        int rowCount =
            Mathf.Min(
                reviewRows.Length,
                report.reviewScores.Length,
                report.reviewComments.Length);

        for (int i = 0; i < rowCount; i++)
        {
            ReviewRowUI row = reviewRows[i];

            if (row == null)
            {
                continue;
            }

            int starRating =
                ConvertScoreToStars(
                    report.reviewScores[i]);

            UpdateStarImages(
                row.stars,
                starRating);

            if (row.commentText != null)
            {
                row.commentText.text =
                    report.reviewComments[i];
            }
        }
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
            starRating);

        UpdateVerdict(finalScore);
    }

    private int ConvertScoreToStars(
        float score)
    {
        return Mathf.Clamp(
            Mathf.RoundToInt(score / 20f),
            1,
            5);
    }

    private void UpdateStarImages(
        Image[] stars,
        int starRating)
    {
        if (stars == null)
        {
            return;
        }

        for (int i = 0; i < stars.Length; i++)
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
            verdictText.text = "EXCELLENT";
            verdictText.color =
                new Color32(35, 247, 55, 255);
        }
        else if (score >= 70f)
        {
            verdictText.text = "VERY POSITIVE";
            verdictText.color =
                new Color32(88, 245, 203, 255);
        }
        else if (score >= 55f)
        {
            verdictText.text = "POSITIVE";
            verdictText.color =
                new Color32(147, 255, 107, 255);
        }
        else if (score >= 40f)
        {
            verdictText.text = "MIXED";
            verdictText.color =
                new Color32(254, 242, 0, 255);
        }
        else
        {
            verdictText.text = "NEGATIVE";
            verdictText.color =
                new Color32(254, 80, 80, 255);
        }
    }

    private Color GetThemeColor(
        string theme)
    {
        switch (theme.ToUpperInvariant())
        {
            case "FANTASY":
                return new Color32(
                    255, 186, 254, 255);

            case "SCIFI":
            case "SCI-FI":
            case "SCIFI ":
                return new Color32(
                    186, 255, 253, 255);

            case "HORROR":
                return new Color32(
                    210, 150, 255, 255);

            default:
                return Color.white;
        }
    }

    private Color GetGenreColor(
        string genre)
    {
        switch (genre.ToUpperInvariant())
        {
            case "RPG":
                return new Color32(
                    107, 204, 255, 255);

            case "ACTION":
                return new Color32(
                    255, 107, 117, 255);

            case "SIMULATION":
                return new Color32(
                    255, 208, 107, 255);

            default:
                return Color.white;
        }
    }

    private Color GetScopeColor(
    string scope)
    {
        switch (scope.ToUpperInvariant())
        {
            case "SMALL":
                return new Color32(
                    147, 255, 107, 255);

            case "MEDIUM":
                return new Color32(
                    254, 242, 0, 255);

            case "LARGE":
                return new Color32(
                    255, 107, 117, 255);

            default:
                return Color.white;
        }
    }
}