// Student ID: 23208000

using System;

/// <summary>
/// Stores the final outcome of a completed game project.
/// The Results UI can use this information to display
/// the project's final performance and rewards.
/// </summary>
[Serializable]
public class FinalProjectResult
{
    // Final score of the completed project.
    public float finalScore;

    // Final project conditions.
    public float quality;
    public float workload;
    public int technicalDebt;
    public int bugs;
    public float developmentTime;

    // Rewards earned from completing the project.
    public int moneyEarned;
    public int fandomGained;

    // Bonus received from matching the market trend.
    public float marketBonus;
}