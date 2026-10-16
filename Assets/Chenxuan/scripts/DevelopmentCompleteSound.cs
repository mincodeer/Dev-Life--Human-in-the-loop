using UnityEngine;
using UnityEngine.UI;

public class DevelopmentCompleteSound : MonoBehaviour
{
    public Slider progressBar;
    public AudioSource sfxSource;
    public AudioClip completeSound;

    private bool hasPlayed = false;

    void Update()
    {
        if (progressBar == null || sfxSource == null || completeSound == null)
            return;

        if (progressBar.value >= 100f && !hasPlayed)
        {
            sfxSource.PlayOneShot(completeSound);
            hasPlayed = true;
        }

        if (progressBar.value < 100f)
        {
            hasPlayed = false;
        }
    }
}
