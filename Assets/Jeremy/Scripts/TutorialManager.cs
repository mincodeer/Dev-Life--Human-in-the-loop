using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    // ============================================================
    // TUTORIAL PHASES
    // ============================================================
    // This enum lists every stage of the tutorial.
    // Each phase can display dialogue or wait for a specific
    // player action before moving to the next phase.
    private enum TutorialPhase
    {
        Introduction,
        EnterComputer,
        ComputerDialogue,
        ThemeAndGenre,
        StartDevelopment,
        Development,
        Shop,
        Complete
    }


    // ============================================================
    // UI REFERENCES
    // ============================================================

    // Reference to the TextMeshPro object used to display
    // tutorial dialogue on the screen.
    public TextMeshProUGUI dialogueText;

    // Reference to the Next button.
    // The button can be hidden when the player must complete
    // an action instead of clicking through dialogue.
    public GameObject nextButton;

    // Reference to the interaction prompt shown when the player
    // needs to press E to interact with the computer.
    public GameObject interactPrompt;


    // ============================================================
    // TYPING EFFECT SETTINGS
    // ============================================================

    // Controls how quickly each character appears.
    // A smaller number means faster typing.
    [SerializeField] private float typingSpeed = 0.03f;

    // Stores whether the current dialogue is still typing.
    private bool isTyping = false;

    // Stores the full dialogue currently being displayed.
    // This allows the player to instantly reveal the full text
    // when clicking the Next button during the typing effect.
    private string currentFullDialogue;

    // Stores the currently running typing coroutine.
    // This allows the coroutine to be stopped safely if needed.
    private Coroutine typingCoroutine;


    // ============================================================
    // TUTORIAL STATE
    // ============================================================

    // Stores the tutorial phase that is currently active.
    private TutorialPhase currentPhase = TutorialPhase.Introduction;

    // Keeps track of which dialogue message is currently displayed.
    private int currentDialogueIndex = 0;

    // Checks whether the tutorial is waiting for the player
    // to press E and interact with the computer.
    private bool waitingForComputer = false;


    // ============================================================
    // INTRODUCTION DIALOGUE
    // ============================================================

    // Stores the dialogue shown when the tutorial first begins.
    private string[] introductionDialogues =
    {
        "Welcome to Dev Life: Human in the Loop!",

        "In this game, you will manage your own indie game development studio.",

        "Your goal is to develop and release successful games while managing your resources and making important decisions.",

        "This is your workspace. Your computer is where you will begin developing your games.",

        "When you are ready, press E to use your computer and begin developing your first game."
    };


    // ============================================================
    // COMPUTER DIALOGUE
    // ============================================================

    // Stores dialogue shown after the player enters the computer.
    private string[] computerDialogues =
    {
        "Great! You are now ready to begin developing your first game.",

        "Every game begins with an idea.",

        "First, you will need to choose a theme and genre for your game."
    };


    // ============================================================
    // UNITY METHODS
    // ============================================================

    // Start is called once when the Tutorial Scene begins.
    private void Start()
    {
        // Begin the tutorial with the introduction.
        StartPhase(TutorialPhase.Introduction);
    }


    // Update is called once every frame.
    // It checks for player actions required by the tutorial.
    private void Update()
    {
        // Only check for the E key while the tutorial is waiting
        // for the player to interact with the computer.
        if (waitingForComputer)
        {
            // Keyboard.current accesses the keyboard using Unity's
            // new Input System.
            if (Keyboard.current != null &&
                Keyboard.current.eKey.wasPressedThisFrame)
            {
                // The player pressed E, so enter the computer.
                EnterComputer();
            }
        }
    }


    // ============================================================
    // PHASE MANAGEMENT
    // ============================================================

    // Starts a new tutorial phase.
    private void StartPhase(TutorialPhase newPhase)
    {
        // Store the new phase.
        currentPhase = newPhase;

        // Reset the dialogue index whenever a new dialogue
        // section begins.
        currentDialogueIndex = 0;

        // Handle the behaviour for each tutorial phase.
        switch (currentPhase)
        {
            case TutorialPhase.Introduction:

                if (nextButton != null)
                {
                    nextButton.SetActive(true);
                }

                ShowCurrentDialogue();
                break;


            case TutorialPhase.EnterComputer:

                WaitForComputer();
                break;


            case TutorialPhase.ComputerDialogue:

                if (nextButton != null)
                {
                    nextButton.SetActive(true);
                }

                ShowCurrentDialogue();
                break;


            case TutorialPhase.ThemeAndGenre:

                ShowThemeAndGenreInstruction();
                break;


            case TutorialPhase.StartDevelopment:

                ShowStartDevelopmentInstruction();
                break;


            case TutorialPhase.Development:

                ShowDevelopmentInstruction();
                break;


            case TutorialPhase.Shop:

                ShowShopInstruction();
                break;


            case TutorialPhase.Complete:

                CompleteTutorial();
                break;
        }
    }


    // ============================================================
    // DIALOGUE DISPLAY
    // ============================================================

    // Displays dialogue based on the current tutorial phase.
    private void ShowCurrentDialogue()
    {
        // Stop if the dialogue text reference has not been assigned.
        if (dialogueText == null)
        {
            return;
        }

        // Determine which dialogue should be displayed.
        switch (currentPhase)
        {
            case TutorialPhase.Introduction:

                StartTyping(
                    introductionDialogues[currentDialogueIndex]
                );
                break;


            case TutorialPhase.ComputerDialogue:

                StartTyping(
                    computerDialogues[currentDialogueIndex]
                );
                break;
        }
    }


    // ============================================================
    // TYPING EFFECT
    // ============================================================

    // Starts the typing effect for a new dialogue message.
    private void StartTyping(string dialogue)
    {
        // Stop the previous typing coroutine if one is still running.
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // Store the complete dialogue so it can be revealed instantly.
        currentFullDialogue = dialogue;

        // Start the typing animation.
        typingCoroutine = StartCoroutine(TypeDialogue(dialogue));
    }


    // Types the dialogue one character at a time.
    private IEnumerator TypeDialogue(string dialogue)
    {
        // Tell the script that text is currently typing.
        isTyping = true;

        // Clear the dialogue box before typing begins.
        dialogueText.text = "";

        // Go through every character in the dialogue.
        foreach (char character in dialogue)
        {
            // Add the next character to the dialogue box.
            dialogueText.text += character;

            // Wait before displaying the next character.
            yield return new WaitForSeconds(typingSpeed);
        }

        // The full dialogue has finished typing.
        isTyping = false;

        // Clear the coroutine reference because typing is finished.
        typingCoroutine = null;
    }


    // Instantly displays the complete current dialogue.
    private void CompleteCurrentDialogue()
    {
        // Stop the typing coroutine if it is still running.
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        // Display the complete dialogue immediately.
        if (dialogueText != null)
        {
            dialogueText.text = currentFullDialogue;
        }

        // The dialogue is no longer typing.
        isTyping = false;
    }


    // ============================================================
    // NEXT BUTTON
    // ============================================================

    // This method is connected to the Next button.
    // If dialogue is currently typing, the first click completes it.
    // If dialogue has already finished, the next click progresses.
    public void NextDialogue()
    {
        // If the current dialogue is still typing,
        // instantly show the complete sentence instead of progressing.
        if (isTyping)
        {
            CompleteCurrentDialogue();
            return;
        }

        // Move through the tutorial depending on the current phase.
        switch (currentPhase)
        {
            case TutorialPhase.Introduction:

                NextIntroductionDialogue();
                break;


            case TutorialPhase.ComputerDialogue:

                NextComputerDialogue();
                break;


            // These phases will eventually progress through
            // real player actions instead of the Next button.
            case TutorialPhase.ThemeAndGenre:
            case TutorialPhase.StartDevelopment:
            case TutorialPhase.Development:
            case TutorialPhase.Shop:
                break;
        }
    }


    // ============================================================
    // INTRODUCTION PROGRESSION
    // ============================================================

    // Moves to the next introduction dialogue.
    private void NextIntroductionDialogue()
    {
        // Move to the next dialogue message.
        currentDialogueIndex++;

        // Check whether more introduction dialogue remains.
        if (currentDialogueIndex < introductionDialogues.Length)
        {
            ShowCurrentDialogue();
        }
        else
        {
            // The introduction is complete.
            // Move to the computer interaction phase.
            StartPhase(TutorialPhase.EnterComputer);
        }
    }


    // ============================================================
    // COMPUTER DIALOGUE PROGRESSION
    // ============================================================

    // Moves through dialogue shown after entering the computer.
    private void NextComputerDialogue()
    {
        // Move to the next computer dialogue.
        currentDialogueIndex++;

        // Check whether more computer dialogue remains.
        if (currentDialogueIndex < computerDialogues.Length)
        {
            ShowCurrentDialogue();
        }
        else
        {
            // Computer dialogue is complete.
            // Move to the Theme and Genre phase.
            StartPhase(TutorialPhase.ThemeAndGenre);
        }
    }


    // ============================================================
    // COMPUTER INTERACTION
    // ============================================================

    // Sets up the tutorial to wait for the player to press E.
    private void WaitForComputer()
    {
        // Tell the tutorial that it is waiting for computer input.
        waitingForComputer = true;

        // Hide the Next button because the player must now
        // perform an action instead of clicking Next.
        if (nextButton != null)
        {
            nextButton.SetActive(false);
        }
        // Show the interaction prompt to tell the player
        // that they need to press E to use the computer.
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(true);
        }
    }


    // Called when the player presses E during the computer step.
    private void EnterComputer()
    {
        // Stop waiting so the interaction cannot trigger repeatedly.
        waitingForComputer = false;

        // Hide the interaction prompt because the player
        // has successfully entered the computer.
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
        // Temporary debug message used to confirm the interaction works.
        Debug.Log("Player entered the computer!");

        // Future zoom-in animation can be triggered here.
        //
        // Example:
        // StartComputerZoomAnimation();

        // The development screen can also be opened here later.
        //
        // Example:
        // developmentScreen.SetActive(true);

        // Move to the dialogue shown after entering the computer.
        StartPhase(TutorialPhase.ComputerDialogue);
    }


    // ============================================================
    // PLACEHOLDER PHASES
    // ============================================================

    // Displays instructions for choosing a Theme and Genre.
    private void ShowThemeAndGenreInstruction()
    {
        if (dialogueText != null)
        {
            StartTyping(
                "Choose a theme and genre for your game."
            );
        }

        // Hide Next because this phase should eventually wait
        // for the player to make their selections.
        if (nextButton != null)
        {
            nextButton.SetActive(false);
        }

        Debug.Log("Waiting for Theme and Genre selection...");
    }


    // Displays instructions for starting development.
    private void ShowStartDevelopmentInstruction()
    {
        if (dialogueText != null)
        {
            StartTyping(
                "Great! Now click Start Development to begin creating your game."
            );
        }

        Debug.Log("Waiting for Start Development...");
    }


    // Displays a placeholder for the development process.
    private void ShowDevelopmentInstruction()
    {
        if (dialogueText != null)
        {
            StartTyping(
                "Your game is now entering development."
            );
        }

        // The future development animation can be connected here.
        Debug.Log("Development phase started.");
    }


    // Displays instructions for the Shop and upgrades.
    private void ShowShopInstruction()
    {
        if (dialogueText != null)
        {
            StartTyping(
                "As you develop and release games, you can earn money."
            );
        }

        Debug.Log("Shop tutorial phase started.");
    }


    // ============================================================
    // TUTORIAL COMPLETION
    // ============================================================

    // Called when the tutorial has been completed.
    private void CompleteTutorial()
    {
        if (dialogueText != null)
        {
            StartTyping(
                "Tutorial complete! You are now ready to begin your journey as a game developer."
            );
        }

        Debug.Log("Tutorial completed!");

        // Once the final gameplay scene is available,
        // this can load the normal gameplay scene.
        //
        // SceneManager.LoadScene("GameplayScene");
    }


    // ============================================================
    // SKIP TUTORIAL
    // ============================================================

    // This method is connected to the Skip Tutorial button.
    public void SkipTutorial()
    {
        // The final gameplay scene is not available yet.
        // For now, this confirms that the Skip button works.
        Debug.Log("Tutorial skipped!");

        // Once the correct gameplay scene name is known:
        //
        // SceneManager.LoadScene("GameplayScene");
    }
}