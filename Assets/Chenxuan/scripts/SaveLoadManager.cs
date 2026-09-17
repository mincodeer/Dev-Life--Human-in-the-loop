using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class SaveLoadManager : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Slider KeyboardSlider;
    public GameObject saveMessage;
    // Save the current volume settings
    public void SaveGame()
    {
        PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.SetFloat("keyboardVolume", KeyboardSlider.value);
       
        PlayerPrefs.Save();

        Debug.Log("Game Saved");
        StartCoroutine(ShowSaveMessage());
    }
    IEnumerator ShowSaveMessage()
    { saveMessage.SetActive(true);
        yield return new WaitForSeconds(2f);
        saveMessage.SetActive(false);
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

        if (PlayerPrefs.HasKey("keyboardVolume"))
        {
            KeyboardSlider.value = PlayerPrefs.GetFloat("keyboardVolume");
        }
        Debug.Log("Game Loaded");
    }
}
