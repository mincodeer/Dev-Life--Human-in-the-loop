using System.Collections;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    [Header("Lobby UI")]
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private RectTransform lobbyButtons;
    [SerializeField] private CanvasGroup lobbyCanvasGroup;


    [Header("Camera")]
    [SerializeField] private Camera mainCamera;

    [Tooltip("Lobby camera starts slightly to the right.")]
    [SerializeField] private float lobbyXOffset = 2f;

    [Tooltip("1.2 = Lobby camera appears 1.2x closer.")]
    [SerializeField] private float lobbyZoomMultiplier = 1.2f;


    [Header("Transition")]
    [SerializeField] private float transitionDuration = 1.2f;

    [SerializeField] private float buttonSlideDistance = 300f;


    [Header("After Lobby")]
    [SerializeField] private ComputerInteractionIn computerInteraction;

    // Script controller
    [SerializeField] private DevelopmentTutorialPopup tutorialPopup;

    // Actual UI panel
    [SerializeField] private GameObject developmentTutorialPopupPanel;


    private Vector3 roomCameraPosition;
    private float roomCameraSize;

    private Vector3 lobbyCameraPosition;
    private float lobbyCameraSize;

    private Vector2 buttonStartPosition;

    private bool isTransitioning;


    // =========================================================
    // AWAKE
    // =========================================================

    private void Awake()
    {
        // -----------------------------------------------------
        // TUTORIAL MUST BE HIDDEN AT GAME START
        // -----------------------------------------------------

        if (developmentTutorialPopupPanel != null)
        {
            developmentTutorialPopupPanel.SetActive(false);
        }


        // -----------------------------------------------------
        // SAVE NORMAL GAMEPLAY CAMERA
        // -----------------------------------------------------

        if (mainCamera != null)
        {
            roomCameraPosition =
                mainCamera.transform.position;

            // Gameplay room camera always returns here.
            roomCameraPosition.x = 0f;

            roomCameraSize =
                mainCamera.orthographicSize;


            // -------------------------------------------------
            // CREATE LOBBY CAMERA
            // -------------------------------------------------

            lobbyCameraPosition =
                roomCameraPosition +
                new Vector3(
                    lobbyXOffset,
                    0f,
                    0f
                );


            // Smaller Orthographic Size
            // means the camera is zoomed in.
            lobbyCameraSize =
                roomCameraSize /
                lobbyZoomMultiplier;


            // Start game in Lobby view.
            mainCamera.transform.position =
                lobbyCameraPosition;

            mainCamera.orthographicSize =
                lobbyCameraSize;
        }


        // -----------------------------------------------------
        // SAVE BUTTON START POSITION
        // -----------------------------------------------------

        if (lobbyButtons != null)
        {
            buttonStartPosition =
                lobbyButtons.anchoredPosition;
        }


        // -----------------------------------------------------
        // COMPUTER INTERACTION OFF DURING LOBBY
        // -----------------------------------------------------

        if (computerInteraction != null)
        {
            computerInteraction.enabled = false;
        }


        // -----------------------------------------------------
        // SHOW LOBBY
        // -----------------------------------------------------

        if (lobbyPanel != null)
        {
            lobbyPanel.SetActive(true);
        }


        if (lobbyCanvasGroup != null)
        {
            lobbyCanvasGroup.alpha = 1f;

            lobbyCanvasGroup.interactable = true;

            lobbyCanvasGroup.blocksRaycasts = true;
        }
    }


    // =========================================================
    // PLAY BUTTON
    // =========================================================

    public void PlayGame()
    {
        if (isTransitioning)
            return;


        StartCoroutine(
            PlayTransition()
        );
    }


    // =========================================================
    // LOBBY TRANSITION
    // =========================================================

    private IEnumerator PlayTransition()
    {
        isTransitioning = true;


        // -----------------------------------------------------
        // DISABLE LOBBY BUTTON INPUT
        // -----------------------------------------------------

        if (lobbyCanvasGroup != null)
        {
            lobbyCanvasGroup.interactable = false;

            lobbyCanvasGroup.blocksRaycasts = false;
        }


        float timer = 0f;


        Vector3 cameraStartPosition =
            mainCamera.transform.position;


        float cameraStartSize =
            mainCamera.orthographicSize;


        Vector2 buttonTargetPosition =
            buttonStartPosition +
            Vector2.left *
            buttonSlideDistance;


        // -----------------------------------------------------
        // TRANSITION LOOP
        // -----------------------------------------------------

        while (timer < transitionDuration)
        {
            timer +=
                Time.unscaledDeltaTime;


            float t =
                Mathf.Clamp01(
                    timer /
                    transitionDuration
                );


            float smoothT =
                Mathf.SmoothStep(
                    0f,
                    1f,
                    t
                );


            // -------------------------------------------------
            // CAMERA MOVE
            // -------------------------------------------------

            mainCamera.transform.position =
                Vector3.Lerp(
                    cameraStartPosition,
                    roomCameraPosition,
                    smoothT
                );


            // -------------------------------------------------
            // CAMERA ZOOM OUT
            // -------------------------------------------------

            mainCamera.orthographicSize =
                Mathf.Lerp(
                    cameraStartSize,
                    roomCameraSize,
                    smoothT
                );


            // -------------------------------------------------
            // BUTTONS MOVE LEFT
            // -------------------------------------------------

            if (lobbyButtons != null)
            {
                lobbyButtons.anchoredPosition =
                    Vector2.Lerp(
                        buttonStartPosition,
                        buttonTargetPosition,
                        smoothT
                    );
            }


            // -------------------------------------------------
            // LOBBY FADE OUT
            // -------------------------------------------------

            if (lobbyCanvasGroup != null)
            {
                lobbyCanvasGroup.alpha =
                    1f - smoothT;
            }


            yield return null;
        }


        // =====================================================
        // TRANSITION COMPLETELY FINISHED
        // =====================================================


        // -----------------------------------------------------
        // FORCE EXACT GAMEPLAY CAMERA
        // -----------------------------------------------------

        mainCamera.transform.position =
            roomCameraPosition;


        mainCamera.orthographicSize =
            roomCameraSize;


        // -----------------------------------------------------
        // LOBBY COMPLETELY INVISIBLE
        // -----------------------------------------------------

        if (lobbyCanvasGroup != null)
        {
            lobbyCanvasGroup.alpha = 0f;

            lobbyCanvasGroup.interactable = false;

            lobbyCanvasGroup.blocksRaycasts = false;
        }


        // -----------------------------------------------------
        // ENABLE COMPUTER INTERACTION
        // -----------------------------------------------------

        if (computerInteraction != null)
        {
            computerInteraction.enabled = true;
        }


        // -----------------------------------------------------
        // WAIT ONE FRAME
        //
        // This lets ComputerInteractionIn
        // finish its OnEnable / Start work first.
        // -----------------------------------------------------

        yield return null;


        // =====================================================
        // FORCE TUTORIAL PANEL ON
        // =====================================================

        if (developmentTutorialPopupPanel != null)
        {
            developmentTutorialPopupPanel.SetActive(true);


            Debug.Log(
                "Tutorial Panel ActiveSelf: " +
                developmentTutorialPopupPanel.activeSelf
            );


            Debug.Log(
                "Tutorial Panel ActiveInHierarchy: " +
                developmentTutorialPopupPanel.activeInHierarchy
            );
        }
        else
        {
            Debug.LogError(
                "Development Tutorial Popup Panel is NOT connected!"
            );
        }


        // =====================================================
        // START TUTORIAL
        // =====================================================

        if (tutorialPopup != null)
        {
            tutorialPopup.ShowTutorial();


            Debug.Log(
                "ShowTutorial called successfully."
            );
        }
        else
        {
            Debug.LogError(
                "DevelopmentTutorialPopup script is NOT connected!"
            );
        }


        isTransitioning = false;


        Debug.Log(
            "Lobby finished. Tutorial started."
        );
    }


    // =========================================================
    // SETTINGS
    // =========================================================

    public void OpenSettings()
    {
        Debug.Log(
            "Settings not implemented yet."
        );
    }


    // =========================================================
    // EXIT
    // =========================================================

    public void ExitGame()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying =
            false;

#else

        Application.Quit();

#endif
    }
}