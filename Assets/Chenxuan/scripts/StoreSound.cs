using UnityEngine;

public class StoreSound : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioClip successSound;
    public AudioClip failSound;

    private int moneyBefore;

    public void RecordMoney()
    {
        if (ResourceManager.Instance != null)
        {
            moneyBefore = ResourceManager.Instance.Money;
        }
    }

    public void CheckPurchase()
    {
        if (ResourceManager.Instance == null)
            return;

        int moneyAfter = ResourceManager.Instance.Money;

        if (moneyAfter < moneyBefore)
        {
            PlaySound(successSound);
        }
        else
        {
            PlaySound(failSound);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
            return;

        if (!sfxSource.gameObject.activeInHierarchy)
            return;

        if (!sfxSource.enabled)
            sfxSource.enabled = true;

        sfxSource.PlayOneShot(clip);
    }
}
