using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DesignStageController : MonoBehaviour
{
    [SerializeField] private DevelopmentFlowManager flowManager;
    [SerializeField] private GameObject modeChoicePanel;
    [SerializeField] private GameObject aiOverlay;
    [SerializeField] private Image animationImage;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float aiInterval = 0.8f;

    private int frame;
    private bool started;
    private bool automatic;
    private bool finished;

    [System.Serializable]
    private class DesignSaveState
    {
        public int frame;
        public bool started;
        public bool automatic;
        public bool finished;
    }

    private bool hasRestorableProgress;

    private void OnEnable()
    {
        // Resume existing or loaded progress.
        if (hasRestorableProgress)
        {
            RestoreDesignView();
            return;
        }

        // First opening starts with an empty activity.
        ResetSavedProgress();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public bool CanBegin()
    {
        return isActiveAndEnabled && !started && frames.Length >= 2;
    }

    public void Begin(bool useAI)
    {
        if (!CanBegin()) return;

        started = true;
        automatic = useAI;

        modeChoicePanel.SetActive(false);
        aiOverlay.SetActive(useAI);
        UpdateStatus();

        if (useAI)
            StartCoroutine(AutoPlay());
    }

    public void OnPencilClicked()
    {
        if (started && !automatic && !finished)
            NextFrame();
    }

    private void NextFrame()
    {
        frame++;
        animationImage.sprite = frames[frame];
        UpdateStatus();

        if (frame == frames.Length - 1)
        {
            finished = true;
            StartCoroutine(Complete());
        }
    }

    private void UpdateStatus()
    {
        string message = automatic
            ? "AI Working..."
            : "Click on the pencil!";

        statusText.text = $"{message}  {frame}/{frames.Length - 1}";
    }

    private IEnumerator AutoPlay()
    {
        while (!finished)
        {
            yield return new WaitForSeconds(aiInterval);
            NextFrame();
        }
    }

    private IEnumerator Complete()
    {
        aiOverlay.SetActive(false);
        statusText.text = "Design Complete!";

        yield return new WaitForSeconds(2f);
        flowManager.CompleteDesign();
    }

    public void CaptureSaveData(StudioSaveData data)
    {
        if (data == null)
            return;

        DesignSaveState saved = new DesignSaveState
        {
            frame = frame,
            started = started,
            automatic = automatic,
            finished = finished
        };

        data.designProgressJson = JsonUtility.ToJson(saved);
    }

    // Restore directly without selecting a work method again.
    public void RestoreSaveData(StudioSaveData data)
    {
        if (data == null)
            return;

        if (string.IsNullOrEmpty(data.designProgressJson))
        {
            ResetSavedProgress();
            return;
        }

        DesignSaveState saved =
            JsonUtility.FromJson<DesignSaveState>(
                data.designProgressJson);

        if (saved == null)
        {
            ResetSavedProgress();
            return;
        }

        StopAllCoroutines();

        int lastFrame =
            frames == null ? 0 : Mathf.Max(0, frames.Length - 1);

        frame = Mathf.Clamp(saved.frame, 0, lastFrame);
        started = saved.started;
        automatic = saved.automatic;
        finished = saved.finished;

        hasRestorableProgress = true;

        if (isActiveAndEnabled)
            RestoreDesignView();
    }

    // Called when starting a new project.
    public void ResetSavedProgress()
    {
        StopAllCoroutines();

        frame = 0;
        started = false;
        automatic = false;
        finished = false;
        hasRestorableProgress = true;

        if (isActiveAndEnabled)
            RestoreDesignView();
    }

    // Rebuilds the UI and resumes AI or the completion delay.
    private void RestoreDesignView()
    {
        StopAllCoroutines();

        if (modeChoicePanel != null)
            modeChoicePanel.SetActive(!started);

        if (aiOverlay != null)
            aiOverlay.SetActive(started && automatic && !finished);

        if (animationImage != null &&
            frames != null &&
            frames.Length > 0)
        {
            animationImage.sprite = frames[
                Mathf.Clamp(frame, 0, frames.Length - 1)];
        }

        if (!started)
        {
            if (statusText != null)
                statusText.text = "";

            return;
        }

        if (finished)
        {
            StartCoroutine(Complete());
            return;
        }

        UpdateStatus();

        if (automatic)
            StartCoroutine(AutoPlay());
    }

}