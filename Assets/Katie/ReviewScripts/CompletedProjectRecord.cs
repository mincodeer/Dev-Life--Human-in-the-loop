using System;

[Serializable]
public class CompletedProjectRecord
{
    public string projectName;

    public GameTheme theme;
    public GameGenre genre;

    // Stored as the original 0-100 result score.
    public float finalScore;

    public int moneyEarned;
    public int completedWeek;

    // Stores the detailed result values so the
    // Review Report can create relevant comments.
    public float quality;
    public float workload;
    public int technicalDebt;
    public int bugs;
    public float developmentTime;

    // Stores the rewards from the completed project.
    public int fandomGained;

    // Stores the Market Trend result.
    public float marketBonus;
    public string marketMatchType;
    public string trendingTheme;
    public string trendingGenre;

    public CompletedProjectRecord()
    {
    }

    // This original constructor is kept so the
    // Dashboard sample test continues to work.
    public CompletedProjectRecord(
        string projectName,
        GameTheme theme,
        GameGenre genre,
        float finalScore,
        int moneyEarned,
        int completedWeek)
    {
        this.projectName = projectName;
        this.theme = theme;
        this.genre = genre;
        this.finalScore = finalScore;
        this.moneyEarned = moneyEarned;
        this.completedWeek = completedWeek;
    }
}