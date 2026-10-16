using UnityEngine;

public class BuildSoundEffects : MonoBehaviour
{
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip buildingSound;

    private AudioSource buildLoopSource;

    private void Awake()
    {
        buildLoopSource = gameObject.AddComponent<AudioSource>();

        buildLoopSource.playOnAwake = false;
        buildLoopSource.loop = true;
        buildLoopSource.spatialBlend = 0f;
    }

    public void PlayBuildSound()
    {
        if (buildingSound == null)
            return;

        if (buildLoopSource.isPlaying)
            return;

        buildLoopSource.clip = buildingSound;
        buildLoopSource.volume = sfxSource != null ? sfxSource.volume : 1f;
        buildLoopSource.mute = sfxSource != null && sfxSource.mute;

        buildLoopSource.Play();
    }

    private void OnDisable()
    {
        if (buildLoopSource != null)
            buildLoopSource.Stop();
    }
}