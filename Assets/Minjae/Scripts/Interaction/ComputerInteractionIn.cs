using UnityEngine;

public class ComputerInteractionIn : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform zoomTarget;
    [SerializeField] private SpriteRenderer characterSprite;
    [SerializeField] private DevelopmentFlowManager flowManager;
    [SerializeField] private GameObject developmentUI;

    [Header("Zoom")]
    [SerializeField, Min(0.01f)] private float zoomedSize = 2f;

    private Vector3 roomPosition;
    private float roomSize;
    private float characterAlpha;

    private bool hasRoomView;
    private bool isInside;

    private void OnEnable()
    {
        if (!ConnectionsReady())
            return;

        // Capture once, so the zoomed view cannot replace the room view.
        if (!hasRoomView)
        {
            roomPosition = mainCamera.transform.position;
            roomSize = mainCamera.orthographicSize;
            characterAlpha = characterSprite.color.a;
            hasRoomView = true;
        }

        RestoreRoom();

        // Start each new activation with the computer closed.
        flowManager.CloseComputer();
    }

    public void InteractIn()
    {
        if (!isActiveAndEnabled || !hasRoomView)
            return;

        // First click: enter the computer view.
        if (!isInside)
        {
            mainCamera.transform.position = new Vector3(
                zoomTarget.position.x,
                zoomTarget.position.y,
                roomPosition.z
            );

            mainCamera.orthographicSize = zoomedSize;
            SetCharacterAlpha(0f);

            isInside = true;
            return;
        }

        // Second click: open the current development stage.
        flowManager.OpenComputer();
    }

    public void ReturnToRoom()
    {
        if (!hasRoomView || !isInside)
            return;

        // Keep the current development stage when leaving the computer.
        developmentUI.SetActive(false);
        RestoreRoom();
    }

    private void RestoreRoom()
    {
        if (!hasRoomView)
            return;

        if (mainCamera != null)
        {
            mainCamera.transform.position = roomPosition;
            mainCamera.orthographicSize = roomSize;
        }

        if (characterSprite != null)
            SetCharacterAlpha(characterAlpha);

        isInside = false;
    }

    private void SetCharacterAlpha(float alpha)
    {
        Color color = characterSprite.color;
        color.a = alpha;
        characterSprite.color = color;
    }

    private void OnDisable()
    {
        // Also runs when stopping Play Mode.
        RestoreRoom();

        if (developmentUI != null)
            developmentUI.SetActive(false);
    }

    private bool ConnectionsReady()
    {
        if (mainCamera == null || zoomTarget == null ||
            characterSprite == null || flowManager == null ||
            developmentUI == null)
        {
            Debug.LogError("Computer: Inspector connections are missing.", this);
            return false;
        }

        if (!mainCamera.orthographic)
        {
            Debug.LogError("Computer: use an Orthographic camera.", this);
            return false;
        }

        return true;
    }
}