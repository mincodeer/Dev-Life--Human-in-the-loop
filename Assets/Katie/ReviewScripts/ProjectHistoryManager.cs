using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores every completed project while the game is running.
///
/// This object survives scene changes so the Development,
/// Dashboard and Review scenes can share the same records.
/// </summary>
public class ProjectHistoryManager : MonoBehaviour
{
    public static ProjectHistoryManager Instance
    {
        get;
        private set;
    }

    [Header("Completed Projects")]
    [SerializeField]
    private List<CompletedProjectRecord> completedProjects =
        new List<CompletedProjectRecord>();

    // Other scripts can read the list but cannot replace it.
    public IReadOnlyList<CompletedProjectRecord> CompletedProjects
    {
        get
        {
            return completedProjects;
        }
    }

    // The project currently selected on the dashboard.
    public CompletedProjectRecord SelectedProject
    {
        get;
        private set;
    }

    public int ProjectCount
    {
        get
        {
            return completedProjects.Count;
        }
    }

    // DashboardUI listens to these events.
    public event Action HistoryChanged;
    public event Action SelectedProjectChanged;

    private void Awake()
    {
        // Prevent two history managers from existing.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Keep the history when another scene opens.
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Creates and saves a completed record using the project
    /// choices and the result returned by FinalResultsCalculator.
    /// </summary>
    public void AddCompletedProjectFromResult(
        ProjectData projectData,
        FinalProjectResult finalResult)
    {
        if (projectData == null)
        {
            Debug.LogWarning(
                "Cannot save a project because ProjectData is null.");

            return;
        }

        if (finalResult == null)
        {
            Debug.LogWarning(
                "Cannot save a project because FinalProjectResult is null.");

            return;
        }

        // TODO:
        // Replace this with the real current week when a week
        // or calendar system becomes available.
        int temporaryCompletedWeek =
            completedProjects.Count + 1;

        // Create a permanent copy before ProjectData is reset.
        CompletedProjectRecord completedProject =
            new CompletedProjectRecord(
                projectData,
                finalResult,
                temporaryCompletedWeek);

        AddCompletedProject(completedProject);
    }

    /// <summary>
    /// Adds an already-created record to project history.
    /// </summary>
    public void AddCompletedProject(
        CompletedProjectRecord project)
    {
        if (project == null)
        {
            Debug.LogWarning(
                "Cannot add a null completed project.");

            return;
        }

        completedProjects.Add(project);

        // Automatically select the newest project.
        SelectedProject = project;

        // Tell the dashboard that its values must refresh.
        HistoryChanged?.Invoke();
        SelectedProjectChanged?.Invoke();

        Debug.Log(
            "Completed project added: "
            + project.projectName);
    }

    /// <summary>
    /// Selects the project represented by a clicked dashboard row.
    /// </summary>
    public void SelectProject(
        CompletedProjectRecord project)
    {
        if (project == null)
        {
            Debug.LogWarning(
                "Cannot select a null project.");

            return;
        }

        SelectedProject = project;

        SelectedProjectChanged?.Invoke();

        Debug.Log(
            "Selected project: "
            + SelectedProject.projectName);
    }

    /// <summary>
    /// Adds together the money earned by all completed projects.
    /// </summary>
    public int GetTotalMoneyEarned()
    {
        int totalMoney = 0;

        for (int i = 0; i < completedProjects.Count; i++)
        {
            totalMoney += completedProjects[i].moneyEarned;
        }

        return totalMoney;
    }

    /// <summary>
    /// Adds together the fandom earned by all completed projects.
    /// </summary>
    public int GetTotalFandomGained()
    {
        int totalFandom = 0;

        for (int i = 0; i < completedProjects.Count; i++)
        {
            totalFandom += completedProjects[i].fandomGained;
        }

        return totalFandom;
    }

    /// <summary>
    /// Returns the highest final score in project history.
    /// </summary>
    public float GetBestFinalScore()
    {
        float bestScore = 0f;

        for (int i = 0; i < completedProjects.Count; i++)
        {
            if (completedProjects[i].finalScore > bestScore)
            {
                bestScore = completedProjects[i].finalScore;
            }
        }

        return bestScore;
    }

    /// <summary>
    /// Calculates the average quality of every completed project.
    /// </summary>
    public float GetAverageQuality()
    {
        if (completedProjects.Count == 0)
        {
            return 0f;
        }

        float totalQuality = 0f;

        for (int i = 0; i < completedProjects.Count; i++)
        {
            totalQuality += completedProjects[i].quality;
        }

        return totalQuality / completedProjects.Count;
    }

    /// <summary>
    /// Clears project history. Mainly useful for testing.
    /// </summary>
    public void ClearHistory()
    {
        completedProjects.Clear();
        SelectedProject = null;

        HistoryChanged?.Invoke();
        SelectedProjectChanged?.Invoke();

        Debug.Log("Project history cleared.");
    }

#if UNITY_EDITOR

    /// <summary>
    /// Adds a sample project from the Inspector context menu.
    /// </summary>
    [ContextMenu("TEST - Add Sample Project")]
    private void TestAddSampleProject()
    {
        int projectNumber =
            completedProjects.Count + 1;

        CompletedProjectRecord sampleProject =
            new CompletedProjectRecord(
                "PROJECT " + projectNumber,
                GameTheme.Fantasy,
                GameGenre.RPG,
                80f,
                500,
                projectNumber);

        // Give the test record some fandom.
        sampleProject.fandomGained = 20;

        AddCompletedProject(sampleProject);
    }

    [ContextMenu("TEST - Clear Project History")]
    private void TestClearProjectHistory()
    {
        ClearHistory();
    }

#endif
}
