using System;
using TMPro;
using UnityEngine;

public class MarketTrendManager : MonoBehaviour
{
    // The Themes that can appear in the market.
    private readonly string[] themes =
    {
        "Fantasy",
        "Sci-Fi",
        "Horror"
    };

    // The Genres that can appear in the market.
    private readonly string[] genres =
    {
        "RPG",
        "Action",
        "Simulation"
    };

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

    [Header("Trend Duration")]
    [SerializeField] private int minimumWeeks = 1;
    [SerializeField] private int maximumWeeks = 4;
    [SerializeField] private int daysPerWeek = 7;

    [Header("Bonuses Per Match")]
    [SerializeField] private int qualityBonus = 5;
    [SerializeField] private float moneyBonusPercent = 0.10f;
    [SerializeField] private float fandomBonusPercent = 0.15f;

    // The current Theme and Genre shown in the market summary.
    // Other systems can read these values but cannot change them.
    public string CurrentTheme { get; private set; }
    public string CurrentGenre { get; private set; }

    // The shorter remaining time from the summary Theme and Genre.
    public int DaysRemaining { get; private set; }

    public int WeeksRemaining
    {
        get
        {
            return Mathf.CeilToInt(
                DaysRemaining / (float)daysPerWeek
            );
        }
    }

    // Keeps track of how many days have passed in this manager.
    public int CurrentDay { get; private set; }

    public int CurrentWeek
    {
        get
        {
            return (CurrentDay / daysPerWeek) + 1;
        }
    }

    // Tells other systems when any market trend changes.
    public event Action OnTrendChanged;

    // Stores the direction and remaining duration of every Theme.
    private int[] themeDirections;
    private int[] themeDurationDays;

    // Stores the demand and remaining duration of every Genre.
    private int[] genreDemands;
    private int[] genreDurationDays;

    // Stores which Theme and Genre are currently used in the summary.
    private int currentThemeIndex;
    private int currentGenreIndex;

    private readonly Color risingColor =
        new Color32(35, 247, 55, 255);

    private readonly Color fallingColor =
        new Color32(254, 12, 0, 255);

    private readonly Color stableColor =
        new Color32(254, 242, 0, 255);

    // Required colour for durations below one week.
    private readonly Color daysLeftColor =
        new Color32(197, 0, 3, 255); // #C50003

    private readonly Color warningDurationColor =
        new Color32(197, 154, 0, 255);

    private readonly Color safeDurationColor =
        new Color32(0, 197, 21, 255);

    private void Start()
    {
        // Create the first set of market trends when the game begins.
        CurrentDay = 0;
        GenerateNewTrend();
    }

    public void GenerateNewTrend()
    {
        // Generate all Theme and Genre information and refresh the UI.
        GenerateThemeTrends();
        GenerateGenreTrends();
        UpdateMarketTrendUI();

        Debug.Log(
            "New Market Trend: "
            + CurrentTheme
            + " + "
            + CurrentGenre
            + ", "
            + DaysRemaining
            + " days remaining."
        );

        OnTrendChanged?.Invoke();
    }

    private void GenerateThemeTrends()
    {
        // Create arrays with one entry for every available Theme.
        themeDirections =
            new int[themes.Length];

        themeDurationDays =
            new int[themes.Length];

        currentThemeIndex =
            UnityEngine.Random.Range(
                0,
                themes.Length
            );

        for (int i = 0;
             i < themes.Length;
             i++)
        {
            // Choose whether this Theme is rising, steady, or declining.
            themeDirections[i] =
                UnityEngine.Random.Range(-1, 2);

            // Give this Theme its own random duration.
            themeDurationDays[i] =
                GenerateDurationDays();
        }

        themeDirections[currentThemeIndex] = 1;
        CurrentTheme = themes[currentThemeIndex];
    }

