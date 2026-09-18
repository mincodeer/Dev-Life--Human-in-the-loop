using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// Keep this enabled on a Managers object. Only this script should read ESC.
public class EscapeInputRouter : MonoBehaviour
{
    public static EscapeInputRouter Instance { get; private set; }

    [Header("Connections")]
    [SerializeField] private LobbyController lobby;
    [SerializeField] private ComputerInteractionIn computer;
    [SerializeField] private GameObject settingsPanel;

    [Header("Optional existing Settings methods")]
    [SerializeField] private UnityEvent onSettingsOpened;
    [SerializeField] private UnityEvent onSettingsClosed;

    [Header("Optional: tutorial or other modal panels")]
    [SerializeField] private GameObject[] blockingPanels;

    private int lastHandledFrame = -1;
    public bool SettingsOpen => settingsPanel != null && settingsPanel.activeSelf;

    public static bool WorldInputBlocked
    {
        get
        {
            if (FeatureSceneNavigation.Instance != null &&
                FeatureSceneNavigation.Instance.IsFeatureOpen) return true;
            if (Instance == null) return false;
            return Instance.SettingsOpen || Instance.HasBlockingPanel() ||
                (Instance.lobby != null &&
                 (!Instance.lobby.IsGameStarted || Instance.lobby.IsTransitioning));
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatic() { Instance = null; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            enabled = false;
            return;
        }
        Instance = this;
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            HandleEscape();
    }

    public void HandleEscape()
    {
        if (lastHandledFrame == Time.frameCount) return;
        lastHandledFrame = Time.frameCount;

        var navigation = FeatureSceneNavigation.Instance;
        if (navigation != null && navigation.IsBusy) return;
        if (lobby != null && lobby.IsTransitioning) return;

        if (SettingsOpen) { CloseSettings(); return; }
        if (navigation != null && navigation.IsFeatureOpen)
        {
            navigation.BackToRoom();
            return;
        }
        if (HasBlockingPanel()) return;
        if (computer != null && computer.CurrentState != ComputerViewState.Room)
        {
            computer.ReturnToRoom();
            return;
        }
        OpenSettings();
    }

    public void OpenSettings()
    {
        if (SettingsOpen || settingsPanel == null) return;
        if (lobby != null && lobby.IsTransitioning) return;
        if (FeatureSceneNavigation.Instance != null &&
            FeatureSceneNavigation.Instance.IsFeatureOpen) return;
        settingsPanel.SetActive(true);
        onSettingsOpened?.Invoke();
    }

    public void CloseSettings()
    {
        if (!SettingsOpen) return;
        onSettingsClosed?.Invoke();
        settingsPanel.SetActive(false);
    }

    private bool HasBlockingPanel()
    {
        if (blockingPanels == null) return false;
        foreach (var panel in blockingPanels)
            if (panel != null && panel.activeInHierarchy) return true;
        return false;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
