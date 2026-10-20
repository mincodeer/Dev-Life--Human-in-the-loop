using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBGM : MonoBehaviour
{
    public AudioSource bgmSource;

    public AudioClip originalBGM;
    public AudioClip marketBGM;
    public AudioClip reviewBGM;

    private AudioClip currentBGM;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        UpdateMusic();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMusic();
    }

    void OnSceneUnloaded(Scene scene)
    {
        UpdateMusic();
    }

    void UpdateMusic()
    {
        if (bgmSource == null) return;

        AudioClip targetBGM = originalBGM;

        Scene dashboard = SceneManager.GetSceneByName("Dashboard");
        Scene market = SceneManager.GetSceneByName("MarketTrend Scene");

        if (dashboard.IsValid() && dashboard.isLoaded)
            targetBGM = reviewBGM;
        else if (market.IsValid() && market.isLoaded)
            targetBGM = marketBGM;

        if (targetBGM == null || currentBGM == targetBGM)
            return;

        bgmSource.Stop();
        bgmSource.clip = targetBGM;
        bgmSource.loop = true;
        bgmSource.Play();

        currentBGM = targetBGM;
    }
}