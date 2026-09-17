using System;
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

    [Header("Time Tracker")]
    [SerializeField] private GameTimeManager gameTimeManager;

    [Header("Trend Duration")]
    [SerializeField] private int minimumTrendDays = 5;
    [SerializeField] private int maximumTrendDays = 14;
    [SerializeField] private int daysPerWeek = 7;
    [SerializeField] private int hoursPerDay = 24;
    [SerializeField] private int minutesPerHour = 60;

    // The current Theme and Genre used by the market summary.
    // Other systems can read these values but cannot change them.
    public string CurrentTheme { get; private set; }
    public string CurrentGenre { get; private set; }

    // The shorter duration from the summary Theme and Genre.
    public int MinutesRemaining { get; private set; }

    public int HoursRemaining
    {
        get
        {
            return Mathf.CeilToInt(
                MinutesRemaining / (float)minutesPerHour);
        }
    }

    public int DaysRemaining
    {
        get
        {
            return Mathf.CeilToInt(
                MinutesRemaining
                / (float)(minutesPerHour * hoursPerDay));
        }
    }

    // The number of in-game days that have passed.
    public int CurrentDay { get; private set; }

    public int CurrentWeek
    {
        get
        {
            return (CurrentDay / daysPerWeek) + 1;
        }
    }

    public int WeeksRemaining
    {
        get
        {
            return Mathf.CeilToInt(
                MinutesRemaining
                / (float)(daysPerWeek
                    * hoursPerDay
                    * minutesPerHour));
        }
    }

    // Allows the UI to refresh whenever the market data updates.
    public event Action OnMarketUpdated;

    // Tells other systems when at least one trend is replaced.
    public event Action OnTrendChanged;

    // Stores the direction and remaining duration of every Theme.
    private int[] themeDirections;
    private int[] themeDurationMinutes;

    // Stores the demand and remaining duration of every Genre.
    private int[] genreDemands;
    private int[] genreDurationMinutes;

    // Counts elapsed hours for the optional week display.
    private int elapsedMinutes;

    // Stores which Theme and Genre are shown in the summary.
    private int currentThemeIndex;
    private int currentGenreIndex;

    // Returns how many Themes and Genres the UI must display.
    public int ThemeCount => themes.Length;
    public int GenreCount => genres.Length;

    // Returns true after the first trends have been generated.
    public bool IsInitialized
    {
        get
        {
            return themeDirections != null
                && themeDurationMinutes != null
                && genreDemands != null
                && genreDurationMinutes != null;
        }
    }

    private void OnEnable()
    {
        // Listen for a new day from the Time Tracker.
        if (gameTimeManager != null)
        {
            gameTimeManager.OnMinutesPassed += AdvanceMinutes;
        }
    }

    private void OnDisable()
    {
        // Stop listening when this manager becomes inactive.
        if (gameTimeManager != null)
        {
            gameTimeManager.OnMinutesPassed -= AdvanceMinutes;
        }
    }

    private void Start()
    {
        // Create the first set of trends when the game begins.
        CurrentDay = 0;
        elapsedMinutes = 0;
        GenerateNewTrend();
    }

    // Generates a complete new set of Theme and Genre trends.
    public void GenerateNewTrend()
    {
        GenerateThemeTrends();
        GenerateGenreTrends();
        UpdateCurrentMarketSummary();

        OnTrendChanged?.Invoke();
        OnMarketUpdated?.Invoke();

        Debug.Log(
            "New Market Trend: "
            + CurrentTheme
            + " + "
            + CurrentGenre);
    }

    // Generates the direction and duration of every Theme.
    private void GenerateThemeTrends()
    {
        themeDirections = new int[themes.Length];
        themeDurationMinutes = new int[themes.Length];

        for (int i = 0; i < themes.Length; i++)
        {
            // -1 means declining, 0 means steady, and 1 means rising.
            themeDirections[i] =
                UnityEngine.Random.Range(-1, 2);

            themeDurationMinutes[i] =
                GenerateDurationMinutes();
        }

        // Make sure at least one Theme begins as rising.
        int risingTheme =
            UnityEngine.Random.Range(0, themes.Length);

        themeDirections[risingTheme] = 1;
    }

    // Generates the demand and duration of every Genre.
    private void GenerateGenreTrends()
    {
        genreDemands = new int[genres.Length];
        genreDurationMinutes = new int[genres.Length];

        for (int i = 0; i < genres.Length; i++)
        {
            genreDemands[i] =
                UnityEngine.Random.Range(20, 91);

            genreDurationMinutes[i] =
                GenerateDurationMinutes();
        }
    }

    // Chooses a random duration between 5 and 14 days.
    private int GenerateDurationMinutes()
    {
        int durationDays =
            UnityEngine.Random.Range(
                minimumTrendDays,
                maximumTrendDays + 1);

        return durationDays
            * hoursPerDay
            * minutesPerHour;
    }

    // Advances every countdown by the supplied in-game minutes.
    public void AdvanceMinutes(int minutesPassed)
    {
        if (minutesPassed <= 0)
        {
            return;
        }

        if (!IsInitialized)
        {
            GenerateNewTrend();
            return;
        }

        elapsedMinutes += minutesPassed;
        CurrentDay = elapsedMinutes
            / (minutesPerHour * hoursPerDay);
        bool anyTrendChanged = false;

        for (int i = 0; i < themeDurationMinutes.Length; i++)
        {
            themeDurationMinutes[i] -= minutesPassed;

            if (themeDurationMinutes[i] <= 0)
            {
                themeDirections[i] =
                    UnityEngine.Random.Range(-1, 2);

                themeDurationMinutes[i] =
                    GenerateDurationMinutes();

                anyTrendChanged = true;
            }
        }

        for (int i = 0; i < genreDurationMinutes.Length; i++)
        {
            genreDurationMinutes[i] -= minutesPassed;

            if (genreDurationMinutes[i] <= 0)
            {
                genreDemands[i] =
                    UnityEngine.Random.Range(20, 91);

                genreDurationMinutes[i] =
                    GenerateDurationMinutes();

                anyTrendChanged = true;
            }
        }

        UpdateCurrentMarketSummary();

        if (anyTrendChanged)
        {
            OnTrendChanged?.Invoke();
        }

        OnMarketUpdated?.Invoke();
    }

    // Advances every market countdown by one in-game day.
    public void AdvanceDay()
    {
        AdvanceMinutes(
            hoursPerDay * minutesPerHour);
    }

    // Advances seven individual days for testing.
    public void AdvanceWeek()
    {
        for (int i = 0; i < daysPerWeek; i++)
        {
            AdvanceDay();
        }
    }

    // Chooses the best Theme and highest-demand Genre for the summary.
    private void UpdateCurrentMarketSummary()
    {
        FindBestTheme();
        FindHighestDemandGenre();

        CurrentTheme = themes[currentThemeIndex];
        CurrentGenre = genres[currentGenreIndex];

        // The summary changes when the earlier trend expires.
        MinutesRemaining = Mathf.Min(
            themeDurationMinutes[currentThemeIndex],
            genreDurationMinutes[currentGenreIndex]);
    }

    // Rising is preferred over steady, and steady over declining.
    private void FindBestTheme()
    {
        int bestDirection = -2;
        currentThemeIndex = 0;

        for (int i = 0; i < themeDirections.Length; i++)
        {
            if (themeDirections[i] > bestDirection)
            {
                bestDirection = themeDirections[i];
                currentThemeIndex = i;
            }
        }
    }

    // Finds the Genre with the highest demand percentage.
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

    // These methods allow the UI to read each row safely.
    public string GetThemeName(int index) => themes[index];
    public int GetThemeDirection(int index) => themeDirections[index];
    public int GetThemeMinutesRemaining(int index) => themeDurationMinutes[index];
    public string GetGenreName(int index) => genres[index];
    public int GetGenreDemand(int index) => genreDemands[index];
    public int GetGenreMinutesRemaining(int index) => genreDurationMinutes[index];

    // Allows ten minutes to be tested from the component menu.
    [ContextMenu("TEST - Advance 10 Minutes")]
    private void TestAdvanceTenMinutes()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Enter Play mode before testing.");
            return;
        }

        AdvanceMinutes(10);
    }

    // Allows one hour to be tested from the component menu.
    [ContextMenu("TEST - Advance 1 Hour")]
    private void TestAdvanceOneHour()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Enter Play mode before testing.");
            return;
        }

        AdvanceMinutes(minutesPerHour);
    }

#if UNITY_EDITOR

    // Allows one day to be tested from the component menu.
    [ContextMenu("TEST - Advance 1 Day")]
    private void TestAdvanceOneDay()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Enter Play mode before testing.");
            return;
        }

        AdvanceDay();
    }

    // Allows one week to be tested from the component menu.
    [ContextMenu("TEST - Advance 1 Week")]
    private void TestAdvanceOneWeek()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Enter Play mode before testing.");
            return;
        }

        AdvanceWeek();
    }

    // Generates new trends directly from the component menu.
    [ContextMenu("TEST - Generate New Trend")]
    private void TestGenerateNewTrend()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Enter Play mode before testing.");
            return;
        }

        GenerateNewTrend();
    }

#endif
}

