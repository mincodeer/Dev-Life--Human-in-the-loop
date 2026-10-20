using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardSound : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioClip keyboardClip;
    public float keyboardVolume = 0.5f;

    private void Start()
    {
        keyboardVolume = PlayerPrefs.GetFloat("KeyboardVolume", keyboardVolume);
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (!Keyboard.current.anyKey.wasPressedThisFrame)
            return;

        if (sfxSource == null || keyboardClip == null)
            return;

        if (keyboardVolume <= 0f)
            return;

        sfxSource.PlayOneShot(keyboardClip, keyboardVolume);
    }

    public void SetKeyboardVolume(float volume)
    {
        keyboardVolume = Mathf.Clamp01(volume);

        PlayerPrefs.SetFloat("KeyboardVolume", keyboardVolume);
        PlayerPrefs.Save();
    }
}