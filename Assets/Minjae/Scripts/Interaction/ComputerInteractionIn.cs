using UnityEngine;

public enum ComputerViewState
{
    Room,
    ComputerZoomedIn,
    ComputerUIOpen
}

public class ComputerInteractionIn : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform zoomTarget;
    [SerializeField] private SpriteRenderer characterSprite;
    [SerializeField] private DevelopmentFlowManager flowManager;

    [Header("Zoom")]
    [SerializeField, Min(0.01f)]
    private float zoomedSize = 2f;

    [Header("Current State")]
    [SerializeField]
    private ComputerViewState currentState =
        ComputerViewState.Room;

    public ComputerViewState CurrentState => currentState;

    private Vector3 roomPosition;
    private float roomSize;
    private float characterAlpha;

    private bool hasRoomView;


    private void Start()
    {
        if (!ConnectionsReady())
            return;

        // Remember the normal room camera position.
        roomPosition = mainCamera.transform.position;
        roomSize = mainCamera.orthographicSize;
        characterAlpha = characterSprite.color.a;

        hasRoomView = true;

        RestoreRoom();

        // Computer UI starts closed.
        // Development stage itself is NOT reset.
        flowManager.CloseComputer();
    }


    public void InteractIn()
    {
        if (!isActiveAndEnabled || !hasRoomView)
            return;

        switch (currentState)
        {
            // First click:
            // Room -> Zoomed In
            case ComputerViewState.Room:

                ZoomToComputer();

                break;


            // Second click:
            // Zoomed In -> Open UI
            case ComputerViewState.ComputerZoomedIn:

                flowManager.OpenComputer();

                currentState =
                    ComputerViewState.ComputerUIOpen;

                Debug.Log(
                    "Computer State: UI Open");

                break;


            // Already using the computer UI.
            case ComputerViewState.ComputerUIOpen:

                break;
        }
    }


    // Called by ESC through ComputerInteractionOut.
    public void ExitOneStep()
    {
        switch (currentState)
        {
            // First ESC:
            // UI disappears,
            // but camera stays zoomed in.
            case ComputerViewState.ComputerUIOpen:

                flowManager.CloseComputer();

                currentState =
                    ComputerViewState.ComputerZoomedIn;

                Debug.Log(
                    "Computer State: Zoomed In");

                break;


            // Second ESC:
            // Leave the computer completely.
            case ComputerViewState.ComputerZoomedIn:

                ReturnToRoom();

                break;


            // ESC does nothing here.
            case ComputerViewState.Room:

                break;
        }
    }


    // Called when Coding / Design / Sound finishes.
    // UI has disappeared, but camera should stay zoomed in.
    public void ReturnToZoomedInState()
    {
        if (!hasRoomView)
            return;

        // Do not magically zoom in if the player
        // is actually already in the room.
        if (currentState == ComputerViewState.Room)
            return;

        currentState =
            ComputerViewState.ComputerZoomedIn;

        Debug.Log(
            "Computer State: Zoomed In after stage completion");
    }


    private void ZoomToComputer()
    {
        mainCamera.transform.position =
            new Vector3(
                zoomTarget.position.x,
                zoomTarget.position.y,
                roomPosition.z
            );

        mainCamera.orthographicSize =
            zoomedSize;

        // Hide character while looking at computer.
        SetCharacterAlpha(0f);

        currentState =
            ComputerViewState.ComputerZoomedIn;

        Debug.Log(
            "Computer State: Zoomed In");
    }


    public void ReturnToRoom()
    {
        if (!hasRoomView)
            return;

        // Make sure UI is closed.
        flowManager.CloseComputer();

        RestoreRoom();

        Debug.Log(
            "Computer State: Room");
    }


    private void RestoreRoom()
    {
        if (!hasRoomView)
            return;

        if (mainCamera != null)
        {
            mainCamera.transform.position =
                roomPosition;

            mainCamera.orthographicSize =
                roomSize;
        }

        if (characterSprite != null)
        {
            SetCharacterAlpha(
                characterAlpha);
        }

        currentState =
            ComputerViewState.Room;
    }


    private void SetCharacterAlpha(float alpha)
    {
        Color color =
            characterSprite.color;

        color.a = alpha;

        characterSprite.color =
            color;
    }


    private bool ConnectionsReady()
    {
        if (mainCamera == null ||
            zoomTarget == null ||
            characterSprite == null ||
            flowManager == null)
        {
            Debug.LogError(
                "Computer: Inspector connections are missing.",
                this);

            return false;
        }

        if (!mainCamera.orthographic)
        {
            Debug.LogError(
                "Computer: use an Orthographic camera.",
                this);

            return false;
        }

        return true;
    }
}