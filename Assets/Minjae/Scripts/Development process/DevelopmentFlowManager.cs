// Student ID: 23208000

using UnityEngine;

/// <summary>
/// Controls the order of the development stages.
/// This saves the player's choices before moving to the next stage.
/// It also triggers the appropriate tutorial explanation
/// when each development stage begins.
/// </summary>
public class DevelopmentFlowManager : MonoBehaviour
{
    // =========================================================
    // CURRENT STAGE
    // =========================================================

    [Header("Current Stage")]

    [SerializeField]
    private DevelopmentStage currentStage =
        DevelopmentStage.None;

    public DevelopmentStage CurrentStage =>
        currentStage;


    // =========================================================
    // MAIN UI
    // =========================================================

    [Header("Main UI")]

    [SerializeField]
    private GameObject developmentUI;


    // =========================================================
    // DEVELOPMENT PANELS
    // =========================================================

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


    // =========================================================
    // PROJECT RESOURCES
    // =========================================================

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


    // =========================================================
    // TUTORIAL
    // =========================================================

    [Header("Tutorial")]

    [SerializeField]
    private DevelopmentTutorialPopup developmentTutorialPopup;


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        CloseComputer();
    }


    // =========================================================
    // OPEN COMPUTER
    // =========================================================

    public void OpenComputer()
    {
        developmentUI.SetActive(true);

        projectConditionsPanel.SetActive(true);

        ChangeStage(
            DevelopmentStage.ProjectSetup);
    }


    // =========================================================
    // CLOSE COMPUTER
    // =========================================================

    public void CloseComputer()
    {
        developmentUI.SetActive(false);

        projectConditionsPanel.SetActive(false);

        currentStage =
            DevelopmentStage.None;
    }


    // =========================================================
    // CHANGE STAGE
    // =========================================================

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

        // Tell the tutorial that a new stage has started.
        ShowStageTutorial();

        Debug.Log(
            "Development Stage: "
            + currentStage);
    }


    // =========================================================
    // SHOW STAGE TUTORIAL
    // =========================================================

    private void ShowStageTutorial()
    {
        if (developmentTutorialPopup == null)
        {
            return;
        }

        switch (currentStage)
        {
            case DevelopmentStage.Coding:

                developmentTutorialPopup.ShowCoding();

                break;


            case DevelopmentStage.Design:

                developmentTutorialPopup.ShowDesign();

                break;


            case DevelopmentStage.Sound:

                developmentTutorialPopup.ShowSound();

                break;


            case DevelopmentStage.Debugging:

                developmentTutorialPopup.ShowDebugging();

                break;


            case DevelopmentStage.Build:

                developmentTutorialPopup.ShowBuild();

                break;


            case DevelopmentStage.Result:

                developmentTutorialPopup.ShowFinalResult();

                break;
        }
    }


    // =========================================================
    // HIDE ALL DEVELOPMENT PANELS
    // =========================================================

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


    // =========================================================
    // GOTO CODING
    // =========================================================

    public void GotoCoding()
    {
        ChangeStage(
            DevelopmentStage.Coding);
    }


    // =========================================================
    // GOTO DESIGN
    // =========================================================

    public void GotoDesign()
    {
        ChangeStage(
            DevelopmentStage.Design);
    }


    // =========================================================
    // GOTO SOUND
    // =========================================================

    public void GotoSound()
    {
        ChangeStage(
            DevelopmentStage.Sound);
    }


    // =========================================================
    // GOTO DEBUGGING
    // =========================================================

    public void GotoDebugging()
    {
        ChangeStage(
            DevelopmentStage.Debugging);
    }


    // =========================================================
    // GOTO BUILD
    // =========================================================

    public void GotoBuild()
    {
        ChangeStage(
            DevelopmentStage.Build);
    }


    // =========================================================
    // GOTO RESULT
    // =========================================================

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

        if (projectConditionsPanel != null)
        {
            projectConditionsPanel.SetActive(false);
        }


        // Open the final Results panel.
        // This will also trigger the Final Result tutorial.

        ChangeStage(
            DevelopmentStage.Result);
    }


    // =========================================================
    // NEW PROJECT
    // =========================================================

    public void StartNewProject()
    {
        // Reset the current project data.

        if (ProjectDataManager.Instance != null)
        {
            ProjectDataManager.Instance.StartNewProject();
        }
        else
        {
            Debug.LogWarning(
                "ProjectDataManager is missing.");
        }


        // Reset project conditions.

        if (projectResourceController != null)
        {
            projectResourceController.StartNewProject();
        }
        else
        {
            Debug.LogWarning(
                "ProjectResourceController is missing.");
        }


        // Return to project setup.

        ChangeStage(
            DevelopmentStage.ProjectSetup);


        Debug.Log(
            "New project started.");
    }


    // =========================================================
    // PROJECT SETUP SELECTION
    // =========================================================

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


    // =========================================================
    // START DEVELOPMENT
    // =========================================================

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


        // Reset project conditions.
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


    // =========================================================
    // MANUAL / AI SELECTION
    // =========================================================

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


    // =========================================================
    // SAVE METHOD AND CONTINUE
    // =========================================================

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


        // Apply the consequences of the decision.

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


    // =========================================================
    // THEME DROPDOWN
    // =========================================================

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


    // =========================================================
    // GENRE DROPDOWN
    // =========================================================

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
    // =========================================================
    // COMPLETE CODING
    // =========================================================

    public void CompleteCoding()
    {
        GotoDesign();
    }


    // =========================================================
    // COMPLETE DESIGN
    // =========================================================

    public void CompleteDesign()
    {
        GotoSound();
    }


    // =========================================================
    // COMPLETE SOUND
    // =========================================================

    public void CompleteSound()
    {
        GotoDebugging();
    }
}