    private void GenerateGenreTrends()
    {
        // Create arrays with one entry for every available Genre.
        genreDemands =
            new int[genres.Length];

        genreDurationDays =
            new int[genres.Length];

        int highestDemand = -1;
        currentGenreIndex = 0;

        for (int i = 0;
             i < genres.Length;
             i++)
        {
            // Give this Genre a random demand percentage and duration.
            genreDemands[i] =
                UnityEngine.Random.Range(20, 91);

            genreDurationDays[i] =
                GenerateDurationDays();

            if (genreDemands[i] > highestDemand)
            {
                highestDemand = genreDemands[i];
                currentGenreIndex = i;
            }
        }

        CurrentGenre =
            genres[currentGenreIndex];
    }

    private int GenerateDurationDays()
    {
        // Choose a random number of weeks and convert it into days.
        int durationWeeks =
            UnityEngine.Random.Range(
                minimumWeeks,
                maximumWeeks + 1
            );

        return durationWeeks * daysPerWeek;
    }

    public void AdvanceDay()
    {
        // Generate trends first if the arrays have not been created yet.
        if (!TrendsAreInitialized())
        {
            GenerateNewTrend();
            return;
        }

        // Move the market forward by one in-game day.
        CurrentDay++;

        bool anyTrendChanged = false;

        for (int i = 0;
             i < themeDurationDays.Length;
             i++)
        {
            // Reduce this Theme's remaining time by one day.
            themeDurationDays[i]--;

            if (themeDurationDays[i] <= 0)
            {
                // Only this expired Theme changes.
                themeDirections[i] =
                    UnityEngine.Random.Range(-1, 2);

                themeDurationDays[i] =
                    GenerateDurationDays();

                anyTrendChanged = true;

                Debug.Log(
                    themes[i]
                    + " Theme trend changed."
                );
            }
        }

        for (int i = 0;
             i < genreDurationDays.Length;
             i++)
        {
            // Reduce this Genre's remaining time by one day.
            genreDurationDays[i]--;

            if (genreDurationDays[i] <= 0)
            {
                // Only this expired Genre changes.
                genreDemands[i] =
                    UnityEngine.Random.Range(20, 91);

                genreDurationDays[i] =
                    GenerateDurationDays();

                anyTrendChanged = true;

                Debug.Log(
                    genres[i]
                    + " Genre demand changed."
                );
            }
        }

        if (anyTrendChanged)
        {
            // Notify other systems only when at least one row changed.
            OnTrendChanged?.Invoke();
        }

        UpdateMarketTrendUI();

        Debug.Log(
            "One day passed. Current day: "
            + CurrentDay
            + ", current week: "
            + CurrentWeek
        );
    }

    public void AdvanceWeek()
    {
        // A week is handled as seven separate day changes.
        // This allows each row to expire on the correct day.
        for (int i = 0;
             i < daysPerWeek;
             i++)
        {
            AdvanceDay();
        }
    }

    private bool TrendsAreInitialized()
    {
        // All four data arrays must exist before they can be updated.
        return themeDirections != null
            && themeDurationDays != null
            && genreDemands != null
            && genreDurationDays != null;
    }

    private void UpdateMarketTrendUI()
    {
        // Refresh every visible section of the Market Trends screen.
        UpdateThemeUI();
        UpdateGenreUI();
        UpdateCurrentMarketSummary();
        UpdateWeekUI();
    }

    private void UpdateThemeUI()
    {
        // Update the name, direction, status, duration, and colour of each Theme.
        for (int i = 0;
             i < themes.Length;
             i++)
        {
            if (themeNameTexts != null &&
                i < themeNameTexts.Length &&
                themeNameTexts[i] != null)
            {
                themeNameTexts[i].text =
                    themes[i];
            }

            string arrow;
            string status;
            Color trendColor;

            if (themeDirections[i] > 0)
            {
                arrow = "↑";
                status = "RISING";
                trendColor = risingColor;
            }
            else if (themeDirections[i] < 0)
            {
                arrow = "↓";
                status = "DECLINING";
                trendColor = fallingColor;
            }
            else
            {
                arrow = "-";
                status = "STEADY";
                trendColor = stableColor;
            }

            if (themeDirectionTexts != null &&
                i < themeDirectionTexts.Length &&
                themeDirectionTexts[i] != null)
            {
                themeDirectionTexts[i].text =
                    arrow;

                themeDirectionTexts[i].color =
                    trendColor;
            }

            if (themeStatusTexts != null &&
                i < themeStatusTexts.Length &&
                themeStatusTexts[i] != null)
            {
                themeStatusTexts[i].text =
                    status;

                themeStatusTexts[i].color =
                    trendColor;
            }

            if (themeDurationTexts != null &&
                i < themeDurationTexts.Length &&
                themeDurationTexts[i] != null)
            {
                int remainingDays =
                    themeDurationDays[i];

                themeDurationTexts[i].text =
                    FormatDuration(remainingDays);

                themeDurationTexts[i].color =
                    GetDurationColor(remainingDays);
            }
        }
    }

