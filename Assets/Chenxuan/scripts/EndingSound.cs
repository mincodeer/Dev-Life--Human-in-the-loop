using UnityEngine;
using TMPro;

public class EndingSound : MonoBehaviour
{
    [Header("Ending References")]
    public GameObject endingPanel;
    public TMP_Text endingTitle;
    public AudioSource audioSource;

    [Header("Ending Sound Effects")]
    public AudioClip fandomSound;
    public AudioClip technicalDebtSound;
    public AudioClip aiTakesOverSound;
    public AudioClip cantPayRentSound;
    public AudioClip workloadSound;

    private bool wasActive;

    void Start()
    {
        wasActive = endingPanel != null && endingPanel.activeInHierarchy;

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }
    }

    void LateUpdate()
    {
        if (endingPanel == null || endingTitle == null || audioSource == null)
            return;

        bool isActive = endingPanel.activeInHierarchy;

        if (isActive && !wasActive)
        {
            PlayEndingSound();
        }

        wasActive = isActive;
    }

    void PlayEndingSound()
    {
        string title = endingTitle.text.Trim().ToUpperInvariant();
        AudioClip selectedClip = null;

        switch (title)
        {
            case "YOU HAVE BEEN REPLACED":
                selectedClip = replacedSound;
                break;

            case "YOU ACTUALLY DID IT!":
                selectedClip = successSound;
                break;

            case "CONGRATULATIONS! YOU ARE HOMELESS":
                selectedClip = homelessSound;
                break;

            case "IT WASN'T SUPPOSED TO DO THAT":
                selectedClip = unexpectedSound;
                break;

            case "YOU HAVE BECOME THE BUG":
                selectedClip = bugSound;
                break;
        }

        if (selectedClip != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(selectedClip);
        }
        else
        {
            Debug.LogWarning("No ending sound assigned for: " + title);
        }
    }
}
