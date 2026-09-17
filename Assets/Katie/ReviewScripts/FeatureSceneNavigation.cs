using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Provides scene-loading methods for the computer menu.
/// Future UI buttons can connect to these methods through On Click().
/// </summary>
public class FeatureSceneNavigation : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField]
    private string dashboardSceneName = "Dashboard";

    [SerializeField]
    private string marketTrendsSceneName = "MarketTrendsScene";

    [SerializeField]
    private string shopSceneName = "ShopScene";

    [SerializeField]
    private string reviewsSceneName = "GameReviewsScene";

    public void OpenDashboard()
    {
        LoadConfiguredScene(dashboardSceneName);
    }

    public void OpenMarketTrends()
    {
        LoadConfiguredScene(marketTrendsSceneName);
    }

    public void OpenShop()
    {
        LoadConfiguredScene(shopSceneName);
    }

    public void OpenReviews()
    {
        LoadConfiguredScene(reviewsSceneName);
    }

    private void LoadConfiguredScene(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogWarning(
                "A destination scene name has not been assigned.");

            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError(
                "Cannot load scene '" + sceneName
                + "'. Check the scene name and add it to Build Settings.");

            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}
