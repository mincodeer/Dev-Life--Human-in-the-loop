using UnityEngine;

public class SoundMixingSFX : MonoBehaviour
{
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip mixingSound;
    [SerializeField] private float soundInterval = 0.15f;

    private float lastPlayTime = -999f;

    public void PlayMixingSound()
    {
        if (sfxSource == null || mixingSound == null)
            return;

        if (Time.unscaledTime - lastPlayTime < soundInterval)
            return;

        sfxSource.PlayOneShot(mixingSound);
        lastPlayTime = Time.unscaledTime;
    }
}