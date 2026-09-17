using UnityEngine;

public class TitleUIToggle : MonoBehaviour
{
    public GameObject expandedTitleUI;
    public GameObject minimisedTitleUI;

    private void Start()
    {
        ShowMinimised();
    }

    public void ShowExpanded()
    {
        expandedTitleUI.SetActive(true);
        minimisedTitleUI.SetActive(false);
    }

    public void ShowMinimised()
    {
        expandedTitleUI.SetActive(false);
        minimisedTitleUI.SetActive(true);
    }
}