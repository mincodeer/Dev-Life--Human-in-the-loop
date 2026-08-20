using UnityEngine;

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

    public ProjectData CurrentProject
    {
        get
        {
            return currentProject;
        }
    }

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

    public void StartNewProject()
    {
        currentProject = new ProjectData();

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

        Debug.Log("Current project data reset.");
    }
}