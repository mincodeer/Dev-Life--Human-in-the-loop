using UnityEngine;
using UnityEngine.UI;

public class DevelopmentProgress : MonoBehaviour
{
    public Slider progressBar;
    public GameObject completeText;

    private float progress = 0f;
    void Start()
    {
        progress = 0f;
        progressBar.value = progress;
        progressBar.gameObject.SetActive(false);
        completeText.SetActive(false);
    }
    public void AddProgress()
    {
        progressBar.gameObject.SetActive(true);
        progress += 10f;
        progressBar.value = progress;
        if (progress >= 100f)
        {
            progress = 100f;
            progressBar.value = 100f;
            completeText.SetActive(true);
            Debug.Log("Development Complete");
        }
    }
}