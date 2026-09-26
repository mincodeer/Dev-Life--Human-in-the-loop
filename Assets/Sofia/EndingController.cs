using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndingController : MonoBehaviour
{
    // These are the UI elements that will display the ending information
    public GameObject endingPanel;
    public TMP_Text endingTitle;
    public TMP_Text endingDescription;
    public TMP_Text endingStats;

    // These are the buttons used after the player reaches an ending
    public Button restartButton;
    public Button mainMenuButton;

    private void Start()
    {
        // Keep the ending screen hidden when the game first starts
        endingPanel.SetActive(false);

        // Connect the buttons to their functions
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(MainMenu);
    }

    public void ShowEnding(string title, string description, string stats)
    {
        // Show the ending screen when an ending condition is reached
        endingPanel.SetActive(true);

        // Change the text depending on which ending the player reached
        endingTitle.text = title;
        endingDescription.text = description;
        endingStats.text = stats;
    }

    public void RestartGame()
    {
        // Reload the current scene to start the game again
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        // This will be connected to our Main Menu later
        Debug.Log("Main Menu button pressed.");
    }
}