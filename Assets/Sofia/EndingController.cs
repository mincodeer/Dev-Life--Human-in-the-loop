// Student ID: 23208000

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndingController : MonoBehaviour
{
    // These are the UI elements that display the ending information.
    public GameObject endingPanel;
    public TMP_Text endingTitle;
    public TMP_Text endingDescription;
    public TMP_Text endingStats;

    // Image shown for the current ending.
    public Image endingImage;

    // Background image shown for the current ending.
    public Image endingBackground;

    // These are the buttons used after the player reaches an ending.
    public Button restartButton;
    public Button mainMenuButton;

    private void Start()
    {
        // Keep the ending screen hidden when the game first starts.
        endingPanel.SetActive(false);

        // Connect the buttons to their functions.
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(MainMenu);
    }

    /// <summary>
    /// Shows an ending using the information stored in an EndingData asset.
    /// </summary>
    public void ShowEnding(EndingData endingData)
    {
        if (endingData == null)
        {
            Debug.LogWarning(
                "EndingData is missing.");

            return;
        }

        // Show the ending screen.
        endingPanel.SetActive(true);

        // Update the ending text.
        endingTitle.text =
            endingData.endingTitle;

        // Change the title colour for this specific ending.
        endingTitle.color =
            endingData.titleColor;

        endingDescription.text =
            endingData.endingDescription;

        endingStats.text =
            endingData.endingStats;

        // Update the ending image if one has been assigned.
        if (endingImage != null
            && endingData.endingImage != null)
        {
            endingImage.sprite =
                endingData.endingImage;

            endingImage.gameObject.SetActive(true);
        }

        // Update the background if one has been assigned.
        if (endingBackground != null
            && endingData.endingBackground != null)
        {
            endingBackground.sprite =
                endingData.endingBackground;
        }
    }

    /// <summary>
    /// Shows an ending using text supplied directly by another system.
    /// This keeps the existing ending systems working.
    /// </summary>
    public void ShowEnding(
        string title,
        string description,
        string stats)
    {
        // Show the ending screen.
        endingPanel.SetActive(true);

        // Display the supplied ending information.
        endingTitle.text = title;
        endingDescription.text = description;
        endingStats.text = stats;
    }


    public void RestartGame()
    {
        // Reset permanent player resources.
        if (ResourceManager.Instance != null)
        {
            ResourceManager.Instance.ResetResources();
        }

        // Reset the current project and AI project count.
        if (ProjectDataManager.Instance != null)
        {
            ProjectDataManager.Instance.ResetCurrentProject();
            ProjectDataManager.Instance.ResetAIProjectCount();
        }

        // Reload the current scene.
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        // This will be connected to our Main Menu later.
        Debug.Log(
            "Main Menu button pressed.");
    }
}