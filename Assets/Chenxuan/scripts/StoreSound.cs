using UnityEngine;

public class StoreSound : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioClip successSound;
    public AudioClip failSound;

    private int moneyBefore;

    public void RecordMoney()
    {
        moneyBefore = GetMoney();
    }

    public void CheckPurchase()
    {
        PlaySound(successSound);
    }

    private int GetMoney()
    {
        return 0;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
            return;

        if (!sfxSource.gameObject.activeInHierarchy)
            sfxSource.gameObject.SetActive(true);

        if (!sfxSource.enabled)
            sfxSource.enabled = true;

        sfxSource.PlayOneShot(clip);
    }
}
