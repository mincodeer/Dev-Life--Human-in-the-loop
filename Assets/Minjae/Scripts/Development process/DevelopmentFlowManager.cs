using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DevelopmentFlowManager : MonoBehaviour
{
    [Header("Current Stage")]
    [SerializeField] private DevelopmentStage currentStage = DevelopmentStage.None;

    public DevelopmentStage CurrentStage => currentStage;

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

    [Header("Project Resources")]
    [SerializeField] private ProjectResultsCalculator projectResultsCalculator;
    [SerializeField] private ProjectResourceController projectResourceController;
    [SerializeField] private GameObject projectConditionsPanel;
    [SerializeField] private FinalResultsCalculator finalResultsCalculator;
    [SerializeField] private ResultsUI resultsUI;

    [Header("Tutorial")]
    [SerializeField] private DevelopmentTutorialPopup developmentTutorialPopup;

    [Header("Minigames")]
    [SerializeField] private CodingStageController codingActivity;
    [SerializeField] private DesignStageController designActivity;
    [SerializeField] private SoundStageController soundActivity;
    [SerializeField] private BuildStageController buildStage;
    [SerializeField] private DebuggingStageController debuggingActivity;

    [Header("Result Exit")]
    [SerializeField] private UnityEvent onProjectFinished = new UnityEvent();

    public void FinishProjectAndExit()
    {
        if (currentStage != DevelopmentStage.Result) return;
        CloseComputer();
        HideAllPanels();
        currentStage = DevelopmentStage.None;
        onProjectFinished.Invoke();
    }

    public bool IsComputerUIOpen => developmentUI.activeInHierarchy;

    private void Start()
    {
        currentStage = DevelopmentStage.None;
        HideAllPanels();
        CloseComputer();
    }

    public void OpenComputer()
    {
        if (IsComputerUIOpen) return;
        developmentUI.SetActive(true);
        if (projectConditionsPanel != null)
            projectConditionsPanel.SetActive(currentStage != DevelopmentStage.Result);

        if (currentStage == DevelopmentStage.None)
        {
            StartNewProject();
            if (developmentTutorialPopup != null)
                developmentTutorialPopup.ShowThemeExplanation();
        }
    }

    public void CloseComputer()
    {
        developmentUI.SetActive(false);
        if (projectConditionsPanel != null)
            projectConditionsPanel.SetActive(false);
        if (developmentTutorialPopup != null)
            developmentTutorialPopup.CloseTutorial();
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
        ShowStageTutorial();

        Debug.Log("Development Stage: "
            + currentStage);
    }

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

    public void GotoCoding() => ChangeStage(DevelopmentStage.Coding);

    public void GotoDesign() => ChangeStage(DevelopmentStage.Design);

    public void GotoSound() => ChangeStage(DevelopmentStage.Sound);

    public void GotoDebugging() => ChangeStage(DevelopmentStage.Debugging);

    public void GotoBuild() => ChangeStage(DevelopmentStage.Build);

    public void GotoResult()
    {
        if (currentStage != DevelopmentStage.Build) return;

        if (finalResultsCalculator != null)
        {
            FinalProjectResult result = finalResultsCalculator.CalculateFinalResult();
            if (ProjectDataManager.Instance != null)
                ProjectDataManager.Instance.ArchiveCurrentProject(result);
        }
        else
        {
            Debug.LogWarning("FinalResultsCalculator is missing.");
        }

        if (resultsUI != null)
        {
            resultsUI.DisplayResults();
        }
        else
        {
            Debug.LogWarning("ResultsUI is missing.");
        }

        if (projectConditionsPanel != null)
        {
            projectConditionsPanel.SetActive(false);
        }

        ChangeStage(DevelopmentStage.Result);
    }

    public void StartNewProject()
    {
        if (buildStage != null) buildStage.ResetBuild();
        if (debuggingActivity != null) debuggingActivity.ResetDebugging();

        if (ProjectDataManager.Instance != null)
        {
            ProjectDataManager.Instance.StartNewProject();
        }
        else
        {
            Debug.LogWarning("ProjectDataManager is missing.");
        }

        if (projectResourceController != null)
        {
            projectResourceController.StartNewProject();
        }
        else
        {
            Debug.LogWarning("ProjectResourceController is missing.");
        }

        // Match the visible dropdowns to the empty data without firing tutorials.
        foreach (TMP_Dropdown dropdown in projectSetupPanel.GetComponentsInChildren<TMP_Dropdown>(true))
        {
            dropdown.SetValueWithoutNotify(0);
            dropdown.RefreshShownValue();
        }
        ChangeStage(DevelopmentStage.ProjectSetup);

        Debug.Log("New project started.");
    }

    public void None() { }

    public void SelectFantasy() => ProjectDataManager.Instance.SetTheme(GameTheme.Fantasy);

    public void SelectSciFi() => ProjectDataManager.Instance.SetTheme(GameTheme.SciFi);

    public void SelectHorror() => ProjectDataManager.Instance.SetTheme(GameTheme.Horror);

    public void SelectRPG() => ProjectDataManager.Instance.SetGenre(GameGenre.RPG);

    public void SelectAction() => ProjectDataManager.Instance.SetGenre(GameGenre.Action);

    public void SelectSimulation() => ProjectDataManager.Instance.SetGenre(GameGenre.Simulation);

    public void StartDevelopment()
    {
        if (ProjectDataManager.Instance == null)
        {
            Debug.LogError("ProjectDataManager is missing.");

            return;
        }

        ProjectData project = ProjectDataManager.Instance.CurrentProject;

        if (!project.HasProjectSetup)
        {
            Debug.LogWarning("Select both Theme and Genre.");

            return;
        }

        GotoCoding();
    }

    public void ChooseManual() => SaveMethodAndContinue(WorkMethod.Manual);

    public void ChooseAI() => SaveMethodAndContinue(WorkMethod.AI);

    private void SaveMethodAndContinue(WorkMethod method)
    {
        if (!IsComputerUIOpen) return;
        if (currentStage != DevelopmentStage.Coding &&
            currentStage != DevelopmentStage.Design &&
            currentStage != DevelopmentStage.Sound &&
            currentStage != DevelopmentStage.Debugging) return;

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

        if (currentStage == DevelopmentStage.Sound && (soundActivity == null || !soundActivity.CanBegin()))
        {
            Debug.LogWarning("Check Sound activity and Tracks");
            return;
        }

        if (currentStage == DevelopmentStage.Debugging &&
            (debuggingActivity == null || !debuggingActivity.CanBegin() ||
             projectResultsCalculator == null || projectResourceController == null))
        {
            Debug.LogWarning("Check Debugging Activity, its four lines and resource references.");
            return;
        }

        if (ProjectDataManager.Instance == null)
        {
            Debug.LogError("ProjectDataManager is missing.");
            return;
        }
        bool wasSaved = ProjectDataManager.Instance.SetWorkMethod(currentStage, method);

        if (!wasSaved)
        {
            Debug.LogWarning("Cannot select a work method during: "
                + currentStage);

            return;
        }
        if (currentStage == DevelopmentStage.Debugging)
        {
            // Debugging applies its stage rewards on completion instead.
        }
        else if (projectResultsCalculator != null)
        {
            projectResultsCalculator.ProcessStageDecision(currentStage, method);
        }
        else
        {
            Debug.LogWarning("ProjectResultsCalculator is missing.");
        }
        // Selection starts the minigame; completion advances the stage.
        switch (currentStage)
        {
            case DevelopmentStage.Coding:
                codingActivity.Begin(method == WorkMethod.AI);
                break;
            case DevelopmentStage.Design:
                designActivity.Begin(method == WorkMethod.AI);
                break;
            case DevelopmentStage.Sound:
                soundActivity.Begin(method == WorkMethod.AI);
                break;
            case DevelopmentStage.Debugging:
                debuggingActivity.Begin(method == WorkMethod.AI);
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

    public void CompleteCoding()
    {
        if (currentStage != DevelopmentStage.Coding) return;
        GotoDesign();
    }

    public void CompleteDesign()
    {
        if (currentStage != DevelopmentStage.Design) return;
        GotoSound();
    }

    public void CompleteSound()
    {
        if (currentStage != DevelopmentStage.Sound) return;
        GotoDebugging();
    }
    public void CompleteDebugging()
    {
        if (currentStage != DevelopmentStage.Debugging) return;
        if (!projectResourceController.HasStageResult(DevelopmentStage.Debugging))
        {
            WorkMethod method = ProjectDataManager.Instance.CurrentProject.debuggingMethod;
            StageResult result = projectResultsCalculator.GenerateStageResult(
                DevelopmentStage.Debugging, method);
            // Red lines already removed bugs; do not remove a percentage again.
            result.bugRemovalPercentage = 0f;
            projectResourceController.ApplyStageResult(DevelopmentStage.Debugging, result);
        }
        GotoBuild();
    }

    #if UNITY_EDITOR
[Header("Editor Play Test")]
[SerializeField] private DevelopmentStage testStage = DevelopmentStage.None;

private void Update()
{
    if (testStage == DevelopmentStage.None)
        return;

    DevelopmentStage stage = testStage;
    testStage = DevelopmentStage.None;

    OpenComputer();
    ChangeStage(stage);
}
#endif
}

