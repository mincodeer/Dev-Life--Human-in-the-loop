using System.Collections;
using UnityEngine;

/// <summary>
/// Restores the plain room view after returning from a feature scene.
/// Normal game startup is unchanged and can still show the Lobby menu.
/// </summary>
public class RoomReturnState : MonoBehaviour
{
    [Header("Room UI")]
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject shortcutPanel;

    private IEnumerator Start()
    {
        if (!ReturnToRoom.ConsumePlainRoomRequest())
        {
            yield break;
        }

        // Wait until the other scene scripts have applied their normal
        // startup state, then override only the return-from-feature state.
        yield return null;

        if (lobbyPanel != null)
        {
            lobbyPanel.SetActive(false);
        }

        if (shortcutPanel != null)
        {
            shortcutPanel.SetActive(true);
        }
    }
}

