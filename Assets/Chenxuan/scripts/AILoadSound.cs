using UnityEngine;

public class AILoadingSound : MonoBehaviour
{
    public AudioSource loadingSource;
    public AudioClip loadingSound;
    public DevelopmentFlowManager flowManager;

    private bool isLoading;
    private DevelopmentStage loadingStage;

    public void StartLoadingSound()
    {
        if (loadingSource == null ||
            loadingSound == null ||
            flowManager == null)
            return;

        loadingStage = flowManager.CurrentStage;

        loadingSource.Stop();
        loadingSource.clip = loadingSound;
        loadingSource.loop = true;
        loadingSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        loadingSource.Play();

        isLoading = true;
    }

    private void Update()
    {
        if (!isLoading)
            return;

        loadingSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        if (flowManager.CurrentStage != loadingStage ||
            !flowManager.IsComputerUIOpen)
        {
            StopLoadingSound();
        }
    }

    public void StopLoadingSound()
    {
        if (loadingSource != null)
        {
            loadingSource.Stop();
            loadingSource.loop = false;
        }

        isLoading = false;
    }
}