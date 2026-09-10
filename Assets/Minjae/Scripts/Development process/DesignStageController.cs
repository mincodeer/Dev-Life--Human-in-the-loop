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

    private void OnEnable()
    {
        frame = 0;
        started = false;
        automatic = false;
        finished = false;

        modeChoicePanel.SetActive(true);
        aiOverlay.SetActive(false);
        statusText.text = "";

        if (frames.Length > 0)
            animationImage.sprite = frames[0];
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        if (started || Keyboard.current == null)
            return;

        if (Keyboard.current.mKey.wasPressedThisFrame)
            flowManager.ChooseManual();
        else if (Keyboard.current.aKey.wasPressedThisFrame)
            flowManager.ChooseAI();
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
}