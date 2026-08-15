using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    public GameObject settingspanel;
    public void OpenSettings()
    { settingspanel.SetActive(true); }
    public void CloseSettings()
    { settingspanel.SetActive(false); }
    public void SetBGMVolume(float volume)
    { Debug.Log("BGM Volume: "+ volume); 
    }
public void SetSFXVolume(float volume)
{
    Debug.Log("SFX Volume: "+ volume);
}
}
