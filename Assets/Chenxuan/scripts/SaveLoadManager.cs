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

    [SerializeField] private GameObject saveNotification;
    [SerializeField, Min(0.1f)] private float notificationDuration = 2f;

    private GameTimeManager gameTimeManager;

    // Stores the full saved game separately from live upgrade flags.
    private const string StudioSaveKey = "StudioSaveSnapshot_v1";

    [SerializeField] private ProjectResourceController projectResources;
    [SerializeField] private DevelopmentFlowManager developmentFlow;
    [SerializeField] private PlayerTitleController playerTitle;

    private Coroutine saveMessageRoutine;

    private bool isRestoring;

    // Tracks initialization for this Play session only.
    public static bool SessionInitialized { get; private set; }

    // Also runs when entering Play mode with domain reload disabled.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetSessionState()
    {
        SessionInitialized = false;
    }

    private IEnumerator Start()
    {
        if (saveMessage != null)
            saveMessage.gameObject.SetActive(false);

        // Hide the whole notification at startup.
        if (saveNotification != null)
            saveNotification.SetActive(false);

        // Wait for the other managers to finish starting.
        yield return null;

        gameTimeManager = GameTimeManager.Instance;

        if (gameTimeManager == null)
            gameTimeManager =
                FindFirstObjectByType<GameTimeManager>();

        if (SessionInitialized)
            yield break;

        isRestoring = true;

        try
        {
            SessionInitialized = InitializeFreshStudio();
        }
        catch (System.Exception exception)
        {
            Debug.LogException(exception, this);
            ShowSaveMessage("Startup failed: check Console");
        }
        finally
        {
            isRestoring = false;
        }
    }

    public void SaveGame()
    {
        if (isRestoring)
            return;
        try
        {
            StudioSaveData data = CaptureStudioSnapshot();

            if (data == null)
            {
                ShowSaveMessage("Save failed: check connections");
                return;
            }

            // Capture everything before replacing the previous save.
            string json = JsonUtility.ToJson(data);

            PlayerPrefs.SetString(StudioSaveKey, json);
            PlayerPrefs.Save();

            ShowSaveMessage("Saved successfully!");
            Debug.Log("Full studio snapshot saved.");
        }
        catch (System.Exception exception)
        {
            ShowSaveMessage("Save failed");
            Debug.LogException(exception, this);
        }
    }

    public void LoadGame()
    {
        if (isRestoring)
            return;

        if (!PlayerPrefs.HasKey(StudioSaveKey))
        {
            ShowSaveMessage("No saved studio found");
            return;
        }

        StudioSaveData backup = null;
        bool restoreStarted = false;

        try
        {
            string json = PlayerPrefs.GetString(StudioSaveKey);

            StudioSaveData data =
                JsonUtility.FromJson<StudioSaveData>(json);

            // Check the snapshot before changing the current game.
            if (data == null ||
                data.version != 1 ||
                data.currentProject == null ||
                data.currentDay < 1 ||
                data.currentHour < 0 || data.currentHour > 23 ||
                data.currentMinute < 0 || data.currentMinute > 59 ||
                !System.Enum.IsDefined(
                    typeof(DevelopmentStage), data.developmentStage) ||
                string.IsNullOrEmpty(data.codingProgressJson) ||
                string.IsNullOrEmpty(data.designProgressJson) ||
                string.IsNullOrEmpty(data.soundProgressJson) ||
                string.IsNullOrEmpty(data.debuggingProgressJson) ||
                string.IsNullOrEmpty(data.buildProgressJson))
            {
                ShowSaveMessage("Save data is incomplete or unsupported");
                return;
            }

            // Also finds and checks the required scene connections.
            backup = CaptureStudioSnapshot();

            if (backup == null)
            {
                ShowSaveMessage("Load failed: check connections");
                return;
            }

            isRestoring = true;
            restoreStarted = true;

            ApplyStudioSnapshot(data);

            ShowSaveMessage("Game Loaded");
            Debug.Log("Full studio snapshot loaded.");
        }
        catch (System.Exception exception)
        {
            Debug.LogException(exception, this);

            // Attempt to restore the previous live state if loading failed.
            if (restoreStarted && backup != null)
            {
                try
                {
                    ApplyStudioSnapshot(backup);
                }
                catch (System.Exception rollbackException)
                {
                    Debug.LogException(rollbackException, this);
                }
            }

            ShowSaveMessage("Load failed: check Console");
        }
        finally
        {
            isRestoring = false;
        }
    }

    private IEnumerator HideSaveMessage()
    {
        // Works even while the settings menu pauses the game.
        yield return new WaitForSecondsRealtime(notificationDuration);

        if (saveMessage != null)
            saveMessage.gameObject.SetActive(false);

        if (saveNotification != null)
            saveNotification.SetActive(false);

        saveMessageRoutine = null;
    }

    // Captures the current studio without changing gameplay.
    private StudioSaveData CaptureStudioSnapshot()
    {
        gameTimeManager = GameTimeManager.Instance;

        if (gameTimeManager == null)
            gameTimeManager =
                FindFirstObjectByType<GameTimeManager>();

        if (projectResources == null)
            projectResources =
                FindFirstObjectByType<ProjectResourceController>(
                    FindObjectsInactive.Include);

        if (developmentFlow == null)
            developmentFlow =
                FindFirstObjectByType<DevelopmentFlowManager>(
                    FindObjectsInactive.Include);

        if (playerTitle == null)
            playerTitle =
                FindFirstObjectByType<PlayerTitleController>(
                    FindObjectsInactive.Include);

        if (gameTimeManager == null ||
            ResourceManager.Instance == null ||
            ProjectDataManager.Instance == null ||
            RoomUpgradeManager.Instance == null ||
            projectResources == null ||
            developmentFlow == null ||
            playerTitle == null ||
            bgmSlider == null ||
            sfxSlider == null ||
            keyboardSlider == null)
        {
            Debug.LogError(
                "SaveLoadManager: a required manager or volume slider " +
                "is missing. Save from the main room scene.", this);

            return null;
        }

        StudioSaveData data = new StudioSaveData();

        ResourceManager.Instance.CaptureSaveData(data);
        ProjectDataManager.Instance.CaptureSaveData(data);
        RoomUpgradeManager.Instance.CaptureSaveData(data);

        projectResources.CaptureSaveData(data);
        playerTitle.CaptureSaveData(data);
        developmentFlow.CaptureSaveData(data);

        data.currentDay = gameTimeManager.currentDay;
        data.currentHour = gameTimeManager.currentHour;
        data.currentMinute = gameTimeManager.currentMinute;

        data.bgmVolume = bgmSlider.value;
        data.sfxVolume = sfxSlider.value;
        data.keyboardVolume = keyboardSlider.value;

        // Every activity must supply its progress snapshot.
        if (string.IsNullOrEmpty(data.codingProgressJson) ||
            string.IsNullOrEmpty(data.designProgressJson) ||
            string.IsNullOrEmpty(data.soundProgressJson) ||
            string.IsNullOrEmpty(data.debuggingProgressJson) ||
            string.IsNullOrEmpty(data.buildProgressJson))
        {
            Debug.LogError(
                "SaveLoadManager: check all five activity connections " +
                "on DevelopmentFlowManager.", this);

            return null;
        }

        return data;
    }

    // Displays confirmation only after the save has completed.
    private void ShowSaveMessage(string message)
    {
        if (saveMessage == null)
            return;

        if (saveMessageRoutine != null)
            StopCoroutine(saveMessageRoutine);

        saveMessage.text = message;

        if (saveNotification != null)
            saveNotification.SetActive(true);

        saveMessage.gameObject.SetActive(true);
        saveMessageRoutine = StartCoroutine(HideSaveMessage());
    }

    // Applies a snapshot without repeating purchases or project rewards.
    private void ApplyStudioSnapshot(StudioSaveData data)
    {
        // Stop the current activities before replacing their data.
        developmentFlow.CloseComputer();

        // Return the camera and interaction state to the room.
        ComputerInteractionIn computer =
            FindFirstObjectByType<ComputerInteractionIn>(
                FindObjectsInactive.Include);

        if (computer != null)
            computer.ReturnToRoom();

        // Restore underlying data before refreshing its UI.
        ResourceManager.Instance.RestoreSaveData(data);
        ProjectDataManager.Instance.RestoreSaveData(data);
        projectResources.RestoreSaveData(data);

        gameTimeManager.RestoreSavedTime(
            data.currentDay,
            data.currentHour,
            data.currentMinute);

        playerTitle.RestoreSaveData(data);
        RoomUpgradeManager.Instance.RestoreSaveData(data);

        // Restore the stage and activity progress with the computer closed.
        developmentFlow.RestoreSaveData(data);

        // Keep the existing audio preference keys in sync.
        PlayerPrefs.SetFloat("BGMVolume", data.bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", data.sfxVolume);
        PlayerPrefs.SetFloat("KeyboardVolume", data.keyboardVolume);

        bgmSlider.value = data.bgmVolume;
        sfxSlider.value = data.sfxVolume;
        keyboardSlider.value = data.keyboardVolume;

        PlayerPrefs.Save();

        // Notify listeners after the saved systems have been restored.
        ProjectDataManager.Instance.RefreshAfterLoad();

        if (ProjectHistoryManager.Instance != null)
            ProjectHistoryManager.Instance.RefreshFromProjectDataManager();

        ResourceManager.Instance.OnResourcesChanged?.Invoke();
    }
    // Resets live studio progress without deleting the saved snapshot.
    private bool InitializeFreshStudio()
    {
        // Find and validate the required scene connections.
        StudioSaveData current = CaptureStudioSnapshot();

        if (current == null)
        {
            ShowSaveMessage("Startup failed: check connections");
            return false;
        }

        StudioSaveData fresh = new StudioSaveData
        {
            // Match ResourceManager's existing new-game defaults.
            money = 250,
            fandom = 0,
            developmentSkill = 10,
            technicalDebt = 0,
            aiCredits = 10,

            currentDay = 1,
            currentHour = 8,
            currentMinute = 0,

            currentProject = new ProjectData(),
            developmentStage = DevelopmentStage.None,

            // Keep the existing audio startup settings.
            bgmVolume = current.bgmVolume,
            sfxVolume = current.sfxVolume,
            keyboardVolume = current.keyboardVolume
        };

        // Other counters, conditions and upgrades default to zero/false.
        // Empty activity snapshots cause each activity to reset.
        ApplyStudioSnapshot(fresh);

        Debug.Log("Fresh studio started. Saved snapshot preserved.");
        return true;
    }
}
