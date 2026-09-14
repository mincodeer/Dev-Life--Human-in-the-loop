using System;

[Serializable]
public class CompletedProjectRecord
{
    public string projectName;

    public GameTheme theme;
    public GameGenre genre;

    // Stored as the original 0–100 result score.
    public float finalScore;

    public int moneyEarned;
    public int completedWeek;

    public CompletedProjectRecord()
    {
    }

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