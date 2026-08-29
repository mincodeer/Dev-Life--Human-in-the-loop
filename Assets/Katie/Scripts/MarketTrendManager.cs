using System;
using TMPro;
using UnityEngine;

public class MarketTrendManager : MonoBehaviour
{
    [Header("Possible Trends")]
    [SerializeField]
    private string[] themes =
    {
        "Fantasy", "Sci-Fi", "Modern", "Dystopia", "Pixel"
    };

    [SerializeField]
    private string[] genres =
    {
        "RPG", "Action", "Horror",
        "Adventure", "Simulation", "Strategy"
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

    [Header("Trend Duration")]
    [SerializeField] private int minimumWeeks = 1;
    [SerializeField] private int maximumWeeks = 4;

    [Header("Bonuses Per Match")]
    [SerializeField] private int qualityBonus = 5;
    [SerializeField] private float moneyBonusPercent = 0.10f;
    [SerializeField] private float fandomBonusPercent = 0.15f;

    public string CurrentTheme { get; private set; }
    public string CurrentGenre { get; private set; }
    public int WeeksRemaining { get; private set; }

    public event Action OnTrendChanged;

    private int[] themeDirections;
    private int[] themeDurations;
    private int[] genreDemands;
    private int[] genreDurations;

    private int currentThemeIndex;
    private int currentGenreIndex;

    // Trend direction and status colours.
    private readonly Color risingColor =
        new Color32(35, 247, 55, 255);       // #23F737

    private readonly Color fallingColor =
        new Color32(254, 12, 0, 255);        // #FE0C00

    private readonly Color stableColor =
        new Color32(254, 242, 0, 255);       // #FEF200

    // Duration colours.
    private readonly Color urgentDurationColor =
        new Color32(197, 80, 0, 255);        // #C55000

    private readonly Color warningDurationColor =
        new Color32(197, 154, 0, 255);       // #C59A00

    private readonly Color safeDurationColor =
        new Color32(0, 197, 21, 255);        // #00C515

    private void Start()
    {
        GenerateNewTrend();
    }

    public void GenerateNewTrend()
    {
        GenerateThemeTrends();
        GenerateGenreTrends();
        UpdateMarketTrendUI();

        Debug.Log(
            $"New Market Trend: {CurrentTheme} + {CurrentGenre}, " +
            $"{WeeksRemaining} weeks remaining"
        );

        OnTrendChanged?.Invoke();
    }

    private void GenerateThemeTrends()
    {
        themeDirections = new int[themes.Length];
        themeDurations = new int[themes.Length];

        currentThemeIndex =
            UnityEngine.Random.Range(0, themes.Length);

        for (int i = 0; i < themes.Length; i++)
        {
            themeDirections[i] =
                UnityEngine.Random.Range(-1, 2);

            themeDurations[i] = GenerateDuration();
        }

        themeDirections[currentThemeIndex] = 1;
        CurrentTheme = themes[currentThemeIndex];
    }

    private void GenerateGenreTrends()
    {
        genreDemands = new int[genres.Length];
        genreDurations = new int[genres.Length];

        int highestDemand = -1;
        currentGenreIndex = 0;

        for (int i = 0; i < genres.Length; i++)
        {
            genreDemands[i] =
                UnityEngine.Random.Range(20, 91);

            genreDurations[i] = GenerateDuration();

            if (genreDemands[i] > highestDemand)
            {
                highestDemand = genreDemands[i];
                currentGenreIndex = i;
            }
        }

        CurrentGenre = genres[currentGenreIndex];
    }

    private int GenerateDuration()
    {
        return UnityEngine.Random.Range(
            minimumWeeks,
            maximumWeeks + 1
        );
    }

    private void UpdateMarketTrendUI()
    {
        UpdateThemeUI();
        UpdateGenreUI();
        UpdateCurrentMarketSummary();
    }

    private void UpdateThemeUI()
    {
        for (int i = 0; i < themeDirections.Length; i++)
        {
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

            if (i < themeDirectionTexts.Length &&
                themeDirectionTexts[i] != null)
            {
                themeDirectionTexts[i].text = arrow;
                themeDirectionTexts[i].color = trendColor;
            }

            if (i < themeStatusTexts.Length &&
                themeStatusTexts[i] != null)
            {
                themeStatusTexts[i].text = status;
                themeStatusTexts[i].color = trendColor;
            }

            if (i < themeDurationTexts.Length &&
                themeDurationTexts[i] != null)
            {
                themeDurationTexts[i].text =
                    FormatRowDuration(themeDurations[i]);

                themeDurationTexts[i].color =
                    GetDurationColor(themeDurations[i]);
            }
        }
    }

    private void UpdateGenreUI()
    {
        for (int i = 0;
             i < genreDemandTexts.Length &&
             i < genreDemands.Length;
             i++)
        {
            if (genreDemandTexts[i] == null)
            {
                continue;
            }

            genreDemandTexts[i].text =
                genreDemands[i] + "%";
        }

        for (int i = 0;
             i < genreDemandFills.Length &&
             i < genreDemands.Length;
             i++)
        {
            if (genreDemandFills[i] == null)
            {
                continue;
            }

            float width =
                maximumBarWidth * genreDemands[i] / 100f;

            genreDemandFills[i].SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                width
            );
        }

        for (int i = 0;
             i < genreDurationTexts.Length &&
             i < genreDurations.Length;
             i++)
        {
            if (genreDurationTexts[i] == null)
            {
                continue;
            }

            genreDurationTexts[i].text =
                FormatRowDuration(genreDurations[i]);

            genreDurationTexts[i].color =
                GetDurationColor(genreDurations[i]);
        }
    }

