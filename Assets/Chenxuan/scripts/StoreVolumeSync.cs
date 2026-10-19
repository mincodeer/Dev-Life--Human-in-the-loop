using UnityEngine;

public class StoreVolumeSync : MonoBehaviour
{
    [SerializeField] private AudioSource sfxSource;

    private void Start()
    {
        ApplyVolume();
    }

    private void Update()
    {
        ApplyVolume();
    }

    private void ApplyVolume()
    {
        if (sfxSource == null)
            return;

        float volume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        sfxSource.volume = Mathf.Clamp01(volume);
    }
}