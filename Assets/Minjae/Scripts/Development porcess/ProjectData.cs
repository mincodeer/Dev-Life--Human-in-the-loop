using System;
using System.Text;

/// <summary>
/// Stores all choices for the game project currently being developed.
/// This is a serializable data model, not a component placed on a GameObject.
/// </summary>
[Serializable]
public class ProjectData
{
    public string projectName = "Untitled Project";

    public GameTheme selectedTheme = GameTheme.None;
    public GameGenre selectedGenre = GameGenre.None;

    public WorkMethod codingMethod = WorkMethod.None;
    public WorkMethod designMethod = WorkMethod.None;
    public WorkMethod soundMethod = WorkMethod.None;
    public WorkMethod debuggingMethod = WorkMethod.None;

    // Ready for the Result system in a later sprint.
    public float totalTime;
    public int totalCost;
    public float quality;
    public int bugs;

    public bool HasProjectSetup
    {
        get
        {
            return selectedTheme != GameTheme.None
                && selectedGenre != GameGenre.None;
        }
    }

    public bool HasAllWorkMethods
    {
        get
        {
            return codingMethod != WorkMethod.None
                && designMethod != WorkMethod.None
                && soundMethod != WorkMethod.None
                && debuggingMethod != WorkMethod.None;
        }
    }

    public void SetWorkMethod(DevelopmentStage stage, WorkMethod method)
    {
        switch (stage)
        {
            case DevelopmentStage.Coding:
                codingMethod = method;
                break;

            case DevelopmentStage.Design:
                designMethod = method;
                break;

            case DevelopmentStage.Sound:
                soundMethod = method;
                break;

            case DevelopmentStage.Debugging:
                debuggingMethod = method;
                break;

            default:
                throw new ArgumentException(
                    "This stage does not have a Manual/AI choice: " + stage,
                    "stage");
        }
    }

    public WorkMethod GetWorkMethod(DevelopmentStage stage)
    {
        switch (stage)
        {
            case DevelopmentStage.Coding:
                return codingMethod;

            case DevelopmentStage.Design:
                return designMethod;

            case DevelopmentStage.Sound:
                return soundMethod;

            case DevelopmentStage.Debugging:
                return debuggingMethod;

            default:
                throw new ArgumentException(
                    "This stage does not have a Manual/AI choice: " + stage,
                    "stage");
        }
    }

    public string ToReadableSummary()
    {
        StringBuilder summary = new StringBuilder();
        summary.AppendLine("Project: " + projectName);
        summary.AppendLine("Theme: " + selectedTheme);
        summary.AppendLine("Genre: " + selectedGenre);
        summary.AppendLine("Coding: " + codingMethod);
        summary.AppendLine("Design: " + designMethod);
        summary.AppendLine("Sound: " + soundMethod);
        summary.Append("Debugging: " + debuggingMethod);
        return summary.ToString();
    }
}