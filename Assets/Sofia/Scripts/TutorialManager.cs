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
    // This checks whether the player presses E while the tutorial
    // is waiting for the computer interaction.
    private void Update()
    {
        // Only check for the E key if the tutorial is currently
        // waiting for the player to interact with the computer.
        if (waitingForComputer)
        {
            // Keyboard.current accesses the keyboard using
            // Unity's New Input System.
            //
            // The null check prevents errors if a keyboard
            // is not detected by the game.
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
            // Display the next tutorial dialogue.
            ShowCurrentDialogue();
        }
        else
        {
            // All introduction dialogues have been shown.
            // The tutorial will now wait for the player
            // to press E before entering the main game.
            WaitForComputer();
        }
    }


    // Sets up the tutorial to wait for the player
    // to press E before continuing.
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
    // during the computer interaction part of the tutorial.
    private void EnterComputer()
    {
        // Stop waiting for the E key.
        // This prevents the interaction from being triggered
        // repeatedly while the scene is changing.
        waitingForComputer = false;

        // Display a message in the Console so we can confirm
        // that the tutorial was successfully completed.
        Debug.Log("Tutorial complete. Entering the computer.");

        // Load the main gameplay/development scene.
        //
        // The current development scene is named "Minjae's Scene".
        // This scene contains the player's room, computer and
        // DevelopmentFlowManager.
        SceneManager.LoadScene("Minjae's Scene");
    }


    // This method is connected to the Skip Tutorial button.
    // Skipping the tutorial takes the player directly to
    // the main gameplay/development scene.
    public void SkipTutorial()
    {
        // Display a message in the Console so we can confirm
        // that the tutorial was skipped successfully.
        Debug.Log("Tutorial skipped. Loading the game scene.");

        // Load the main gameplay/development scene.
        SceneManager.LoadScene("Minjae's Scene");
    }
}