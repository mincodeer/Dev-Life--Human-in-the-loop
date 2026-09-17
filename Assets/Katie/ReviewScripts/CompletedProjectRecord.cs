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
    /// Creates a completed-project record from individual values.
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

        // Older sample records do not provide these values.
        fandomGained = 0;
        quality = finalScore;
        workload = 0f;
        technicalDebt = 0;
        bugs = 0;
        developmentTime = 0f;
        marketBonus = 0f;
    }

    /// <summary>
    /// Creates a completed-project record from project data
    /// and its calculated final result.
    /// </summary>
    public CompletedProjectRecord(
        ProjectData projectData,
        FinalProjectResult finalResult,
        int completedWeek)
    {
        if (projectData == null)
        {
            throw new ArgumentNullException(
                nameof(projectData));
        }

        if (finalResult == null)
        {
            throw new ArgumentNullException(
                nameof(finalResult));
        }

        // Copy the project's identity before ProjectData is reset.
        projectName = projectData.projectName;
        theme = projectData.selectedTheme;
        genre = projectData.selectedGenre;

        // Copy the calculated final result.
        finalScore = finalResult.finalScore;
        moneyEarned = finalResult.moneyEarned;
        fandomGained = finalResult.fandomGained;

        this.completedWeek = completedWeek;

        // Use the final score as the default quality value.
        // Detailed values can be added by another system later.
        quality = finalScore;
        workload = 0f;
        technicalDebt = 0;
        bugs = 0;
        developmentTime = 0f;
        marketBonus = 0f;
    }
}
