using UnityEngine;

public class MarketReviewSFX : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioClip marketClickSound;
    public AudioClip reviewClickSound;

    public void PlayMarketClick()
    {
        if (sfxSource != null && marketClickSound != null)
            sfxSource.PlayOneShot(marketClickSound);
    }

    public void PlayReviewClick()
    {
        if (sfxSource != null && reviewClickSound != null)
            sfxSource.PlayOneShot(reviewClickSound);
    }
}