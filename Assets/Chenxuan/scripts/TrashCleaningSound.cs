using UnityEngine;

public class TrashCleaningSound : MonoBehaviour
{
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip cleaningSound;

    public void PlayCleaningSound()
    {
        if (sfxSource == null || cleaningSound == null)
            return;

        if (!sfxSource.isActiveAndEnabled)
            return;

        sfxSource.PlayOneShot(cleaningSound);
    }
}