    private void UpdateCurrentMarketSummary()
    {
        CurrentTheme = themes[currentThemeIndex];
        CurrentGenre = genres[currentGenreIndex];

        WeeksRemaining = Mathf.Min(
            themeDurations[currentThemeIndex],
            genreDurations[currentGenreIndex]
        );

        if (currentThemeText != null)
        {
            currentThemeText.text =
                CurrentTheme.ToUpper();

            if (themeNameTexts != null &&
                currentThemeIndex < themeNameTexts.Length &&
                themeNameTexts[currentThemeIndex] != null)
            {
                currentThemeText.color =
                    themeNameTexts[currentThemeIndex].color;
            }
        }

        if (currentGenreText != null)
        {
            currentGenreText.text =
                CurrentGenre.ToUpper();

            if (genreNameTexts != null &&
                currentGenreIndex < genreNameTexts.Length &&
                genreNameTexts[currentGenreIndex] != null)
            {
                currentGenreText.color =
                    genreNameTexts[currentGenreIndex].color;
            }
        }

        if (marketTimeLeftText != null)
        {
            marketTimeLeftText.text =
                FormatSummaryDuration(WeeksRemaining);

            marketTimeLeftText.color =
                GetDurationColor(WeeksRemaining);
        }
    }

    public void AdvanceWeek()
    {
        bool currentThemeExpired = false;

        for (int i = 0; i < themeDurations.Length; i++)
        {
            themeDurations[i]--;

            if (themeDurations[i] <= 0)
            {
                if (i == currentThemeIndex)
                {
                    currentThemeExpired = true;
                }

                themeDirections[i] =
                    UnityEngine.Random.Range(-1, 2);

                themeDurations[i] = GenerateDuration();
            }
        }

        for (int i = 0; i < genreDurations.Length; i++)
        {
            genreDurations[i]--;

            if (genreDurations[i] <= 0)
            {
                genreDemands[i] =
                    UnityEngine.Random.Range(20, 91);

                genreDurations[i] = GenerateDuration();
            }
        }

        if (currentThemeExpired)
        {
            currentThemeIndex =
                UnityEngine.Random.Range(0, themes.Length);

            themeDirections[currentThemeIndex] = 1;
        }

        FindHighestDemandGenre();
        UpdateMarketTrendUI();

        OnTrendChanged?.Invoke();
    }

    private void FindHighestDemandGenre()
    {
        int highestDemand = -1;
        currentGenreIndex = 0;

        for (int i = 0; i < genreDemands.Length; i++)
        {
            if (genreDemands[i] > highestDemand)
            {
                highestDemand = genreDemands[i];
                currentGenreIndex = i;
            }
        }
    }

    private string FormatRowDuration(int weeks)
    {
        if (weeks == 1)
        {
            return "1 WEEK LEFT";
        }

        return weeks + " WEEKS LEFT";
    }

    private string FormatSummaryDuration(int weeks)
    {
        if (weeks == 1)
        {
            return "1 WEEK";
        }

        return weeks + " WEEKS";
    }

    private Color GetDurationColor(int weeks)
    {
        if (weeks <= 1)
        {
            return urgentDurationColor;
        }

        if (weeks == 2)
        {
            return warningDurationColor;
        }

        return safeDurationColor;
    }

    public MarketTrendResult EvaluateProject(
        string projectGenre,
        string projectTheme)
    {
        bool genreMatches = string.Equals(
            projectGenre,
            CurrentGenre,
            StringComparison.OrdinalIgnoreCase
        );

        bool themeMatches = string.Equals(
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
            qualityBonus = qualityBonus * matchCount,
            moneyMultiplier =
                1f + moneyBonusPercent * matchCount,
            fandomMultiplier =
                1f + fandomBonusPercent * matchCount
        };
    }
}

[Serializable]
public class MarketTrendResult
{
    public bool genreMatches;
    public bool themeMatches;
    public int qualityBonus;
    public float moneyMultiplier = 1f;
    public float fandomMultiplier = 1f;
}