using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    public GameObject settingspanel;
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public void OpenSettings()
    { settingspanel.SetActive(true); }
    public void CloseSettings()
    { settingspanel.SetActive(false); }
    public void SetBGMVolume(float volume)
    { bgmSource.volume = volume;
    }
public void SetSFXVolume(float volume)
{
    sfxSource.volume = volume;
}
}
