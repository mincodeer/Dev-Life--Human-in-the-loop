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

    private void Start()
    {
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", bgmSlider.value);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", sfxSlider.value);

        bgmSlider.SetValueWithoutNotify(bgmVolume);
        sfxSlider.SetValueWithoutNotify(sfxVolume);

        bgmSource.volume = bgmVolume;
        sfxSource.volume = sfxVolume;
    }

    public void OpenSettings()
    {
        settingspanel.SetActive(true);

        if (mainButtons != null)
            mainButtons.SetActive(false);
    }

    public void CloseSettings()
    {
        settingspanel.SetActive(false);

        if (mainButtons != null)
            mainButtons.SetActive(true);
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;

        PlayerPrefs.SetFloat("BGMVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;

        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }
}