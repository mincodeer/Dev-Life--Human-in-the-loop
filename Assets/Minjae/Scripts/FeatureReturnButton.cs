using UnityEngine;

// Attach to a Back button inside Shop / Market / Reviews scenes.
public class FeatureReturnButton : MonoBehaviour
{
    public void BackToRoom()
    {
        if (FeatureSceneNavigation.Instance != null)
            FeatureSceneNavigation.Instance.BackToRoom();
        else
            Debug.LogWarning("Start Play Mode from the main room scene first.");
    }

    public void OpenDashboard()
    {
        if (FeatureSceneNavigation.Instance != null)
            FeatureSceneNavigation.Instance.OpenDashboard();
    }
}
