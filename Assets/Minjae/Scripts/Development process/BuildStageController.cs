using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuildStageController : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] private DevelopmentFlowManager flowManager;

    [SerializeField] private Image animationImage;

    [SerializeField] private Image progressBarFill;

    [SerializeField] private Button buildButton;


    [Header("Sprites")]
    [Tooltip("순서: 0%, 30%, 60%, 90%, 100%")]
    [SerializeField] private Sprite[] frames;


    [Header("Timing")]
    [SerializeField, Min(0.1f)]
    private float buildDuration = 4f;

    [SerializeField, Min(0f)]
    private float completionDelay = 1.5f;


    private float progress;

    private bool isBuilding;

    private bool isComplete;


    // =========================================================
    // ON ENABLE
    // =========================================================

    private void OnEnable()
    {
        if (hasRestorableProgress)
        {
            RestoreBuildView();
            return;
        }

        ResetBuild();
    }


    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        if (!isBuilding)
            return;


        // Slowly increase build progress.
        progress +=
            Time.deltaTime / buildDuration;


        progress =
            Mathf.Clamp01(progress);


        UpdateProgressBar();

        UpdateSpriteByProgress();


        // Build finished.
        if (progress >= 1f)
        {
            isBuilding = false;

            isComplete = true;


            StartCoroutine(
                FinishBuild()
            );
        }
    }


    // =========================================================
    // START BUILD
    // =========================================================

    public void StartBuild()
    {
        if (isBuilding || isComplete)
            return;


        if (frames == null ||
            frames.Length == 0)
        {
            Debug.LogError(
                "BuildStageController: Frames are missing."
            );

            return;
        }


        if (progressBarFill == null)
        {
            Debug.LogError(
                "BuildStageController: Progress Bar Fill is missing."
            );

            return;
        }


        isBuilding = true;

        progress = 0f;


        if (buildButton != null)
        {
            buildButton.interactable = false;
        }


        UpdateProgressBar();

        UpdateSpriteByProgress();
    }


    // =========================================================
    // UPDATE PROGRESS BAR
    // =========================================================

    private void UpdateProgressBar()
    {
        if (progressBarFill == null)
            return;


        progressBarFill.type =
            Image.Type.Filled;


        progressBarFill.fillMethod =
            Image.FillMethod.Horizontal;


        progressBarFill.fillOrigin =
            (int)Image.OriginHorizontal.Left;


        progressBarFill.fillAmount =
            progress;
    }


    // =========================================================
    // UPDATE BUILD SPRITE
    // =========================================================

    private void UpdateSpriteByProgress()
    {
        if (animationImage == null ||
            frames == null ||
            frames.Length == 0)
        {
            return;
        }


        int index =
            GetFrameIndexByProgress(
                progress
            );


        if (index >= 0 &&
            index < frames.Length)
        {
            animationImage.sprite =
                frames[index];
        }
    }


    // =========================================================
    // PROGRESS -> SPRITE
    // =========================================================

    private int GetFrameIndexByProgress(
        float value)
    {
        // 100%
        if (value >= 1f)
        {
            return Mathf.Min(
                4,
                frames.Length - 1
            );
        }


        // 90%
        if (value >= 0.9f)
        {
            return Mathf.Min(
                3,
                frames.Length - 1
            );
        }


        // 60%
        if (value >= 0.6f)
        {
            return Mathf.Min(
                2,
                frames.Length - 1
            );
        }


        // 30%
        if (value >= 0.3f)
        {
            return Mathf.Min(
                1,
                frames.Length - 1
            );
        }


        // 0%
        return 0;
    }


    // =========================================================
    // FINISH BUILD
    // =========================================================

    private IEnumerator FinishBuild()
    {
        // Make sure the final visual is exactly 100%.
        progress = 1f;


        UpdateProgressBar();

        UpdateSpriteByProgress();


        yield return new WaitForSeconds(
            completionDelay
        );


        if (flowManager != null)
        {
            flowManager.GotoResult();
        }
        else
        {
            Debug.LogError(
                "BuildStageController: Flow Manager is missing."
            );
        }
    }


    // =========================================================
    // RESET BUILD
    // =========================================================

    public void ResetBuild()
    {
        hasRestorableProgress = true;
        StopAllCoroutines();


        progress = 0f;

        isBuilding = false;

        isComplete = false;


        // Reset button.
        if (buildButton != null)
        {
            buildButton.interactable = true;
        }


        // Reset progress bar.
        if (progressBarFill != null)
        {
            progressBarFill.type =
                Image.Type.Filled;


            progressBarFill.fillMethod =
                Image.FillMethod.Horizontal;


            progressBarFill.fillOrigin =
                (int)Image.OriginHorizontal.Left;


            progressBarFill.fillAmount =
                0f;
        }


        // Reset first sprite.
        if (animationImage != null &&
            frames != null &&
            frames.Length > 0)
        {
            animationImage.sprite =
                frames[0];
        }
    }

    [System.Serializable]
    private class BuildSaveState
    {
        public float progress;
        public bool isBuilding;
        public bool isComplete;
    }

    private bool hasRestorableProgress;

    // Saves the build progress and current state.
    public void CaptureSaveData(StudioSaveData data)
    {
        if (data == null)
            return;

        BuildSaveState saved = new BuildSaveState
        {
            progress = progress,
            isBuilding = isBuilding,
            isComplete = isComplete
        };

        data.buildProgressJson = JsonUtility.ToJson(saved);
    }

    // Restores progress without restarting the build from zero.
    public void RestoreSaveData(StudioSaveData data)
    {
        if (data == null)
            return;

        if (string.IsNullOrEmpty(data.buildProgressJson))
        {
            ResetBuild();
            return;
        }

        BuildSaveState saved =
            JsonUtility.FromJson<BuildSaveState>(
                data.buildProgressJson);

        if (saved == null)
        {
            ResetBuild();
            return;
        }

        StopAllCoroutines();

        progress = Mathf.Clamp01(saved.progress);
        isComplete = saved.isComplete || progress >= 1f;
        isBuilding = saved.isBuilding && !isComplete;

        hasRestorableProgress = true;

        if (isActiveAndEnabled)
            RestoreBuildView();
    }

    // Displays the saved build and resumes its completion delay.
    private void RestoreBuildView()
    {
        StopAllCoroutines();

        UpdateProgressBar();
        UpdateSpriteByProgress();

        if (buildButton != null)
            buildButton.interactable = !isBuilding && !isComplete;

        if (isComplete)
            StartCoroutine(FinishBuild());
    }
}