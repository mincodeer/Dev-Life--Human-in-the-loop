using UnityEngine;

public class QuitWarning : MonoBehaviour
{
    public GameObject quitWarningPanel;
    public GameObject mainButtons;

    public void ShowQuitWarning()
    {
        quitWarningPanel.SetActive(true);
        mainButtons.SetActive(false);
    }

    public void CancelQuit()
    {
        quitWarningPanel.SetActive(false);
        mainButtons.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}