    private void UpdateGenreUI()
    {
        // Update the name, demand bar, percentage, and duration of each Genre.
        for (int i = 0;
             i < genres.Length;
             i++)
        {
            if (genreNameTexts != null &&
                i < genreNameTexts.Length &&
                genreNameTexts[i] != null)
            {
                genreNameTexts[i].text =
                    genres[i];
            }

            if (genreDemandTexts != null &&
                i < genreDemandTexts.Length &&
                genreDemandTexts[i] != null)
            {
                genreDemandTexts[i].text =
                    genreDemands[i] + "%";
            }

            if (genreDemandFills != null &&
                i < genreDemandFills.Length &&
                genreDemandFills[i] != null)
            {
                float width =
                    maximumBarWidth
                    * genreDemands[i]
                    / 100f;

                genreDemandFills[i]
                    .SetSizeWithCurrentAnchors(
                        RectTransform.Axis.Horizontal,
                        width
                    );
            }

            if (genreDurationTexts != null &&
                i < genreDurationTexts.Length &&
                genreDurationTexts[i] != null)
            {
                int remainingDays =
                    genreDurationDays[i];

                genreDurationTexts[i].text =
                    FormatDuration(remainingDays);

                genreDurationTexts[i].color =
                    GetDurationColor(remainingDays);
            }
        }
    }

    private void UpdateCurrentMarketSummary()
    {
        // The summary always uses the best current Theme and
        // the Genre with the highest current demand.
        FindBestTheme();
        FindHighestDemandGenre();

        CurrentTheme =
            themes[currentThemeIndex];

        CurrentGenre =
            genres[currentGenreIndex];

        // The summary changes when the earlier of its two trends expires.
        DaysRemaining = Mathf.Min(
            themeDurationDays[currentThemeIndex],
            genreDurationDays[currentGenreIndex]
        );

        if (currentThemeText != null)
        {
            currentThemeText.text =
                CurrentTheme.ToUpper();

            if (themeNameTexts != null &&
                currentThemeIndex <
                themeNameTexts.Length &&
                themeNameTexts[currentThemeIndex] != null)
            {
                currentThemeText.color =
                    themeNameTexts[
                        currentThemeIndex
                    ].color;
            }
        }

        if (currentGenreText != null)
        {
            currentGenreText.text =
                CurrentGenre.ToUpper();

            if (genreNameTexts != null &&
                currentGenreIndex <
                genreNameTexts.Length &&
                genreNameTexts[currentGenreIndex] != null)
            {
                currentGenreText.color =
                    genreNameTexts[
                        currentGenreIndex
                    ].color;
            }
        }

        if (marketTimeLeftText != null)
        {
            marketTimeLeftText.text =
                FormatSummaryDuration(
                    DaysRemaining
                );

            marketTimeLeftText.color =
                GetDurationColor(
                    DaysRemaining
                );
        }
    }

    private void FindBestTheme()
    {
        // Rising is preferred over steady, and steady over declining.
        int bestDirection = -2;
        currentThemeIndex = 0;

        for (int i = 0;
             i < themeDirections.Length;
             i++)
        {
            if (themeDirections[i] > bestDirection)
            {
                bestDirection = themeDirections[i];
                currentThemeIndex = i;
            }
        }
    }

