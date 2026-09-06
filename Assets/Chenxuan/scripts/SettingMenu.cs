using UnityEngine;
using UnityEngine.UI;
public class SettingMenu : MonoBehaviour
{
    public GameObject settingspanel;
    public GameObject mainButtons;
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public Slider bgmSlider;
    public Slider sfxSlider;

    void Start()
    {
        bgmSource.volume = bgmSlider.value;
        sfxSource.volume = sfxSlider.value;
    }

    public void OpenSettings()
    {
        settingspanel.SetActive(true);
        mainButtons.SetActive(false);
    }

    public void CloseSettings()
    {
        settingspanel.SetActive(false);
        mainButtons.SetActive(true);
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}