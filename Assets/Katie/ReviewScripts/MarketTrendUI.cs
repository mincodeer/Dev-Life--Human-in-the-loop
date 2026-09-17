using TMPro;
using UnityEngine;

public class MarketTrendUI : MonoBehaviour
{
    [Header("Market Trend Manager")]
    [SerializeField] private MarketTrendManager marketTrendManager;

    [Header("Theme UI")]
    [SerializeField] private TMP_Text[] themeNameTexts;
    [SerializeField] private TMP_Text[] themeDirectionTexts;
    [SerializeField] private TMP_Text[] themeStatusTexts;
    [SerializeField] private TMP_Text[] themeDurationTexts;

    [Header("Genre UI")]
    [SerializeField] private TMP_Text[] genreNameTexts;
    [SerializeField] private TMP_Text[] genreDemandTexts;
    [SerializeField] private RectTransform[] genreDemandFills;
    [SerializeField] private TMP_Text[] genreDurationTexts;
    [SerializeField] private float maximumBarWidth = 220f;

    [Header("Current Market Summary UI")]
    [SerializeField] private TMP_Text currentThemeText;
    [SerializeField] private TMP_Text currentGenreText;
    [SerializeField] private TMP_Text marketTimeLeftText;
    [SerializeField] private TMP_Text currentWeekText;

    // Colours used for rising, declining, and steady Theme directions.
    private readonly Color risingColor =
        new Color32(35, 247, 55, 255);

    private readonly Color fallingColor =
        new Color32(254, 12, 0, 255);

    private readonly Color stableColor =
        new Color32(254, 242, 0, 255);

    // Green is used when one to two weeks remain.
    private readonly Color weeksColor =
        new Color32(9, 196, 26, 255); // #09C41A

    // Yellow is used when two to six days remain.
    private readonly Color daysColor =
        new Color32(197, 154, 0, 255); // #C59A00

    // Dark orange is used when only one day remains.
    private readonly Color oneDayColor =
        new Color32(175, 71, 32, 255); // #AF4720

    // Dark red is used when fewer than 24 hours remain.
    private readonly Color hoursColor =
        new Color32(175, 34, 32, 255); // #AF2220

    private void OnEnable()
    {
        // Listen for updates from the Market Trend manager.
        if (marketTrendManager != null)
        {
            marketTrendManager.OnMarketUpdated += RefreshUI;
        }

        RefreshUI();
    }

    private void OnDisable()
    {
        // Stop listening when this UI becomes inactive.
        if (marketTrendManager != null)
        {
            marketTrendManager.OnMarketUpdated -= RefreshUI;
        }
    }

    // Refreshes every visible part of the Market Trends screen.
    public void RefreshUI()
    {
        if (marketTrendManager == null
            || !marketTrendManager.IsInitialized)
        {
            return;
        }

        UpdateThemeUI();
        UpdateGenreUI();
        UpdateSummaryUI();
        UpdateWeekUI();
    }

    // Updates the name, direction, status, and duration of every Theme.
    private void UpdateThemeUI()
    {
        for (int i = 0; i < marketTrendManager.ThemeCount; i++)
        {
            SetText(themeNameTexts, i,
                marketTrendManager.GetThemeName(i));

            int direction =
                marketTrendManager.GetThemeDirection(i);

            string arrow;
            string status;
            Color trendColor;

            if (direction > 0)
            {
                arrow = "\u2191";
                status = "RISING";
                trendColor = risingColor;
            }
            else if (direction < 0)
            {
                arrow = "\u2193";
                status = "DECLINING";
                trendColor = fallingColor;
            }
            else
            {
                arrow = "-";
                status = "STEADY";
                trendColor = stableColor;
            }

            SetTextAndColor(
                themeDirectionTexts,
                i,
                arrow,
                trendColor);

            SetTextAndColor(
                themeStatusTexts,
                i,
                status,
                trendColor);

            int remainingMinutes =
                marketTrendManager.GetThemeMinutesRemaining(i);

            SetTextAndColor(
                themeDurationTexts,
                i,
                FormatDuration(remainingMinutes, true),
                GetDurationColor(remainingMinutes));
        }
    }

