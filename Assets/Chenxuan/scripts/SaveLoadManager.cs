using UnityEngine;
using UnityEngine.UI;

public class SaveLoadManager : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;
    // Save the current volume settings
    public void SaveGame()
    {
        PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);

        PlayerPrefs.Save();

        Debug.Log("Game Saved");
    }
    // Load the saved volume settings
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume");
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        }

        Debug.Log("Game Loaded");
    }
}
