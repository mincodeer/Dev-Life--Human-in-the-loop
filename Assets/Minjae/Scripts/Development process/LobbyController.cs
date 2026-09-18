using System.Collections;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    [Header("Lobby UI")]
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private RectTransform lobbyButtons;
    [SerializeField] private CanvasGroup lobbyCanvasGroup;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float lobbyXOffset = 2f;
    [SerializeField, Min(0.01f)] private float lobbyZoomMultiplier = 1.2f;

    [Header("Transition")]
    [SerializeField, Min(0f)] private float transitionDuration = 1.2f;
    [SerializeField] private float buttonSlideDistance = 300f;

    [Header("After Lobby")]
    [SerializeField] private ComputerInteractionIn computerInteraction;
    [SerializeField] private DevelopmentTutorialPopup tutorialPopup;
    [SerializeField] private GameObject developmentTutorialPopupPanel;

    [Header("Game UI and Settings")]
    [SerializeField] private GameUIVisibility gameUI;
    [SerializeField] private EscapeInputRouter escapeInput;

    private Vector3 roomCameraPosition;
    private float roomCameraSize;
    private Vector2 buttonStartPosition;
    private bool isTransitioning;
    public bool IsTransitioning => isTransitioning;
    public bool IsGameStarted { get; private set; }

    private void Awake()
    {
        IsGameStarted = false;
        if (gameUI != null) gameUI.Hide();
        if (developmentTutorialPopupPanel != null)
            developmentTutorialPopupPanel.SetActive(false);

        if (mainCamera != null)
        {
            roomCameraPosition = mainCamera.transform.position;
            roomCameraPosition.x = 0f; // Preserve your existing room camera convention.
            roomCameraSize = mainCamera.orthographicSize;
            mainCamera.transform.position = roomCameraPosition + Vector3.right * lobbyXOffset;
            mainCamera.orthographicSize = roomCameraSize / Mathf.Max(0.01f, lobbyZoomMultiplier);
        }
        if (lobbyButtons != null) buttonStartPosition = lobbyButtons.anchoredPosition;
        if (computerInteraction != null) computerInteraction.enabled = false;
        if (lobbyPanel != null) lobbyPanel.SetActive(true);
        SetLobbyAlpha(1f, true);
    }

    public void PlayGame()
    {
        if (isTransitioning || IsGameStarted) return;
        if (escapeInput != null && escapeInput.SettingsOpen) return;
        StartCoroutine(PlayTransition());
    }

    private IEnumerator PlayTransition()
    {
        isTransitioning = true;
        SetLobbyAlpha(1f, false);
        Vector3 startPosition = mainCamera != null ? mainCamera.transform.position : Vector3.zero;
        float startSize = mainCamera != null ? mainCamera.orthographicSize : 1f;
        Vector2 target = buttonStartPosition + Vector2.left * buttonSlideDistance;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / transitionDuration));
            if (mainCamera != null)
            {
                mainCamera.transform.position = Vector3.Lerp(startPosition, roomCameraPosition, t);
                mainCamera.orthographicSize = Mathf.Lerp(startSize, roomCameraSize, t);
            }
            if (lobbyButtons != null)
                lobbyButtons.anchoredPosition = Vector2.Lerp(buttonStartPosition, target, t);
            SetLobbyAlpha(1f - t, false);
            yield return null;
        }

        if (mainCamera != null)
        {
            mainCamera.transform.position = roomCameraPosition;
            mainCamera.orthographicSize = roomCameraSize;
        }
        SetLobbyAlpha(0f, false);
        if (lobbyPanel != null) lobbyPanel.SetActive(false);
        if (computerInteraction != null) computerInteraction.enabled = true;
        yield return null; // ComputerInteractionIn.Start records the gameplay camera here.

        IsGameStarted = true;
        isTransitioning = false;
        if (gameUI != null) gameUI.Show();
        if (developmentTutorialPopupPanel != null)
            developmentTutorialPopupPanel.SetActive(true);
        if (tutorialPopup != null) tutorialPopup.ShowTutorial();
    }

    private void SetLobbyAlpha(float alpha, bool inputEnabled)
    {
        if (lobbyCanvasGroup == null) return;
        lobbyCanvasGroup.alpha = alpha;
        lobbyCanvasGroup.interactable = inputEnabled;
        lobbyCanvasGroup.blocksRaycasts = inputEnabled;
    }

    public void OpenSettings()
    {
        if (escapeInput != null) escapeInput.OpenSettings();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
