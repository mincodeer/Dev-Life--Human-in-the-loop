//Student ID:23208000

using System;

/// Stores the current conditions of the game project being developed.
/// These values belong to one project and should reset when a new project begins.
[Serializable]
public class ProjectConditions
{
    // Project quality ranges from 0 to 100.
    public float quality = 0f;

    // Project workload ranges from 0 to 100.
    public float workload = 0f;

    // Total number of bugs created during development.
    public int bugs = 0;

    // Total development time for this project.
    public float totalTime = 0f;

    /// Adds to the current project quality and keeps it between 0 and 100.
    public void ChangeQuality(float amount)
    {
        quality += amount;
        quality = Clamp(quality, 0f, 100f);
    }

    /// Adds to the current project workload and keeps it between 0 and 100.
    public void ChangeWorkload(float amount)
    {
        workload += amount;
        workload = Clamp(workload, 0f, 100f);
    }

    /// Adds bugs to the current project.
    /// Bugs cannot go below zero.

    public void ChangeBugs(int amount)
    {
        bugs += amount;

        if (bugs < 0)
        {
            bugs = 0;
        }
    }

    /// Adds development time to the current project.
    /// Time cannot go below zero.
    public void ChangeTime(float amount)
    {
        totalTime += amount;

        if (totalTime < 0f)
        {
            totalTime = 0f;
        }
    }

 
    /// Resets all project conditions for a new project.
    public void ResetConditions()
    {
        quality = 0f;
        workload = 0f;
        bugs = 0;
        totalTime = 0f;
    }

    private float Clamp(float value, float min, float max)
    {
        if (value < min)
        {
            return min;
        }

        if (value > max)
        {
            return max;
        }

        return value;
    }
}