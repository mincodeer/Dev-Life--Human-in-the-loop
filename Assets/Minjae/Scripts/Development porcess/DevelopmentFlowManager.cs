/// <summary>
/// Controls the order of the development stages.
/// This saves the player's choices before moving to the next stage.
/// </summary>

using UnityEngine;

public class DevelopmentFlowManager : MonoBehaviour
{
    [Header("Current Stage")]
    [SerializeField]
    private DevelopmentStage currentStage =
        DevelopmentStage.None;

    [Header("Main UI")]
    [SerializeField]
    private GameObject developmentUI;

    [Header("Development Panels")]
    [SerializeField]
    private GameObject projectSetupPanel;
    [SerializeField]
    private GameObject codingPanel;
    [SerializeField]
    private GameObject designPanel;
    [SerializeField]
    private GameObject soundPanel;
    [SerializeField]
    private GameObject debuggingPanel;
    [SerializeField]
    private GameObject buildPanel;
    [SerializeField]
    private GameObject resultPanel;

    [Header("Project Resources")]
    [SerializeField]
    private ProjectResultsCalculator projectResultsCalculator;

    [SerializeField]
    private ProjectResourceController projectResourceController;

    [SerializeField]
    private GameObject projectConditionsPanel;

    [SerializeField]
    private FinalResultsCalculator finalResultsCalculator;

    [SerializeField]
    private ResultsUI resultsUI;

    private void Start()
    {
        CloseComputer();
    }

    public void OpenComputer()
    {
        developmentUI.SetActive(true);
        projectConditionsPanel.SetActive(true);

        ChangeStage(DevelopmentStage.ProjectSetup);
    }

    public void CloseComputer()
    {
        developmentUI.SetActive(false);
        projectConditionsPanel.SetActive(false);

        currentStage = DevelopmentStage.None;
    }

    public void ChangeStage(
        DevelopmentStage newStage)
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

        Debug.Log(
            "Development Stage: "
            + currentStage);
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
        // Calculate the final result before opening
        // the Results panel.
        if (finalResultsCalculator != null)
        {
            finalResultsCalculator.CalculateFinalResult();
        }
        else
        {
            Debug.LogWarning(
                "FinalResultsCalculator is missing.");
        }

        // Display the calculated result.
        if (resultsUI != null)
        {
            resultsUI.DisplayResults();
        }
        else
        {
            Debug.LogWarning(
                "ResultsUI is missing.");
        }

        // Hide the project conditions because
        // development has now finished.
        projectConditionsPanel.SetActive(false);

        // Open the final Results panel.
        ChangeStage(DevelopmentStage.Result);
    }

    // ==============================
    // New Project
    // ==============================

    // Resets the current project and starts a fresh
    // project without resetting permanent resources.
    public void StartNewProject()
    {
        // Reset the stored project choices.
        if (ProjectDataManager.Instance != null)
        {
            ProjectDataManager.Instance.StartNewProject();
        }
        else
        {
            Debug.LogWarning(
                "ProjectDataManager is missing.");
        }

        // Reset the conditions for the new project.
        if (projectResourceController != null)
        {
            projectResourceController.StartNewProject();
        }
        else
        {
            Debug.LogWarning(
                "ProjectResourceController is missing.");
        }

        // Return to the project setup screen.
        ChangeStage(
            DevelopmentStage.ProjectSetup);

        Debug.Log(
            "New project started.");
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

        // Reset the conditions for the new project.
        // Permanent resources are kept.
        if (projectResourceController != null)
        {
            projectResourceController.StartNewProject();
        }
        else
        {
            Debug.LogWarning(
                "ProjectResourceController is missing.");
        }

        GotoCoding();
    }

    // ==============================
    // Manual / AI Selection
    // ==============================

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

    // Saves the player's Manual or AI choice,
    // applies the resource consequences,
    // then moves to the next stage.
    private void SaveMethodAndContinue(
        WorkMethod method)
    {
        if (ProjectDataManager.Instance == null)
        {
            Debug.LogError(
                "ProjectDataManager is missing.");

            return;
        }

        // Save the player's selected work method.
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

        // Apply the consequences of the player's
        // Manual or AI decision.
        if (projectResultsCalculator != null)
        {
            projectResultsCalculator.ProcessStageDecision(
                currentStage,
                method);
        }
        else
        {
            Debug.LogWarning(
                "ProjectResultsCalculator is missing.");
        }

        // Move to the next development stage.
        switch (currentStage)
        {
            case DevelopmentStage.Coding:
                GotoDesign();
                break;

            case DevelopmentStage.Design:
                GotoSound();
                break;

            case DevelopmentStage.Sound:
                GotoDebugging();
                break;

            case DevelopmentStage.Debugging:
                GotoBuild();
                break;
        }
    }

    // ==============================
    // Dropdown Selection
    // ==============================

    public void OnThemeDropdownChanged(
        int index)
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

    public void OnGenreDropdownChanged(
        int index)
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