    private void FindHighestDemandGenre()
    {
        // Find the Genre with the largest demand percentage.
        int highestDemand = -1;
        currentGenreIndex = 0;

        for (int i = 0;
             i < genreDemands.Length;
             i++)
        {
            if (genreDemands[i] > highestDemand)
            {
                highestDemand = genreDemands[i];
                currentGenreIndex = i;
            }
        }
    }

    private void UpdateWeekUI()
    {
        // The week label is optional, so do nothing when it is unassigned.
        if (currentWeekText == null)
        {
            return;
        }

        currentWeekText.text =
            "WEEK " + CurrentWeek;
    }

    private string FormatDuration(
        int remainingDays)
    {
        // Show days when fewer than seven days remain.
        if (remainingDays < daysPerWeek)
        {
            if (remainingDays == 1)
            {
                return "1 DAY LEFT";
            }

            return remainingDays
                + " DAYS LEFT";
        }

        // Otherwise, display the remaining time in weeks.
        int remainingWeeks =
            Mathf.CeilToInt(
                remainingDays
                / (float)daysPerWeek
            );

        if (remainingWeeks == 1)
        {
            return "1 WEEK LEFT";
        }

        return remainingWeeks
            + " WEEKS LEFT";
    }

    private string FormatSummaryDuration(
        int remainingDays)
    {
        // The summary uses the same countdown without the word LEFT.
        if (remainingDays < daysPerWeek)
        {
            if (remainingDays == 1)
            {
                return "1 DAY";
            }

            return remainingDays + " DAYS";
        }

        int remainingWeeks =
            Mathf.CeilToInt(
                remainingDays
                / (float)daysPerWeek
            );

        if (remainingWeeks == 1)
        {
            return "1 WEEK";
        }

        return remainingWeeks + " WEEKS";
    }

    private Color GetDurationColor(
        int remainingDays)
    {
        // Use red for the final days, yellow for one week, and green otherwise.
        if (remainingDays < daysPerWeek)
        {
            return daysLeftColor;
        }

        if (remainingDays <
            daysPerWeek * 2)
        {
            return warningDurationColor;
        }

        return safeDurationColor;
    }

    public MarketTrendResult EvaluateProject(
        string projectGenre,
        string projectTheme)
    {
        // Compare the project's selections with the current market summary.
        bool genreMatches =
            string.Equals(
                projectGenre,
                CurrentGenre,
                StringComparison.OrdinalIgnoreCase
            );

        bool themeMatches =
            string.Equals(
                projectTheme,
                CurrentTheme,
                StringComparison.OrdinalIgnoreCase
            );

        int matchCount = 0;

        if (genreMatches)
        {
            matchCount++;
        }

        if (themeMatches)
        {
            matchCount++;
        }

        return new MarketTrendResult
        {
            genreMatches = genreMatches,
            themeMatches = themeMatches,
            qualityBonus =
                qualityBonus * matchCount,

            moneyMultiplier =
                1f
                + moneyBonusPercent
                * matchCount,

            fandomMultiplier =
                1f
                + fandomBonusPercent
                * matchCount
        };
    }

#if UNITY_EDITOR

    [ContextMenu("TEST - Advance 1 Day")]
    private void TestAdvanceOneDay()
    {
        // Context-menu tests are only available while the game is running.
        if (!Application.isPlaying)
        {
            Debug.LogWarning(
                "Enter Play mode before testing."
            );

            return;
        }

        AdvanceDay();
    }

    [ContextMenu("TEST - Advance 1 Week")]
    private void TestAdvanceOneWeek()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning(
                "Enter Play mode before testing."
            );

            return;
        }

        AdvanceWeek();
    }

    [ContextMenu("TEST - Generate New Trend")]
    private void TestGenerateNewTrend()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning(
                "Enter Play mode before testing."
            );

            return;
        }

        GenerateNewTrend();
    }

#endif
}

[Serializable]
public class MarketTrendResult
{
    // Stores the market-match information returned for a completed project.
    public bool genreMatches;
    public bool themeMatches;
    public int qualityBonus;
    public float moneyMultiplier = 1f;
    public float fandomMultiplier = 1f;
}
