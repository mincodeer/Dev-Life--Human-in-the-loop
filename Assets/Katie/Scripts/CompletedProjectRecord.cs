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

    /// <summary>
    /// Creates a permanent completed-project record using
    /// the selected project data and calculated final result.
    /// </summary>
    public CompletedProjectRecord(
        ProjectData projectData,
        FinalProjectResult finalResult,
        int completedWeek)
    {
        // Copy the identity of the project.
        projectName = projectData.projectName;
        theme = projectData.selectedTheme;
        genre = projectData.selectedGenre;

        // Copy the main calculated result.
        finalScore = finalResult.finalScore;
        moneyEarned = finalResult.moneyEarned;
        fandomGained = finalResult.fandomGained;

        // Store when the project was completed.
        this.completedWeek = completedWeek;

        // Copy the development conditions.
        quality = finalResult.quality;
        workload = finalResult.workload;
        technicalDebt = finalResult.technicalDebt;
        bugs = finalResult.bugs;
        developmentTime = finalResult.developmentTime;
        marketBonus = finalResult.marketBonus;
    }

    /// <summary>
    /// Keeps compatibility with older test code that creates
    /// a record using only the original six values.
    /// </summary>
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