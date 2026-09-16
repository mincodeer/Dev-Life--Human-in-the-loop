/// <summary>
/// Owns and manages the data for the project currently being developed.
/// </summary>

using UnityEngine;
using System.Collections.Generic;

public class ProjectDataManager : MonoBehaviour
{
    public static ProjectDataManager Instance
    {
        get;
        private set;
    }

    [Header("Current Project Data")]
    [SerializeField]
    private ProjectData currentProject =
        new ProjectData();

    // Main access point for the result calculation system.
    public ProjectData CurrentProject
    {
        get
        {
            return currentProject;
        }
    }

    [Header("Completed Projects")]
    [SerializeField] private List<ProjectData> completedProjects = new List<ProjectData>();
    [SerializeField] private bool currentProjectArchived;

    public IReadOnlyList<ProjectData> CompletedProjects => completedProjects.AsReadOnly();

    public void ArchiveCurrentProject(FinalProjectResult result)
    {
        if (currentProjectArchived || result == null) return;

        currentProject.quality = result.quality;
        currentProject.bugs = result.bugs;
        currentProject.totalTime = result.developmentTime;
        currentProject.finalResult = JsonUtility.FromJson<FinalProjectResult>(
            JsonUtility.ToJson(result));

        // Store a separate copy so later edits cannot change the old project.
        completedProjects.Add(JsonUtility.FromJson<ProjectData>(
            JsonUtility.ToJson(currentProject)));
        currentProjectArchived = true;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }

    public void SetTheme(GameTheme newTheme)
    {
        if (newTheme == GameTheme.None)
        {
            Debug.LogWarning(
                "A valid theme was not selected.");

            return;
        }

        currentProject.selectedTheme = newTheme;

        Debug.Log(
            "Theme selected: " + newTheme);
    }

    public void SetGenre(GameGenre newGenre)
    {
        if (newGenre == GameGenre.None)
        {
            Debug.LogWarning(
                "A valid genre was not selected.");

            return;
        }

        currentProject.selectedGenre = newGenre;

        Debug.Log(
            "Genre selected: " + newGenre);
    }

    public bool SetWorkMethod(
        DevelopmentStage stage,
        WorkMethod method)
    {
        if (method == WorkMethod.None)
        {
            Debug.LogWarning(
                "A valid work method was not selected.");

            return false;
        }

        bool isDevelopmentStage =
            stage == DevelopmentStage.Coding
            || stage == DevelopmentStage.Design
            || stage == DevelopmentStage.Sound
            || stage == DevelopmentStage.Debugging;

        if (!isDevelopmentStage)
        {
            Debug.LogWarning(
                "This stage cannot use Manual/AI: "
                + stage);

            return false;
        }

        currentProject.SetWorkMethod(
            stage,
            method);

        Debug.Log(
            stage + " method selected: " + method);

        return true;
    }

    // Creates empty data for the next project.
    // Call this after the result has been calculated
    public void StartNewProject()
    {
        currentProject = new ProjectData();
        currentProjectArchived = false;

        Debug.Log("New project data created.");
    }

    [ContextMenu("Print Current Project")]
    public void PrintCurrentProject()
    {
        Debug.Log(
            "Theme: "
            + currentProject.selectedTheme
            + "\nGenre: "
            + currentProject.selectedGenre
            + "\nCoding: "
            + currentProject.codingMethod
            + "\nDesign: "
            + currentProject.designMethod
            + "\nSound: "
            + currentProject.soundMethod
            + "\nDebugging: "
            + currentProject.debuggingMethod);
    }

    [ContextMenu("Reset Current Project")]
    public void ResetCurrentProject()
    {
        currentProject = new ProjectData();
        currentProjectArchived = false;

        Debug.Log("Current project data reset.");
    }
}