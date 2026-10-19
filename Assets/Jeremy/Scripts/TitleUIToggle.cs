
using UnityEngine;

public class TitleUIToggle : MonoBehaviour
{
    // Main Developer Status panel
    public GameObject expandedTitleUI;

    // Minimised Developer Status panel
    public GameObject minimisedTitleUI;

    // To-Do List panel
    public GameObject todoPanel;

    // Contains both DEV. and TO-DO buttons
    public GameObject tabButtons;

    // Reference to the tab switching controller
    public DeveloperStatusTabs tabController;

    private void Start()
    {
        // Start with the minimised display
        ShowMinimised();
    }

    public void ShowExpanded()
    {
        // Hide the minimised display
        minimisedTitleUI.SetActive(false);

        // Show both tab buttons
        tabButtons.SetActive(true);

        // Restore the previously selected tab
        if (tabController != null)
        {
            tabController.RestoreSelectedTab();
        }
        else
        {
            expandedTitleUI.SetActive(true);
            todoPanel.SetActive(false);
        }
    }

    public void ShowMinimised()
    {
        // Hide both maximised panels
        expandedTitleUI.SetActive(false);
        todoPanel.SetActive(false);

        // Hide both DEV. and TO-DO buttons
        tabButtons.SetActive(false);

        // Show the minimised display
        minimisedTitleUI.SetActive(true);
    }
}
