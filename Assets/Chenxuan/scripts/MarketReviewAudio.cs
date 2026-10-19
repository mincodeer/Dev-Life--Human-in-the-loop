using UnityEngine;

public class MarketReviewAudio : MonoBehaviour
{
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip marketBGM;
    [SerializeField] private AudioClip reviewBGM;

    [SerializeField] private AudioClip marketClickSound;
    [SerializeField] private AudioClip reviewClickSound;

    private AudioClip originalBGM;
    private float originalTime;
    private bool originalWasPlaying;
    private bool isSpecialBGMPlaying;

    private void Start()
    {
        if (bgmSource != null)
        {
            originalBGM = bgmSource.clip;
        }
    }

    public void OpenMarket()
    {
        PlaySFX(marketClickSound);
        SwitchBGM(marketBGM);
    }

    public void OpenReview()
    {
        PlaySFX(reviewClickSound);
        SwitchBGM(reviewBGM);
    }

    public void RestoreBGM()
    {
        if (bgmSource == null || !isSpecialBGMPlaying)
            return;

        bgmSource.Stop();
        bgmSource.clip = originalBGM;

        if (originalBGM != null && originalWasPlaying)
        {
            bgmSource.time = originalTime;
            bgmSource.Play();
        }

        isSpecialBGMPlaying = false;
    }

    private void SwitchBGM(AudioClip newBGM)
    {
        if (bgmSource == null || newBGM == null)
            return;

        if (!isSpecialBGMPlaying)
        {
            originalBGM = bgmSource.clip;
            originalTime = bgmSource.time;
            originalWasPlaying = bgmSource.isPlaying;
        }

        bgmSource.Stop();
        bgmSource.clip = newBGM;
        bgmSource.loop = true;
        bgmSource.Play();

        isSpecialBGMPlaying = true;
    }

    private void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}