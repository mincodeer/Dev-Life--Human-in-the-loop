using System;
using System.Collections.Generic;
using UnityEngine;

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

    public IReadOnlyList<CompletedProjectRecord> CompletedProjects
    {
        get
        {
            return completedProjects;
        }
    }

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

    public event Action HistoryChanged;
    public event Action SelectedProjectChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddCompletedProject(
        CompletedProjectRecord project)
    {
        if (project == null)
        {
            Debug.LogWarning(
                "Cannot add a null completed project."
            );

            return;
        }

        completedProjects.Add(project);

        Debug.Log(
            "Completed project added: "
            + project.projectName
        );

        HistoryChanged?.Invoke();
    }

    public void SelectProject(
        CompletedProjectRecord project)
    {
        if (project == null)
        {
            Debug.LogWarning(
                "Cannot select a null project."
            );

            return;
        }

        SelectedProject = project;

        Debug.Log(
            "Selected project: "
            + SelectedProject.projectName
        );

        SelectedProjectChanged?.Invoke();
    }

    public int GetTotalMoneyEarned()
    {
        int total = 0;

        for (int i = 0;
             i < completedProjects.Count;
             i++)
        {
            total += completedProjects[i].moneyEarned;
        }

        return total;
    }

    public float GetBestFinalScore()
    {
        float bestScore = 0f;

        for (int i = 0;
             i < completedProjects.Count;
             i++)
        {
            if (completedProjects[i].finalScore >
                bestScore)
            {
                bestScore =
                    completedProjects[i].finalScore;
            }
        }

        return bestScore;
    }

    public void ClearHistory()
    {
        completedProjects.Clear();
        SelectedProject = null;

        HistoryChanged?.Invoke();
        SelectedProjectChanged?.Invoke();

        Debug.Log("Project history cleared.");
    }

#if UNITY_EDITOR

    [ContextMenu("TEST - Add Sample Project")]
    private void TestAddSampleProject()
    {
        int projectNumber =
            completedProjects.Count + 1;

        CompletedProjectRecord sampleProject =
            new CompletedProjectRecord(
                "PROJECT " + projectNumber,
                default,
                default,
                80f,
                500,
                projectNumber * 5
            );

        AddCompletedProject(sampleProject);

        Debug.Log(
            "Projects: " + ProjectCount
            + "\nTotal Money: $"
            + GetTotalMoneyEarned()
            + "\nBest Score: "
            + GetBestFinalScore()
        );
    }

    [ContextMenu("TEST - Clear Project History")]
    private void TestClearProjectHistory()
    {
        ClearHistory();
    }

#endif
}