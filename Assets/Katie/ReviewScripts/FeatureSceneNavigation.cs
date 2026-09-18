using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// Lives in the original room scene, on a Managers object that remains active.
public class FeatureSceneNavigation : MonoBehaviour
{
    public static FeatureSceneNavigation Instance { get; private set; }

    [Header("Scene Names")]
    [SerializeField] private string dashboardSceneName = "Dashboard";
    [SerializeField] private string marketTrendsSceneName = "MarketTrendsScene";
    [SerializeField] private string shopSceneName = "ShopScene";
    [SerializeField] private string reviewsSceneName = "GameReviewsScene";

    [Header("Room Connections")]
    [SerializeField] private ComputerInteractionIn computerInteraction;
    [SerializeField] private LobbyController lobby;

    [Tooltip("Optional room scripts to suspend, such as TimeTracker. Do not add this script, EscapeInputRouter, or persistent data managers.")]
    [SerializeField] private Behaviour[] suspendDuringFeature;

    private Scene roomScene;
    private Scene featureScene;
    private bool roomSuspended;
    private readonly List<Action> restoreActions = new List<Action>();
    private readonly List<Behaviour> roomEventComponents = new List<Behaviour>();
    public bool IsBusy { get; private set; }
    public bool IsFeatureOpen => IsBusy || roomSuspended;

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
        roomScene = gameObject.scene;
    }

    public void OpenDashboard() { OpenScene(dashboardSceneName); }
    public void OpenMarketTrends() { OpenScene(marketTrendsSceneName); }
    public void OpenShop() { OpenScene(shopSceneName); }
    public void OpenReviews() { OpenScene(reviewsSceneName); }

    public void OpenScene(string sceneName)
    {
        if (IsBusy || !isActiveAndEnabled) return;
        if (lobby != null && (!lobby.IsGameStarted || lobby.IsTransitioning)) return;
        if (string.IsNullOrWhiteSpace(sceneName) ||
            !Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError("Feature scene is missing from Build Settings / Scene List: " + sceneName, this);
            return;
        }
        if (sceneName == roomScene.name || sceneName == roomScene.path)
        {
            BackToRoom();
            return;
        }
        Scene existing = FindLoadedScene(sceneName);
        if (existing.IsValid() && existing.isLoaded)
        {
            // Prevent taking ownership of a scene loaded by another system.
            if (existing != featureScene)
                Debug.LogError("Scene is already loaded outside this navigation: " + sceneName, this);
            return;
        }
        StartCoroutine(SwitchFeature(sceneName));
    }

    private IEnumerator SwitchFeature(string sceneName)
    {
        IsBusy = true;
        if (!roomSuspended) SuspendRoom();
        else DisableRoomEventSystem();

        if (featureScene.IsValid() && featureScene.isLoaded)
        {
            SceneManager.SetActiveScene(roomScene);
            AsyncOperation unload = SceneManager.UnloadSceneAsync(featureScene);
            if (unload != null) yield return unload;
            featureScene = default(Scene);
        }

        AsyncOperation load = null;
        try { load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive); }
        catch (Exception exception) { Debug.LogError(exception.Message, this); }

        if (load != null) yield return load;
        Scene loaded = FindLoadedScene(sceneName);
        if (load == null || !loaded.IsValid() || !loaded.isLoaded)
        {
            Debug.LogError("Could not open feature scene: " + sceneName, this);
            RestoreRoom();
            IsBusy = false;
            yield break;
        }
        featureScene = loaded;
        SceneManager.SetActiveScene(featureScene);
        if (!HasEnabledEventSystem(featureScene))
            foreach (Behaviour component in roomEventComponents)
                if (component != null) component.enabled = true;
        IsBusy = false;
    }

    public void BackToRoom()
    {
        if (IsBusy || !roomSuspended || !isActiveAndEnabled) return;
        StartCoroutine(CloseFeature());
    }

    private IEnumerator CloseFeature()
    {
        IsBusy = true;
        SceneManager.SetActiveScene(roomScene);
        if (featureScene.IsValid() && featureScene.isLoaded)
        {
            AsyncOperation unload = SceneManager.UnloadSceneAsync(featureScene);
            if (unload != null) yield return unload;
        }
        featureScene = default(Scene);
        RestoreRoom();
        if (computerInteraction != null) computerInteraction.ReturnToRoom();
        IsBusy = false;
    }

    private void SuspendRoom()
    {
        roomSuspended = true;
        restoreActions.Clear();
        roomEventComponents.Clear();
        foreach (GameObject root in roomScene.GetRootGameObjects())
        {
            foreach (Camera camera in root.GetComponentsInChildren<Camera>(true)) Suspend(camera);
            foreach (Canvas canvas in root.GetComponentsInChildren<Canvas>(true)) Suspend(canvas);
            foreach (AudioListener listener in root.GetComponentsInChildren<AudioListener>(true)) Suspend(listener);
            foreach (EventSystem system in root.GetComponentsInChildren<EventSystem>(true))
            {
                if (system.enabled && system.gameObject.activeInHierarchy) roomEventComponents.Add(system);
                Suspend(system);
            }
            foreach (BaseInputModule module in root.GetComponentsInChildren<BaseInputModule>(true))
            {
                if (module.enabled && module.gameObject.activeInHierarchy) roomEventComponents.Add(module);
                Suspend(module);
            }
            foreach (Renderer renderer in root.GetComponentsInChildren<Renderer>(true))
            {
                bool wasEnabled = renderer.enabled;
                restoreActions.Add(() => { if (renderer != null) renderer.enabled = wasEnabled; });
                renderer.enabled = false;
            }
            foreach (Collider2D collider in root.GetComponentsInChildren<Collider2D>(true)) Suspend(collider);
        }
        if (suspendDuringFeature != null)
            foreach (Behaviour component in suspendDuringFeature)
                if (component != this && !(component is EscapeInputRouter)) Suspend(component);
    }

    private void Suspend(Behaviour component)
    {
        if (component == null) return;
        bool wasEnabled = component.enabled;
        restoreActions.Add(() => { if (component != null) component.enabled = wasEnabled; });
        component.enabled = false;
    }

    private void DisableRoomEventSystem()
    {
        foreach (Behaviour component in roomEventComponents)
            if (component != null) component.enabled = false;
    }

    private void RestoreRoom()
    {
        // Reverse order also handles a component listed twice in the Inspector.
        for (int i = restoreActions.Count - 1; i >= 0; i--) restoreActions[i]();
        restoreActions.Clear();
        roomEventComponents.Clear();
        roomSuspended = false;
        if (roomScene.IsValid() && roomScene.isLoaded) SceneManager.SetActiveScene(roomScene);
    }

    private static Scene FindLoadedScene(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByPath(sceneName);
        return scene.IsValid() ? scene : SceneManager.GetSceneByName(Path.GetFileNameWithoutExtension(sceneName));
    }

    private static bool HasEnabledEventSystem(Scene scene)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
            foreach (EventSystem system in root.GetComponentsInChildren<EventSystem>(true))
                if (system.isActiveAndEnabled) return true;
        return false;
    }

    private void OnDestroy()
    {
        if (Instance != this) return;
        RestoreRoom();
        Instance = null;
    }
}
