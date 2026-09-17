using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Returns a feature screen to the main room scene.
/// Connect ReturnToRoom() to a UI Button On Click event.
/// </summary>
public class ReturnToRoom : MonoBehaviour
{
    [SerializeField] private string roomSceneName = "Minjae's Scene";

    public void ReturnToRoomScene()
    {
        if (string.IsNullOrWhiteSpace(roomSceneName))
        {
            Debug.LogWarning("The room scene name has not been assigned.");
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(roomSceneName))
        {
            Debug.LogError(
                "Cannot load scene '" + roomSceneName
                + "'. Check the scene name and Build Settings.");
            return;
        }

        SceneManager.LoadScene(roomSceneName);
    }
}
