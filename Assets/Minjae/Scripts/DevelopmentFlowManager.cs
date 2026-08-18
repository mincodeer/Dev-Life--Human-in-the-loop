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

    private void Start()
    {
        CloseComputer();
    }

    public void OpenComputer()
    {
        developmentUI.SetActive(true);

        ChangeStage(DevelopmentStage.ProjectSetup);
    }

    public void CloseComputer()
    {
        developmentUI.SetActive(false);

        currentStage = DevelopmentStage.None;
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
        ChangeStage(DevelopmentStage.Result);
    }
}