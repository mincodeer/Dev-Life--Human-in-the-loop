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

    [Header("Market Trend UI")]
    [SerializeField] private TMP_Text[] themeDirectionTexts;
    [SerializeField] private TMP_Text[] genreDemandTexts;
    [SerializeField] private RectTransform[] genreDemandFills;
    [SerializeField] private float maximumBarWidth = 220f;

    [Header("Trend Duration")]
    [SerializeField] private int minimumWeeks = 2;
    [SerializeField] private int maximumWeeks = 5;

    [Header("Bonuses Per Match")]
    [SerializeField] private int qualityBonus = 5;
    [SerializeField] private float moneyBonusPercent = 0.10f;
    [SerializeField] private float fandomBonusPercent = 0.15f;

    public string CurrentTheme { get; private set; }
    public string CurrentGenre { get; private set; }
    public int WeeksRemaining { get; private set; }

    public event Action OnTrendChanged;

    private int[] themeDirections;
    private int[] genreDemands;

    private readonly Color risingColor = new Color32(53, 245, 107, 255);
    private readonly Color fallingColor = new Color32(255, 59, 69, 255);
    private readonly Color stableColor = new Color32(244, 237, 106, 255);

    private void Start()
    {
        GenerateNewTrend();
    }

    public void GenerateNewTrend()
    {
        GenerateThemeDirections();
        GenerateGenreDemand();

        WeeksRemaining =
            UnityEngine.Random.Range(minimumWeeks, maximumWeeks + 1);

        UpdateMarketTrendUI();

        Debug.Log(
            $"New Market Trend: {CurrentGenre}, " +
            $"{CurrentTheme}, {WeeksRemaining} weeks"
        );

        OnTrendChanged?.Invoke();
    }

    private void GenerateThemeDirections()
    {
        themeDirections = new int[themes.Length];

        int popularThemeIndex =
            UnityEngine.Random.Range(0, themes.Length);

        for (int i = 0; i < themeDirections.Length; i++)
        {
            // Creates -1, 0 or 1.
            themeDirections[i] = UnityEngine.Random.Range(-1, 2);
        }

        // Ensure the selected current theme is rising.
        themeDirections[popularThemeIndex] = 1;
        CurrentTheme = themes[popularThemeIndex];
    }

    private void GenerateGenreDemand()
    {
        genreDemands = new int[genres.Length];

        int highestDemand = -1;
        int highestDemandIndex = 0;

        for (int i = 0; i < genreDemands.Length; i++)
        {
            genreDemands[i] = UnityEngine.Random.Range(20, 91);

            if (genreDemands[i] > highestDemand)
            {
                highestDemand = genreDemands[i];
                highestDemandIndex = i;
            }
        }

        CurrentGenre = genres[highestDemandIndex];
    }

    private void UpdateMarketTrendUI()
    {
        for (int i = 0;
             i < themeDirectionTexts.Length &&
             i < themeDirections.Length;
             i++)
        {
            if (themeDirections[i] > 0)
            {
                themeDirectionTexts[i].text = "↑";
                themeDirectionTexts[i].color = risingColor;
            }
            else if (themeDirections[i] < 0)
            {
                themeDirectionTexts[i].text = "↓";
                themeDirectionTexts[i].color = fallingColor;
            }
            else
            {
                themeDirectionTexts[i].text = "-";
                themeDirectionTexts[i].color = stableColor;
            }
        }

        for (int i = 0;
             i < genreDemandTexts.Length &&
             i < genreDemands.Length;
             i++)
        {
            genreDemandTexts[i].text = genreDemands[i] + "%";
        }

        for (int i = 0;
             i < genreDemandFills.Length &&
             i < genreDemands.Length;
             i++)
        {
            float width =
                maximumBarWidth * genreDemands[i] / 100f;

            genreDemandFills[i].SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                width
            );
        }
    }

    public void AdvanceWeek()
    {
        WeeksRemaining--;

        if (WeeksRemaining <= 0)
        {
            GenerateNewTrend();
            return;
        }

        OnTrendChanged?.Invoke();
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
            matchCount++;

        if (themeMatches)
            matchCount++;

        return new MarketTrendResult
        {
            genreMatches = genreMatches,
            themeMatches = themeMatches,
            qualityBonus = qualityBonus * matchCount,
            moneyMultiplier = 1f + moneyBonusPercent * matchCount,
            fandomMultiplier = 1f + fandomBonusPercent * matchCount
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