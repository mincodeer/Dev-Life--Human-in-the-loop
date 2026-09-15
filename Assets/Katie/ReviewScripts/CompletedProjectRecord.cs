using System;

/// <summary>
/// Stores a permanent snapshot of one completed project.
///
/// This is separate from ProjectData because ProjectData is reset
/// whenever the player starts a new project.
/// </summary>
[Serializable]
public class CompletedProjectRecord
{
    // ==============================
    // Project Identity
    // ==============================

    public string projectName;

    public GameTheme theme;
    public GameGenre genre;

    // ==============================
    // Main Result
    // ==============================

    // Final review score from 0 to 100.
    public float finalScore;

    // Rewards earned by this project.
    public int moneyEarned;
    public int fandomGained;

    // Temporary week value.
    // This can be connected to a real week system later.
    public int completedWeek;

    // ==============================
    // Development Result Details
    // ==============================

    public float quality;
    public float workload;
    public int technicalDebt;
    public int bugs;
    public float developmentTime;
    public float marketBonus;

    /// <summary>
    /// Empty constructor required by Unity serialization.
    /// </summary>
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

        // Older records did not store a separate quality value.
        // Using finalScore prevents their quality from displaying as zero.
        quality = finalScore;
    }
}