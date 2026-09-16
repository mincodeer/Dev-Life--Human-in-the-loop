// Student ID: 23208000

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Controls the development tutorial popup.
/// Displays tutorial dialogue with a typing animation
/// and follows the player's actual development progress.
/// </summary>
public class DevelopmentTutorialPopup : MonoBehaviour
{
    // =========================================================
    // POPUP REFERENCES
    // =========================================================

    [Header("Popup References")]

    [SerializeField]
    private GameObject popupPanel;

    [SerializeField]
    private TextMeshProUGUI popupText;

    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private Button skipButton;


    // =========================================================
    // TYPING SETTINGS
    // =========================================================

    [Header("Typing Settings")]

    [SerializeField]
    private float typingSpeed = 0.03f;


    // =========================================================
    // TUTORIAL STATE
    // =========================================================

    // Intro tutorial:
    // 0 = Welcome
    // 1 = Computer instruction
    private int currentTutorialStep = 0;

    private Coroutine typingCoroutine;

    private bool isTyping = false;

    private bool tutorialSkipped = false;


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextClicked);
        }

        if (skipButton != null)
        {
            skipButton.onClick.AddListener(OnSkipClicked);
        }

        ShowWelcome();
    }


    // =========================================================
    // WELCOME
    // =========================================================

    public void ShowWelcome()
    {
        if (tutorialSkipped)
        {
            return;
        }

        currentTutorialStep = 0;

        ShowPopup(
            "WELCOME TO DEV LIFE: HUMAN IN THE LOOP!\n" +
            "You're about to start your journey as an indie game developer."
        );
    }


    // =========================================================
    // COMPUTER INSTRUCTION
    // =========================================================

    public void ShowComputerInstruction()
    {
        if (tutorialSkipped)
        {
            return;
        }

        currentTutorialStep = 1;

        ShowPopup(
            "This is your development workspace.\n" +
            "Click the computer to start creating your game."
        );
    }


    // =========================================================
    // THEME EXPLANATION
    // =========================================================

    public void ShowThemeExplanation()
    {
        if (tutorialSkipped)
        {
            return;
        }

        ShowPopup(
            "Every game starts with an idea!\n" +
            "Choose a theme for your game."
        );
    }


    // =========================================================
    // THEME SELECTED
    // =========================================================

    public void ThemeSelected()
    {
        if (tutorialSkipped)
        {
            return;
        }

        ShowPopup(
            "Great choice!\n" +
            "Now choose a genre for your game."
        );
    }


    // =========================================================
    // GENRE SELECTED
    // =========================================================

    public void GenreSelected()
    {
        if (tutorialSkipped)
        {
            return;
        }

        ShowPopup(
            "Great choice!\n" +
            "Your theme and genre are ready.\n" +
            "Select Start Development when you're ready to begin."
        );
    }


    // =========================================================
    // CODING
    // =========================================================

    public void ShowCoding()
    {
        if (tutorialSkipped)
        {
            return;
        }

        ShowPopup(
            "CODING\n\n" +
            "Create the systems and mechanics that make your game work.\n" +
            "Your AI or Manual choice affects Quality, Workload, and Technical Debt."
        );
    }


    // =========================================================
    // DESIGN
    // =========================================================

    public void ShowDesign()
    {
        if (tutorialSkipped)
        {
            return;
        }

        ShowPopup(
            "DESIGN\n\n" +
            "Shape how your game looks and feels.\n" +
            "Your choices can affect the quality of your project."
        );
    }


    // =========================================================
    // SOUND
    // =========================================================

    public void ShowSound()
    {
        if (tutorialSkipped)
        {
            return;
        }

        ShowPopup(
            "SOUND\n\n" +
            "Give your game a voice through music and sound effects.\n" +
            "Good audio can help bring your game to life."
        );
    }


    // =========================================================
    // DEBUGGING
    // =========================================================

    public void ShowDebugging()
    {
        if (tutorialSkipped)
        {
            return;
        }

        ShowPopup(
            "DEBUGGING\n\n" +
            "Find and fix problems before your game is released.\n" +
            "Keep an eye on your bugs and project conditions."
        );
    }


    // =========================================================
    // BUILD
    // =========================================================

    public void ShowBuild()
    {
        if (tutorialSkipped)
        {
            return;
        }

        ShowPopup(
            "BUILD\n\n" +
            "Your game is almost ready!\n" +
            "Make sure you're happy with your project before building it."
        );
    }


    // =========================================================
    // FINAL RESULT
    // =========================================================

    public void ShowFinalResult()
    {
        if (tutorialSkipped)
        {
            return;
        }

        ShowPopup(
            "CONGRATULATIONS!\n\n" +
            "Your game is complete!\n\n" +
            "Let's see how your development choices affected the final result."
        );
    }


    // =========================================================
    // NEXT BUTTON
    // =========================================================

    private void OnNextClicked()
    {
        // If text is still typing,
        // finish it first.
        if (isTyping)
        {
            FinishTyping();
            return;
        }

        // Welcome → Computer instruction
        if (currentTutorialStep == 0)
        {
            currentTutorialStep = 1;

            ShowComputerInstruction();

            return;
        }

        // Once the computer instruction is displayed,
        // Next cannot move the tutorial forward.
        //
        // The player must actually click the computer.
    }


    // =========================================================
    // FINISH TYPING
    // =========================================================

    private void FinishTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);

            typingCoroutine = null;
        }

        popupText.text = GetCurrentMessage();

        isTyping = false;
    }


    // =========================================================
    // GET CURRENT MESSAGE
    // =========================================================

    private string GetCurrentMessage()
    {
        switch (currentTutorialStep)
        {
            case 0:

                return
                    "WELCOME TO DEV LIFE: HUMAN IN THE LOOP!\n\n" +
                    "You're about to start your journey as an indie game developer.";

            case 1:

                return
                    "This is your development workspace.\n\n" +
                    "Click the computer to start creating your game.";

            default:

                return popupText.text;
        }
    }


    // =========================================================
    // SKIP BUTTON
    // =========================================================

    private void OnSkipClicked()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);

            typingCoroutine = null;
        }

        isTyping = false;

        tutorialSkipped = true;

        CloseTutorial();
    }


    // =========================================================
    // SHOW POPUP
    // =========================================================

    private void ShowPopup(string message)
    {
        if (tutorialSkipped)
        {
            return;
        }

        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(
            TypeMessage(message));
    }


    // =========================================================
    // TYPING ANIMATION
    // =========================================================

    private IEnumerator TypeMessage(string message)
    {
        isTyping = true;

        popupText.text = "";

        foreach (char letter in message)
        {
            popupText.text += letter;

            yield return new WaitForSeconds(
                typingSpeed);
        }

        isTyping = false;

        typingCoroutine = null;
    }


    // =========================================================
    // CLOSE TUTORIAL
    // =========================================================

    public void CloseTutorial()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
    }


    // =========================================================
    // SHOW TUTORIAL
    // =========================================================

    public void ShowTutorial()
    {
        tutorialSkipped = false;

        currentTutorialStep = 0;

        ShowWelcome();
    }


    // =========================================================
    // CLEAN UP
    // =========================================================

    private void OnDestroy()
    {
        if (nextButton != null)
        {
            nextButton.onClick.RemoveListener(
                OnNextClicked);
        }

        if (skipButton != null)
        {
            skipButton.onClick.RemoveListener(
                OnSkipClicked);
        }
    }
}