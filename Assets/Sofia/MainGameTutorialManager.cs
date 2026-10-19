
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainGameTutorialManager : MonoBehaviour
{
    public enum TutorialType
    {
        DayNight,
        DeveloperTitles,
        Dashboard,
        GameReview,
        Shop,
        AICosts,
        MarketTrends
    }

    [Serializable]
    public class TutorialContent
    {
        public TutorialType tutorialType;
        public string tutorialTitle;

        [TextArea(3, 6)]
        public string dialogue;
    }

    [Header("Tutorial UI")]
    public GameObject popupPanel;
    public TMP_Text titleText;
    public TMP_Text dialogueText;
    public Button closeButton;

    [Header("Tutorial Content")]
    public TutorialContent[] tutorials;

    [Header("Typing Effect")]
    public float typingSpeed = 0.03f;

    [Header("Testing")]
    [Tooltip("Enable this to reset all tutorials whenever Play Mode starts.")]
    public bool resetTutorialsOnPlayForTesting = false;

    private TutorialType currentTutorialType;
    private Coroutine typingCoroutine;
    private bool isTutorialActive;
    private bool isTyping;

    public static MainGameTutorialManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning(
                "Duplicate MainGameTutorialManager found on: "
                + gameObject.name,
                gameObject
            );

            Destroy(gameObject);
            return;
        }

        Instance = this;

        Debug.Log(
            "MainGameTutorialManager initialized on: "
            + gameObject.name,
            gameObject
        );

        // Reset tutorial completion flags when testing, if enabled.
        if (resetTutorialsOnPlayForTesting)
        {
            ResetAllTutorials();
        }

        DontDestroyOnLoad(gameObject);

        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }

        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(CloseTutorial);
            closeButton.onClick.AddListener(CloseTutorial);
        }
    }

    public void ShowTutorial(TutorialType type)
    {
        Debug.Log("Requested tutorial: " + type);

        // Prevent multiple tutorials from appearing simultaneously.
        if (isTutorialActive)
        {
            Debug.Log("A tutorial is already active.");
            return;
        }

        // Do not show tutorials that have already been completed.
        if (PlayerPrefs.GetInt(GetCompletionKey(type), 0) == 1)
        {
            Debug.Log("Tutorial already completed: " + type);
            return;
        }

        if (tutorials == null)
        {
            Debug.LogWarning("Tutorial content array is missing.");
            return;
        }

        TutorialContent content = Array.Find(
            tutorials,
            tutorial => tutorial.tutorialType == type
        );

        if (content == null)
        {
            Debug.LogWarning(
                "No tutorial content found for: " + type
            );
            return;
        }

        if (popupPanel == null || titleText == null ||
            dialogueText == null)
        {
            Debug.LogWarning(
                "Tutorial UI references are missing."
            );
            return;
        }

        currentTutorialType = type;
        isTutorialActive = true;

        popupPanel.SetActive(true);
        titleText.text = content.tutorialTitle;

        Debug.Log("Displaying tutorial: " + content.tutorialTitle);

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(
            TypeDialogue(content.dialogue)
        );
    }

    private IEnumerator TypeDialogue(string dialogue)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in dialogue)
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    public void CloseTutorial()
    {
        if (!isTutorialActive)
        {
            return;
        }

        // Finish typing immediately if the player closes during the effect.
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isTyping = false;

        // Remember that this tutorial has been completed.
        PlayerPrefs.SetInt(
            GetCompletionKey(currentTutorialType), 1
        );

        PlayerPrefs.Save();

        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }

        isTutorialActive = false;

        Debug.Log("Tutorial completed: " + currentTutorialType);
    }

    private string GetCompletionKey(TutorialType type)
    {
        return "MainGameTutorial_" + type;
    }

    // Reset one tutorial so it can be tested again.
    public void ResetTutorial(TutorialType type)
    {
        PlayerPrefs.DeleteKey(GetCompletionKey(type));
        PlayerPrefs.Save();

        Debug.Log("Tutorial reset: " + type);
    }

    // Reset all seven tutorials for testing.
    public void ResetAllTutorials()
    {
        foreach (TutorialType type in Enum.GetValues(
            typeof(TutorialType)))
        {
            PlayerPrefs.DeleteKey(GetCompletionKey(type));
        }

        PlayerPrefs.Save();

        Debug.Log("All tutorial completion flags reset.");
    }

    public void ShowTutorialForScene(string sceneName)
    {
        Debug.Log("Checking tutorial for scene: " + sceneName);

        switch (sceneName)
        {
            case "Dashboard":
                ShowTutorial(TutorialType.Dashboard);
                break;

            case "GameReviewsScene":
                ShowTutorial(TutorialType.GameReview);
                break;

            case "ShopScene":
            case "StoreScene":
                ShowTutorial(TutorialType.Shop);
                break;

            case "MarketTrend Scene":
                ShowTutorial(TutorialType.MarketTrends);
                break;

            default:
                Debug.Log("No tutorial configured for scene: " + sceneName);
                break;
        }
    }
}
