//Student Id: 23208000

using System;

/// Stores the generated consequences of one development stage.
/// The values are generated once when the player confirms
/// Manual or AI and can be used later for the final project results.
[Serializable]
public class StageResult
{
    // Development time added by this stage.
    public float time;

    // Quality contribution from this stage.
    public float qualityContribution;

    // Number of bugs created by this stage.
    public int bugsCreated;

    // Percentage of the current bugs removed during debugging.
    // This is only used by the Debugging stage.
    public float bugRemovalPercentage;

    // Workload added by this stage.
    public float workload;

    // Development Skill gained from this stage.
    public int skillGained;

    // Technical Debt change caused by this stage.
    public int technicalDebtChange;
}