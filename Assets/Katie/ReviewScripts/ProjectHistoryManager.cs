using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Provides completed-project records to the Dashboard and Review scenes.
/// The original project data remains owned by ProjectDataManager.
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
    /// Copies archived projects from ProjectDataManager into the
    /// Dashboard's display format. ProjectDataManager remains the source
    /// of truth and is not modified by this method.
    /// </summary>
    public void RefreshFromProjectDataManager()
    {
        if (ProjectDataManager.Instance == null)
        {
            Debug.LogWarning(
                "ProjectHistoryManager could not find ProjectDataManager.");

            return;
        }

        IReadOnlyList<ProjectData> archivedProjects =
            ProjectDataManager.Instance.CompletedProjects;

        int selectedIndex =
            SelectedProject == null
            ? -1
            : completedProjects.IndexOf(SelectedProject);

        // Rebuild the display list so it always matches the archived source.
        completedProjects.Clear();

        for (int i = 0;
             i < archivedProjects.Count;
             i++)
        {
            ProjectData projectData = archivedProjects[i];

            if (projectData == null || projectData.finalResult == null)
            {
                Debug.LogWarning(
                    "Archived project " + (i + 1)
                    + " does not contain a final result.");

                continue;
            }

            CompletedProjectRecord completedProject =
                new CompletedProjectRecord(
                    projectData,
                    projectData.finalResult,
                    i + 1);

            completedProjects.Add(completedProject);
        }

        if (completedProjects.Count == 0)
        {
            SelectedProject = null;
        }
        else if (selectedIndex >= 0
                 && selectedIndex < completedProjects.Count)
        {
            SelectedProject = completedProjects[selectedIndex];
        }
        else
        {
            // Select the newest completed project by default.
            SelectedProject =
                completedProjects[completedProjects.Count - 1];
        }

        HistoryChanged?.Invoke();
        SelectedProjectChanged?.Invoke();
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

        GameTheme[] sampleThemes =
        {
            GameTheme.Fantasy,
            GameTheme.SciFi,
            GameTheme.Horror
        };

        GameGenre[] sampleGenres =
        {
            GameGenre.RPG,
            GameGenre.Action,
            GameGenre.Simulation
        };

        float randomFinalScore =
            Random.Range(35f, 96f);

        int randomMoneyEarned =
            Random.Range(200, 2001);

        CompletedProjectRecord sampleProject =
            new CompletedProjectRecord(
                "PROJECT " + projectNumber,
                sampleThemes[
                    Random.Range(0, sampleThemes.Length)],
                sampleGenres[
                    Random.Range(0, sampleGenres.Length)],
                randomFinalScore,
                randomMoneyEarned,
                projectNumber);

        // Random details make the Dashboard and Review tests visibly different.
        sampleProject.fandomGained =
            Random.Range(5, 101);

        sampleProject.quality =
            Mathf.Clamp(
                randomFinalScore + Random.Range(-15f, 16f),
                0f,
                100f);

        sampleProject.workload =
            Random.Range(20f, 101f);

        sampleProject.technicalDebt =
            Random.Range(0, 31);

        sampleProject.bugs =
            Random.Range(0, 11);

        sampleProject.developmentTime =
            Random.Range(5f, 31f);

        sampleProject.marketBonus =
            Random.value >= 0.5f
            ? Random.Range(1f, 16f)
            : 0f;

        AddCompletedProject(sampleProject);
    }

    [ContextMenu("TEST - Clear Project History")]
    private void TestClearProjectHistory()
    {
        ClearHistory();
    }

#endif
}