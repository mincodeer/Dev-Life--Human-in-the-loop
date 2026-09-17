using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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
            StartCoroutine(ShowCompleteText());
            Debug.Log("Development Complete");
        }
    }IEnumerator ShowCompleteText()
    {
        completeText.SetActive(true);
        yield return new WaitForSeconds(2f);
        completeText.SetActive(false);
    }
}