/// <summary>
/// Controls the order of the development stages.
/// This saves the player's choices before moving to the next stage.
/// </summary>

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DevelopmentFlowManager : MonoBehaviour
{
    [Header("Tutorial")]
    [SerializeField]
    private UnityEvent onProjectSetupOpened =
        new UnityEvent();


    [Header("Current Stage")]
    [SerializeField]
    private DevelopmentStage currentStage =
        DevelopmentStage.None;

    public bool IsComputerUIOpen =>
        developmentUI != null &&
        developmentUI.activeInHierarchy;

    public DevelopmentStage CurrentStage =>
        currentStage;


    [Header("Main UI")]
    [SerializeField]
    private GameObject developmentUI;


    [Header("Computer Interaction")]
    [SerializeField]
    private ComputerInteractionIn computerInteraction;


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


    [Header("Development Process")]
    [SerializeField]
    private CodingStageController codingActivity;

    [SerializeField]
    private DesignStageController designActivity;

    [SerializeField]
    private SoundStageController soundActivity;

    [SerializeField]
    private Animator buildAnimator;

    [SerializeField]
    private GameObject buildButtonObject;

    [SerializeField]
    private GameObject succeededObject;

    [SerializeField]
    private float buildAnimationTime = 3f;


    private void Start()
    {
        CloseComputer();
    }


    // ==============================
    // Computer
    // ==============================

    public void OpenComputer()
    {
        if (developmentUI.activeInHierarchy)
            return;

        DevelopmentStage stageToOpen =
            currentStage;

        if (stageToOpen == DevelopmentStage.None)
        {
            stageToOpen =
                DevelopmentStage.ProjectSetup;
        }

        ChangeStage(stageToOpen);

        developmentUI.SetActive(true);

        if (stageToOpen ==
            DevelopmentStage.ProjectSetup)
        {
            StartCoroutine(
                NotifyProjectSetupOpened());
        }
    }


    private IEnumerator NotifyProjectSetupOpened()
    {
        yield return null;

        if (currentStage ==
                DevelopmentStage.ProjectSetup &&
            developmentUI.activeInHierarchy &&
            projectSetupPanel.activeInHierarchy)
        {
            onProjectSetupOpened.Invoke();
        }
    }


    // Important:
    // Closing the computer UI does NOT reset
    // the current development stage.
    public void CloseComputer()
    {
        if (developmentUI != null)
        {
            developmentUI.SetActive(false);
        }

        if (projectConditionsPanel != null)
        {
            projectConditionsPanel.SetActive(false);
        }
    }


    // ==============================
    // Stage Completion
    // ==============================

    public void CompleteCoding()
    {
        CompleteStageAndReturnToComputer(
            DevelopmentStage.Coding,
            DevelopmentStage.Design);
    }


    public void CompleteDesign()
    {
        CompleteStageAndReturnToComputer(
            DevelopmentStage.Design,
            DevelopmentStage.Sound);
    }


    public void CompleteSound()
    {
        CompleteStageAndReturnToComputer(
            DevelopmentStage.Sound,
            DevelopmentStage.Debugging);

        // If you decide to skip Debugging,
        // change DevelopmentStage.Debugging above
        // to DevelopmentStage.Build.
    }


    private void CompleteStageAndReturnToComputer(
        DevelopmentStage expectedStage,
        DevelopmentStage nextStage)
    {
        if (currentStage != expectedStage)
            return;

        // Save the next stage.
        currentStage = nextStage;

        // Hide the stage panels.
        HideAllPanels();

        // Hide the computer UI.
        CloseComputer();

        // Camera stays zoomed in.
        // Only the interaction state changes.
        if (computerInteraction != null)
        {
            computerInteraction
                .ReturnToZoomedInState();
        }
        else
        {
            Debug.LogWarning(
                "ComputerInteractionIn is missing.");
        }

        Debug.Log(
            "Stage Complete. Next Stage: "
            + currentStage);
    }


    // ==============================
    // Change Stage
    // ==============================

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


    // ==============================
    // Stage Navigation
    // ==============================

    public void GotoCoding()
    {
        ChangeStage(
            DevelopmentStage.Coding);
    }


    public void GotoDesign()
    {
        ChangeStage(
            DevelopmentStage.Design);
    }


    public void GotoSound()
    {
        ChangeStage(
            DevelopmentStage.Sound);
    }


    public void GotoDebugging()
    {
        ChangeStage(
            DevelopmentStage.Debugging);
    }


    public void GotoBuild()
    {
        ChangeStage(
            DevelopmentStage.Build);

        if (succeededObject != null)
        {
            succeededObject.SetActive(false);
        }

        if (buildButtonObject != null)
        {
            buildButtonObject.SetActive(true);
        }
    }


    // ==============================
    // Build
    // ==============================

    public void StartBuild()
    {
        if (buildButtonObject != null)
        {
            buildButtonObject.SetActive(false);
        }

        StartCoroutine(
            BuildRoutine());
    }


    private IEnumerator BuildRoutine()
    {
        if (succeededObject != null)
        {
            succeededObject.SetActive(false);
        }

        if (buildAnimator != null)
        {
            buildAnimator.SetTrigger("Build");
        }
        else
        {
            Debug.LogWarning(
                "Build Animator is missing.");
        }

        yield return new WaitForSeconds(
            buildAnimationTime);

        if (succeededObject != null)
        {
            succeededObject.SetActive(true);
        }

        Debug.Log(
            "Build Succeeded!");
    }


    // ==============================
    // Result
    // ==============================

    public void GotoResult()
    {
        if (finalResultsCalculator != null)
        {
            finalResultsCalculator
                .CalculateFinalResult();
        }
        else
        {
            Debug.LogWarning(
                "FinalResultsCalculator is missing.");
        }


        if (resultsUI != null)
        {
            resultsUI.DisplayResults();
        }
        else
        {
            Debug.LogWarning(
                "ResultsUI is missing.");
        }


        if (projectConditionsPanel != null)
        {
            projectConditionsPanel.SetActive(false);
        }


        ChangeStage(
            DevelopmentStage.Result);
    }


    // ==============================
    // New Project
    // ==============================

    public void StartNewProject()
    {
        if (ProjectDataManager.Instance != null)
        {
            ProjectDataManager.Instance
                .StartNewProject();
        }
        else
        {
            Debug.LogWarning(
                "ProjectDataManager is missing.");
        }


        if (projectResourceController != null)
        {
            projectResourceController
                .StartNewProject();
        }
        else
        {
            Debug.LogWarning(
                "ProjectResourceController is missing.");
        }


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
            ProjectDataManager.Instance
                .CurrentProject;

        if (!project.HasProjectSetup)
        {
            Debug.LogWarning(
                "Select both Theme and Genre.");

            return;
        }


        if (projectResourceController != null)
        {
            projectResourceController
                .StartNewProject();
        }
        else
        {
            Debug.LogWarning(
                "ProjectResourceController is missing.");
        }


        GotoCoding();
    }


    // ==============================
    // Manual / AI
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


    private void SaveMethodAndContinue(
        WorkMethod method)
    {
        if (currentStage ==
                DevelopmentStage.Coding &&
            (codingActivity == null ||
             !codingActivity.CanBegin()))
        {
            Debug.LogWarning(
                "Coding activity is missing, not ready, or already running.");

            return;
        }


        if (currentStage ==
                DevelopmentStage.Design &&
            (designActivity == null ||
             !designActivity.CanBegin()))
        {
            Debug.LogWarning(
                "Check Design Activity and Drawing Steps.");

            return;
        }


        if (currentStage ==
                DevelopmentStage.Sound &&
            (soundActivity == null ||
             !soundActivity.CanBegin()))
        {
            Debug.LogWarning(
                "Check Sound activity and Tracks");

            return;
        }


        if (ProjectDataManager.Instance == null)
        {
            Debug.LogError(
                "ProjectDataManager is missing.");

            return;
        }


        bool wasSaved =
            ProjectDataManager.Instance
                .SetWorkMethod(
                    currentStage,
                    method);

        if (!wasSaved)
        {
            Debug.LogWarning(
                "Cannot select a work method during: "
                + currentStage);

            return;
        }


        if (projectResultsCalculator != null)
        {
            projectResultsCalculator
                .ProcessStageDecision(
                    currentStage,
                    method);
        }
        else
        {
            Debug.LogWarning(
                "ProjectResultsCalculator is missing.");
        }


        switch (currentStage)
        {
            case DevelopmentStage.Coding:

                codingActivity.Begin(
                    method == WorkMethod.AI);

                break;


            case DevelopmentStage.Design:

                designActivity.Begin(
                    method == WorkMethod.AI);

                break;


            case DevelopmentStage.Sound:

                soundActivity.Begin(
                    method == WorkMethod.AI);

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