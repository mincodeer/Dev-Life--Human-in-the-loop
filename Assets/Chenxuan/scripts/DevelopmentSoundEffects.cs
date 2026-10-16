using UnityEngine;

public class DevelopmentSoundEffects : MonoBehaviour
{
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip completionSound;
    [SerializeField] private AudioClip pencilSound;
    [SerializeField] private DevelopmentFlowManager flowManager;

    private DevelopmentStage previousStage = DevelopmentStage.None;
    private void Start()
    {
        if (flowManager != null)
            previousStage = flowManager.CurrentStage;
    }
    private void Update()
    {
        if (flowManager == null) return;

        DevelopmentStage currentStage = flowManager.CurrentStage;

        if (previousStage == DevelopmentStage.Design &&
            currentStage == DevelopmentStage.Sound)
        {
            PlayCompletionSound();
        }

        if (previousStage == DevelopmentStage.Sound &&
            currentStage == DevelopmentStage.Debugging)
        {
            PlayCompletionSound();
        }

        if (previousStage == DevelopmentStage.Debugging &&
            currentStage == DevelopmentStage.Build)
        {
            PlayCompletionSound();
        }

        if (previousStage == DevelopmentStage.Build &&
            currentStage == DevelopmentStage.Result)
        {
            PlayCompletionSound();
        }

        previousStage = currentStage;
    }

    public void PlayCompletionSound()
    {
        PlaySound(completionSound);
    }

    private float lastPencilTime = -999f;

    public void PlayPencilSound()
    {
        if (sfxSource == null || pencilSound == null)
            return;

        if (Time.unscaledTime - lastPencilTime < pencilSound.length)
            return;

        sfxSource.PlayOneShot(pencilSound);
        lastPencilTime = Time.unscaledTime;
    }

    private void PlaySound(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}