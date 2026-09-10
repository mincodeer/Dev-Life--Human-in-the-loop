/// <summary>
/// Controls the order of the development stages
/// This saves the player's choices before moving to the next stage.
/// </summary>

using UnityEngine;

public class DevelopmentFlowManager : MonoBehaviour
{
    [Header("Current Stage")]
    [SerializeField] private DevelopmentStage currentStage = DevelopmentStage.None;

    [Header("Main UI")]
    [SerializeField] private GameObject developmentUI;

    [Header("Development Panels")]
    [SerializeField] private GameObject projectSetupPanel;
    [SerializeField] private GameObject codingPanel;
    [SerializeField] private GameObject designPanel;
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private GameObject debuggingPanel;
    [SerializeField] private GameObject buildPanel;
    [SerializeField] private GameObject resultPanel;

    [Header("Coding Activity")]
    [SerializeField] private CodingStageController codingActivity;
    [SerializeField] private DesignStageController designActivity;

    private void Start()
    {
        CloseComputer();
    }

    public void OpenComputer()
    {
        if (developmentUI.activeInHierarchy)
            return;

        DevelopmentStage stageToOpen = currentStage;

        if (stageToOpen == DevelopmentStage.None)
        {
            stageToOpen = DevelopmentStage.ProjectSetup;
        }

        ChangeStage(stageToOpen);
        developmentUI.SetActive(true);
    }

    public void CloseComputer()
    {
        developmentUI.SetActive(false);

        currentStage = DevelopmentStage.None;
    }

    // Coding is done, but Design waits for the next computer click.
    public void CompleteCoding()
    {
        if (currentStage != DevelopmentStage.Coding) return;
        currentStage = DevelopmentStage.Design;
        HideAllPanels();
        developmentUI.SetActive(false);
    }

    public void CompleteDesign()
    {
        if (currentStage != DevelopmentStage.Design)
            return;

        currentStage = DevelopmentStage.Sound;
        HideAllPanels();
        developmentUI.SetActive(false);
    }

    public void ChangeStage(DevelopmentStage newStage)
    {
        currentStage = newStage;

        HideAllPanels();

        switch (currentStage)
        {
            case DevelopmentStage.ProjectSetup:
                projectSetupPanel.SetActive(true);
                break;

            case DevelopmentStage.Coding:
                codingPanel.SetActive(true);
                break;

            case DevelopmentStage.Design:
                designPanel.SetActive(true);
                break;

            case DevelopmentStage.Sound:
                soundPanel.SetActive(true);
                break;

            case DevelopmentStage.Debugging:
                debuggingPanel.SetActive(true);
                break;

            case DevelopmentStage.Build:
                buildPanel.SetActive(true);
                break;

            case DevelopmentStage.Result:
                resultPanel.SetActive(true);
                break;
        }

        Debug.Log("Development Stage: " + currentStage);
    }

    private void HideAllPanels()
    {
        projectSetupPanel.SetActive(false);
        codingPanel.SetActive(false);
        designPanel.SetActive(false);
        soundPanel.SetActive(false);
        debuggingPanel.SetActive(false);
        buildPanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    public void GotoCoding()
    {
        ChangeStage(DevelopmentStage.Coding);
    }

    public void GotoDesign()
    {
        ChangeStage(DevelopmentStage.Design);
    }

    public void GotoSound()
    {
        ChangeStage(DevelopmentStage.Sound);
    }

    public void GotoDebugging()
    {
        ChangeStage(DevelopmentStage.Debugging);
    }

    public void GotoBuild()
    {
        ChangeStage(DevelopmentStage.Build);
    }

    public void GotoResult()
    {
        // Result calculation integration point
        // After the build stage, it will read ProjectDataManager.Instance.Current Project.
        // Check the porject.HasAllWorkedMethods, and calculate Money, fandom, Tech Debt, Dev skill, Time, Quality, Cost and bugs (if too much remove some of them).
        // Apply the claculated changes to ResourceManager(create one).
        // Display the calculated outcome on the result panel.
        ChangeStage(DevelopmentStage.Result);
    }

        // ==============================
    // Project Setup Selection
    // ==============================

    public void None()
    {
        return;
    }
    public void SelectFantasy()
    {
        ProjectDataManager.Instance.SetTheme(
            GameTheme.Fantasy);
    }

    public void SelectSciFi()
    {
        ProjectDataManager.Instance.SetTheme(
            GameTheme.SciFi);
    }

    public void SelectHorror()
    {
        ProjectDataManager.Instance.SetTheme(
            GameTheme.Horror);
    }

    public void SelectRPG()
    {
        ProjectDataManager.Instance.SetGenre(
            GameGenre.RPG);
    }

    public void SelectAction()
    {
        ProjectDataManager.Instance.SetGenre(
            GameGenre.Action);
    }

    public void SelectSimulation()
    {
        ProjectDataManager.Instance.SetGenre(
            GameGenre.Simulation);
    }

    public void StartDevelopment()
    {
        if (ProjectDataManager.Instance == null)
        {
            Debug.LogError(
                "ProjectDataManager is missing.");

            return;
        }

        ProjectData project =
            ProjectDataManager.Instance.CurrentProject;

        if (!project.HasProjectSetup)
        {
            Debug.LogWarning(
                "Select both Theme and Genre.");

            return;
        }

        GotoCoding();
    }

    // Manual / AI Selection

    public void ChooseManual()
    {
        SaveMethodAndContinue(
            WorkMethod.Manual);
    }

    public void ChooseAI()
    {
        SaveMethodAndContinue(
            WorkMethod.AI);
    }

    // Save first. Coding now runs an activity; other stages keep their old behavior for lesson 1.
    private void SaveMethodAndContinue(
        WorkMethod method)
    {
        if (currentStage == DevelopmentStage.Coding &&
            (codingActivity == null || !codingActivity.CanBegin()))
        {
            Debug.LogWarning("Coding activity is missing, not ready, or already running.");
            return;
        }

        if (currentStage == DevelopmentStage.Design &&
            (designActivity == null || !designActivity.CanBegin()))
        {
            Debug.LogWarning("Check Design Activity and Drawing Steps.");
            return;
        }

        if (ProjectDataManager.Instance == null)
        {
            Debug.LogError("ProjectDataManager is missing.");
            return;
        }

        bool wasSaved =
            ProjectDataManager.Instance.SetWorkMethod(
                currentStage,
                method);

        if (!wasSaved)
        {
            Debug.LogWarning(
                "Cannot select a work method during: "
                + currentStage);

            return;
        }

        switch (currentStage)
        {
            case DevelopmentStage.Coding:
                codingActivity.Begin(method == WorkMethod.AI);
                break;

            case DevelopmentStage.Design:
                designActivity.Begin(method == WorkMethod.AI);
                break;

            case DevelopmentStage.Sound:
                GotoDebugging();
                break;

            case DevelopmentStage.Debugging:
                GotoBuild();
                break;
        }
    }

        public void OnThemeDropdownChanged(int index)
    {
        switch (index)
        {
            case 0:
            None();
            break;
            
            case 1:
            SelectFantasy();
            break;

            case 2:
            SelectSciFi();
            break;

            case 3:
            SelectHorror();
            break;
        }
    }

    public void OnGenreDropdownChanged(int index)
    {
        switch (index)
        {
            case 0:
            None();
            break;

            case 1:
            SelectRPG();
                break;

            case 2:
                SelectAction();
                break;

            case 3:
                SelectSimulation();
                break;
        }
    }
}