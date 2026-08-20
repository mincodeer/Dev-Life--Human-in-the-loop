using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    // Reference to the TextMeshPro text object that displays
    // the current tutorial dialogue on the screen.
    public TextMeshProUGUI dialogueText;

    // Reference to the Next button.
    // This allows the script to hide the button when the player
    // needs to perform an action instead of clicking Next.
    public GameObject nextButton;


    // Stores all of the introduction dialogue messages.
    // Each message is displayed one at a time when the player
    // clicks the Next button.
    private string[] introductionDialogues =
    {
        "Welcome to Dev Life: Human in the Loop!",

        "In this game, you will manage your own indie game development studio.",

        "Your goal is to develop and release successful games while managing your resources and making important decisions.",

        "This is your workspace. Your computer is where you will begin developing your games.",

        "When you are ready, press E to use your computer and begin developing your first game."
    };


    // Keeps track of which dialogue message is currently being displayed.
    // The first dialogue starts at position 0 in the array.
    private int currentDialogueIndex = 0;


    // Checks whether the tutorial is currently waiting for the player
    // to press E and interact with the computer.
    private bool waitingForComputer = false;


    // Start is called once when the Tutorial Scene begins.
    private void Start()
    {
        // Display the first tutorial dialogue when the scene starts.
        ShowCurrentDialogue();
    }


    // Update is called once every frame.
    // This is used to check whether the player presses E
    // while the tutorial is waiting for computer interaction.
    private void Update()
    {
        // Only check for the E key if the tutorial is currently
        // waiting for the player to interact with the computer.
        if (waitingForComputer)
        {
            // Keyboard.current accesses the keyboard using Unity's
            // new Input System.
            //
            // The null check prevents errors if no keyboard is detected.
            if (Keyboard.current != null &&
                Keyboard.current.eKey.wasPressedThisFrame)
            {
                // The player pressed E, so continue to the
                // computer interaction step.
                EnterComputer();
            }
        }
    }


    // Displays the dialogue at the current position
    // in the introductionDialogues array.
    private void ShowCurrentDialogue()
    {
        // Check that the Dialogue Text object has been assigned
        // in the Unity Inspector before changing its text.
        if (dialogueText != null)
        {
            dialogueText.text =
                introductionDialogues[currentDialogueIndex];
        }
    }


    // This method is connected to the Next button.
    // Every time the player clicks Next, the tutorial
    // moves to the next dialogue message.
    public void NextDialogue()
    {
        // Move to the next dialogue message.
        currentDialogueIndex++;

        // Check if there are still dialogue messages remaining.
        if (currentDialogueIndex < introductionDialogues.Length)
        {
            // Display the next dialogue message.
            ShowCurrentDialogue();
        }
        else
        {
            // All introduction dialogues have been shown.
            // The tutorial will now wait for the player
            // to press E to interact with the computer.
            WaitForComputer();
        }
    }


    // Sets up the tutorial to wait for the player
    // to interact with the computer.
    private void WaitForComputer()
    {
        // Tell the script that the tutorial is now waiting
        // for the computer interaction.
        waitingForComputer = true;

        // Hide the Next button because the player now needs
        // to press E instead of clicking Next.
        if (nextButton != null)
        {
            nextButton.SetActive(false);
        }
    }


    // This method is called when the player presses E
    // during the computer interaction step.
    private void EnterComputer()
    {
        // Stop waiting for the computer interaction.
        // This prevents the interaction from triggering repeatedly.
        waitingForComputer = false;

        // Temporary message shown in the Unity Console.
        // This can be useful for testing and debugging.
        Debug.Log("Player entered the computer!");

        // This is where the future zoom-in animation can be triggered.
        //
        // Example:
        // StartComputerZoomAnimation();

        // This is where the future development screen can be opened.
        //
        // Example:
        // developmentScreen.SetActive(true);

        // For now, update the dialogue text to confirm that
        // the player has successfully entered the computer.
        if (dialogueText != null)
        {
            dialogueText.text =
                "Great! You are now ready to begin developing your first game.";
        }

        // Show the Next button again so that we can continue
        // adding more tutorial dialogue after this interaction.
        if (nextButton != null)
        {
            nextButton.SetActive(true);
        }
    }


    // This method is connected to the Skip Tutorial button.
    public void SkipTutorial()
    {
        // The normal gameplay scene is not available yet.
        // This currently prints a message to the Console
        // to confirm that the Skip Tutorial button is working.
        Debug.Log("Tutorial skipped!");

        // Once the gameplay scene has been created and its final
        // scene name is known, we can uncomment and update this line:
        //
        // SceneManager.LoadScene("GameplayScene");
    }
}