using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SaveLoadManager : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Slider keyboardSlider;
    public TMP_Text saveMessage;

    private GameTimeManager gameTimeManager;

    void Start()
    {
        gameTimeManager = FindFirstObjectByType<GameTimeManager>();

        if (saveMessage != null)
        {
            saveMessage.gameObject.SetActive(false);
        }
    }

    public void SaveGame()
    {
        // Save volume settings
        PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.SetFloat("KeyboardVolume", keyboardSlider.value);

        // Save game time
        if (gameTimeManager == null)
        {
            gameTimeManager = FindFirstObjectByType<GameTimeManager>();
        }

        if (gameTimeManager != null)
        {
            PlayerPrefs.SetInt(
                "CurrentDay",
                gameTimeManager.currentDay);

            PlayerPrefs.SetInt(
                "CurrentHour",
                gameTimeManager.currentHour);

            PlayerPrefs.SetInt(
                "CurrentMinute",
                gameTimeManager.currentMinute);
        }

        // Save resources
        if (ResourceManager.Instance != null)
        {
            PlayerPrefs.SetInt(
                "Money",
                ResourceManager.Instance.Money);

            PlayerPrefs.SetInt(
                "Fandom",
                ResourceManager.Instance.Fandom);

            PlayerPrefs.SetInt(
                "DevelopmentSkill",
                ResourceManager.Instance.DevelopmentSkill);

            PlayerPrefs.SetInt(
                "TechnicalDebt",
                ResourceManager.Instance.TechnicalDebt);
        }

        PlayerPrefs.Save();

        // Show save message
        if (saveMessage != null)
        {
            saveMessage.text = "Game Saved";
            saveMessage.gameObject.SetActive(true);

            StopCoroutine(nameof(HideSaveMessage));
            StartCoroutine(HideSaveMessage());
        }

        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        // Load volume settings
        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            bgmSlider.value =
                PlayerPrefs.GetFloat("BGMVolume");

            sfxSlider.value =
                PlayerPrefs.GetFloat("SFXVolume");

            keyboardSlider.value =
                PlayerPrefs.GetFloat("KeyboardVolume");
        }

        // Load game time
        if (gameTimeManager == null)
        {
            gameTimeManager = FindFirstObjectByType<GameTimeManager>();
        }

        if (gameTimeManager != null &&
            PlayerPrefs.HasKey("CurrentDay"))
        {
            gameTimeManager.currentDay =
                PlayerPrefs.GetInt("CurrentDay");

            gameTimeManager.currentHour =
                PlayerPrefs.GetInt("CurrentHour");

            gameTimeManager.currentMinute =
                PlayerPrefs.GetInt("CurrentMinute");
        }

        // Load resources
        if (ResourceManager.Instance != null)
        {
            int savedMoney =
                PlayerPrefs.GetInt(
                    "Money",
                    ResourceManager.Instance.Money);

            int savedFandom =
                PlayerPrefs.GetInt(
                    "Fandom",
                    ResourceManager.Instance.Fandom);

            int savedDevelopmentSkill =
                PlayerPrefs.GetInt(
                    "DevelopmentSkill",
                    ResourceManager.Instance.DevelopmentSkill);

            int savedTechnicalDebt =
                PlayerPrefs.GetInt(
                    "TechnicalDebt",
                    ResourceManager.Instance.TechnicalDebt);

            ResourceManager.Instance.ChangeMoney(
                savedMoney -
                ResourceManager.Instance.Money);

            ResourceManager.Instance.ChangeFandom(
                savedFandom -
                ResourceManager.Instance.Fandom);

            ResourceManager.Instance.ChangeDevelopmentSkill(
                savedDevelopmentSkill -
                ResourceManager.Instance.DevelopmentSkill);

            ResourceManager.Instance.ChangeTechnicalDebt(
                savedTechnicalDebt -
                ResourceManager.Instance.TechnicalDebt);
        }

        Debug.Log("Game Loaded");
    }

    private IEnumerator HideSaveMessage()
    {
        yield return new WaitForSecondsRealtime(2f);

        if (saveMessage != null)
        {
            saveMessage.gameObject.SetActive(false);
        }
    }
}