    // Updates the name, demand bar, percentage, and duration of every Genre.
    private void UpdateGenreUI()
    {
        for (int i = 0; i < marketTrendManager.GenreCount; i++)
        {
            SetText(genreNameTexts, i,
                marketTrendManager.GetGenreName(i));

            int demand =
                marketTrendManager.GetGenreDemand(i);

            SetText(genreDemandTexts, i,
                demand + "%");

            if (genreDemandFills != null
                && i < genreDemandFills.Length
                && genreDemandFills[i] != null)
            {
                float width =
                    maximumBarWidth * demand / 100f;

                genreDemandFills[i].SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Horizontal,
                    width);
            }

            int remainingMinutes =
                marketTrendManager.GetGenreMinutesRemaining(i);

            SetTextAndColor(
                genreDurationTexts,
                i,
                FormatDuration(remainingMinutes, true),
                GetDurationColor(remainingMinutes));
        }
    }

    // Updates the selected Theme, Genre, and summary countdown.
    private void UpdateSummaryUI()
    {
        if (currentThemeText != null)
        {
            currentThemeText.text =
                marketTrendManager.CurrentTheme.ToUpper();

            // Match the summary colour to the selected Theme.
            currentThemeText.color =
                GetThemeColor(marketTrendManager.CurrentTheme);
        }

        if (currentGenreText != null)
        {
            currentGenreText.text =
                marketTrendManager.CurrentGenre.ToUpper();

            // Match the summary colour to the selected Genre.
            currentGenreText.color =
                GetGenreColor(marketTrendManager.CurrentGenre);
        }

        if (marketTimeLeftText != null)
        {
            int remainingMinutes =
                marketTrendManager.MinutesRemaining;

            marketTimeLeftText.text =
                FormatDuration(remainingMinutes, false);

            marketTimeLeftText.color =
                GetDurationColor(remainingMinutes);
        }
    }

    // Updates the optional current-week label.
    private void UpdateWeekUI()
    {
        if (currentWeekText != null)
        {
            currentWeekText.text =
                "WEEK " + marketTrendManager.CurrentWeek;
        }
    }

    // Formats a countdown as weeks, days, or hours.
    private string FormatDuration(
        int remainingMinutes,
        bool includeLeft)
    {
        string result;

        int minutesPerHour = 60;
        int minutesPerDay = 24 * minutesPerHour;
        int minutesPerWeek = 7 * minutesPerDay;

        if (remainingMinutes >= minutesPerWeek)
        {
            int weeks =
                Mathf.CeilToInt(
                    remainingMinutes / (float)minutesPerWeek);

            result = weeks == 1
                ? "1 WEEK"
                : weeks + " WEEKS";
        }
        else if (remainingMinutes >= 2 * minutesPerDay)
        {
            int days =
                Mathf.CeilToInt(
                    remainingMinutes / (float)minutesPerDay);

            result = days + " DAYS";
        }
        else if (remainingMinutes >= minutesPerDay)
        {
            result = "1 DAY";
        }
        else if (remainingMinutes >= minutesPerHour)
        {
            int hours = Mathf.CeilToInt(
                remainingMinutes / (float)minutesPerHour);

            result = hours == 1
                ? "1 HOUR"
                : hours + " HOURS";
        }
        else
        {
            int minutes = Mathf.Max(1, remainingMinutes);

            result = minutes == 1
                ? "1 MINUTE"
                : minutes + " MINUTES";
        }

        if (includeLeft)
        {
            result += " LEFT";
        }

        return result;
    }

    // Chooses a countdown colour based on the remaining minutes.
    private Color GetDurationColor(int remainingMinutes)
    {
        int minutesPerHour = 60;
        int minutesPerDay = 24 * minutesPerHour;
        int minutesPerWeek = 7 * minutesPerDay;

        if (remainingMinutes < minutesPerDay)
        {
            return hoursColor;
        }

        if (remainingMinutes < 2 * minutesPerDay)
        {
            return oneDayColor;
        }

        if (remainingMinutes < minutesPerWeek)
        {
            return daysColor;
        }

        return weeksColor;
    }

    // Returns the correct colour for the selected Theme.
    private Color GetThemeColor(string themeName)
    {
        switch (themeName)
        {
            case "Fantasy":
                return new Color32(255, 186, 254, 255); // #FFBAFE

            case "Sci-Fi":
                return new Color32(186, 255, 253, 255); // #BAFFFD

            case "Horror":
                return new Color32(191, 107, 255, 255); // #BF6BFF

            default:
                return Color.white;
        }
    }

    // Returns the correct colour for the selected Genre.
    private Color GetGenreColor(string genreName)
    {
        switch (genreName)
        {
            case "RPG":
                return new Color32(107, 204, 255, 255); // #6BCCFF

            case "Action":
                return new Color32(255, 107, 117, 255); // #FF6B75

            case "Simulation":
                return new Color32(255, 208, 107, 255); // #FFD06B

            default:
                return Color.white;
        }
    }

    // Safely changes one text element in an Inspector array.
    private void SetText(
        TMP_Text[] textArray,
        int index,
        string value)
    {
        if (textArray != null
            && index < textArray.Length
            && textArray[index] != null)
        {
            textArray[index].text = value;
        }
    }

    // Safely changes one text element and its colour.
    private void SetTextAndColor(
        TMP_Text[] textArray,
        int index,
        string value,
        Color color)
    {
        if (textArray != null
            && index < textArray.Length
            && textArray[index] != null)
        {
            textArray[index].text = value;
            textArray[index].color = color;
        }
    }
}
