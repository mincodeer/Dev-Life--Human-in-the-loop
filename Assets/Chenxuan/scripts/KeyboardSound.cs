using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardSound : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioClip keyboardClip;
    public float keyboardVolume = 0.5f;

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.anyKey.wasPressedThisFrame)
        {
            if (sfxSource != null && keyboardClip != null)
            {
                sfxSource.PlayOneShot(keyboardClip, keyboardVolume);
            }
        }
    }

    public void SetKeyboardVolume(float volume)
    {
        keyboardVolume = Mathf.Clamp01(volume);
    }
}