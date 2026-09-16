using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyboardSound : MonoBehaviour
{
    public AudioSource keyboardSource;
    public AudioClip keyboardClip;
    public Slider keyboardSlider;
    void Start()
    {
        SetKeyboardVolume(keyboardSlider.value);
    }
    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.anyKey.wasPressedThisFrame)
        { keyboardSource.PlayOneShot(keyboardClip, keyboardSlider.value);
        }
    }
    public void SetKeyboardVolume(float volume)
    { keyboardSource.volume = volume;
    }
}
