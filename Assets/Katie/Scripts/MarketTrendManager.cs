using System;
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

    private void Start()
    {
        GenerateNewTrend();
    }

    public void GenerateNewTrend()
    {
        CurrentTheme = themes[UnityEngine.Random.Range(0, themes.Length)];
        CurrentGenre = genres[UnityEngine.Random.Range(0, genres.Length)];

        WeeksRemaining =
            UnityEngine.Random.Range(minimumWeeks, maximumWeeks + 1);

        Debug.Log(
            $"New Market Trend: {CurrentGenre}, " +
            $"{CurrentTheme}, {WeeksRemaining} weeks"
        );

        OnTrendChanged?.Invoke();
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