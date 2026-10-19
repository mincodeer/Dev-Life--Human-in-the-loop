
using UnityEngine;
using UnityEngine.UI;

public class DeveloperStatusTabs : MonoBehaviour
{
    // Panels for each tab
    public GameObject developerStatusPanel;
    public GameObject todoPanel;

    // Buttons used to switch between tabs
    public Button statusButton;
    public Button todoButton;

    // PNG images displayed on the buttons
    public Image statusButtonImage;
    public Image todoButtonImage;

    // Tracks which tab is currently selected
    private bool showingStatus = true;

    void Start()
    {
        // Connect the buttons to their functions
        statusButton.onClick.AddListener(ShowStatus);
        todoButton.onClick.AddListener(ShowTodo);

        // Start with the Developer Status tab selected
        // Only update the images here so the minimised UI stays hidden
        UpdateButtonImages();
    }

    public void ShowStatus()
    {
        // Remember that the Developer Status tab is selected
        showingStatus = true;

        // Display the selected panel
        RestoreSelectedTab();
    }

    public void ShowTodo()
    {
        // Remember that the To-Do List tab is selected
        showingStatus = false;

        // Display the selected panel
        RestoreSelectedTab();
    }

    public void RestoreSelectedTab()
    {
        // Show only the currently selected panel
        developerStatusPanel.SetActive(showingStatus);
        todoPanel.SetActive(!showingStatus);

        // Update the PNG images to match the selected tab
        UpdateButtonImages();
    }

    void UpdateButtonImages()
    {
        // Show only the selected tab's PNG
        statusButtonImage.enabled = showingStatus;
        todoButtonImage.enabled = !showingStatus;
    }
}
