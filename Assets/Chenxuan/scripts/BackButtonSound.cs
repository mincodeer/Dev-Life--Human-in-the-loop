using UnityEngine;

public class BackButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;

    public void PlayBackSound()
    {
        if (clickSound == null)
            return;

        float volume = Mathf.Clamp01(
            PlayerPrefs.GetFloat("SFXVolume", 1f)
        );

        if (volume <= 0.001f)
            return;

        GameObject soundObject = new GameObject("BackButtonAudio");
        AudioSource source = soundObject.AddComponent<AudioSource>();

        source.playOnAwake = false;
        source.spatialBlend = 0f;
        source.volume = volume;
        source.clip = clickSound;

        DontDestroyOnLoad(soundObject);

        source.Play();

        Destroy(soundObject, clickSound.length + 0.1f);
    }
}