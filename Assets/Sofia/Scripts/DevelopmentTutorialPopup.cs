// Student ID: 23208000

using UnityEngine;
using TMPro;

public class DevelopmentTutorialPopup : MonoBehaviour
{
    // =========================================================
    // UI REFERENCES
    // =========================================================

    // The popup panel that contains the tutorial message.
    [SerializeField]
    private GameObject popupPanel;

    // The TextMeshPro text used to display the tutorial message.
    [SerializeField]
    private TextMeshProUGUI popupText;


    // =========================================================
    // DEVELOPMENT SYSTEM
    // =========================================================

    // Reference to Minjae's DevelopmentFlowManager.
    [SerializeField]
    private DevelopmentFlowManager developmentFlowManager;


    // =========================================================
    // TUTORIAL STATE
    // =========================================================

    // Keeps track of the last development stage shown.
    // This prevents the same popup from appearing repeatedly.
    private DevelopmentStage lastStage =
        DevelopmentStage.None;


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        // Hide the popup when the development scene begins.
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }

        // Start with no stage recorded.
        lastStage = DevelopmentStage.None;
    }


    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        // Make sure the DevelopmentFlowManager exists.
        if (developmentFlowManager == null)
        {
            return;
        }

        // Get the current development stage.
        DevelopmentStage currentStage =
            developmentFlowManager.CurrentStage;

        // Only show a new tutorial message when
        // the development stage changes.
        if (currentStage == lastStage)
        {
            return;
        }

        // Remember the new stage.
        lastStage = currentStage;

        // Show the appropriate tutorial message.
        switch (currentStage)
        {
            case DevelopmentStage.Coding:
                ShowCoding();
                break;

            case DevelopmentStage.Design:
                ShowDesign();
                break;

            case DevelopmentStage.Sound:
                ShowSound();
                break;

            case DevelopmentStage.Debugging:
                ShowDebugging();
                break;

            case DevelopmentStage.Build:
                ShowBuild();
                break;

            case DevelopmentStage.Result:
                ShowFinalResult();
                break;
        }
    }

    // =========================================================
    // THEME EXPLANATION
    // =========================================================

    // Explains that the player should choose a theme
    // for their new game.
    public void ShowThemeExplanation()
    {
        ShowPopup(
            "Every game starts with an idea!\n\n" +
            "First, choose a theme for your game."
        );
    }

    // =========================================================
    // THEME SELECTED
    // =========================================================

    // Called when the player successfully selects a theme.
    public void ThemeSelected()
    {
        ShowPopup(
            "Great choice!\n\n" +
            "Now choose a genre for your game."
        );
    }


    // =========================================================
    // GENRE SELECTED
    // =========================================================

    // Called when the player successfully selects a genre.
    public void GenreSelected()
    {
        ShowPopup(
            "Great choice!\n\n" +
            "Your theme and genre are ready. " +
            "Select Start Development when you're ready to begin."
        );
    }

    // =========================================================
    // CODING
    // =========================================================

    // Explains the Coding stage.
    public void ShowCoding()
    {
        ShowPopup(
            "CODING\n\n" +
            "This is where you create the systems and mechanics " +
            "that make your game work.\n\n" +
            "Choose AI or Manual development. " +
            "Your choice will affect your project's " +
            "Quality, Workload, and Technical Debt."
        );
    }


    // =========================================================
    // DESIGN
    // =========================================================

    // Explains the Design stage.
    public void ShowDesign()
    {
        ShowPopup(
            "DESIGN\n\n" +
            "Here you shape how your game looks " +
            "and how players experience it.\n\n" +
            "Choose AI or Manual development. " +
            "Your choice will affect your project's " +
            "Quality, Workload, and Technical Debt."
        );
    }


    // =========================================================
    // SOUND
    // =========================================================

    // Explains the Sound stage.
    public void ShowSound()
    {
        ShowPopup(
            "SOUND\n\n" +
            "This is where you create the audio for your game.\n\n" +
            "Choose AI or Manual development. " +
            "Your choice will affect your project's " +
            "Quality, Workload, and Technical Debt."
        );
    }


    // =========================================================
    // DEBUGGING
    // =========================================================

    // Explains the Debugging stage.
    public void ShowDebugging()
    {
        ShowPopup(
            "DEBUGGING\n\n" +
            "Find and fix bugs before your game is released.\n\n" +
            "Choose AI or Manual development. " +
            "Your choice will affect your project's " +
            "Quality, Workload, Technical Debt, and Bugs."
        );
    }


    // =========================================================
    // BUILD
    // =========================================================

    // Explains the Build stage.
    public void ShowBuild()
    {
        ShowPopup(
            "BUILD\n\n" +
            "Your game is ready to be built and prepared for release.\n\n" +
            "Your choices throughout development have affected " +
            "your project's final results."
        );
    }


    // =========================================================
    // FINAL RESULT
    // =========================================================

    // Explains that the development process is complete.
    public void ShowFinalResult()
    {
        ShowPopup(
            "CONGRATULATIONS!\n\n" +
            "Your game is complete!\n\n" +
            "You've made your development choices and managed " +
            "your project resources.\n\n" +
            "You're ready to start your first game!"
        );
    }


    // =========================================================
    // SHOW POPUP
    // =========================================================

    // Displays the tutorial popup with the supplied message.
    private void ShowPopup(string message)
    {
        if (popupText != null)
        {
            popupText.text = message;
        }

        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
        }
    }


    // =========================================================
    // CLOSE POPUP
    // =========================================================

    // Called by the popup's OK button.
    public void ClosePopup()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
    }
}