using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SoundStageController : MonoBehaviour
{
    [System.Serializable]
    public class Track
    {
        public Slider slider;
        public Image blocks;
        public Image meter;

        [System.NonSerialized] public float previousValue;
        [System.NonSerialized] public float progress;
    }

    [Header("Connections")]
    [SerializeField] private DevelopmentFlowManager flowManager;
    [SerializeField] private GameObject modeChoicePanel;
    [SerializeField] private GameObject aiOverlay;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private RectTransform playhead;
    [SerializeField] private Track[] tracks;

    [Header("Timing")]
    [SerializeField, Min(0.1f)] private float requiredMovement = 2f;
    [SerializeField, Min(0.1f)] private float fillSpeed = 0.5f;
    [SerializeField, Min(0.1f)] private float aiSecondsPerTrack = 2f;

    private bool started;
    private bool automatic;
    private bool finished;
    private int activeTrack;
    private float elapsed;

    private void OnEnable()
    {
        started = false;
        automatic = false;
        finished = false;
        activeTrack = 0;
        elapsed = 0f;

        modeChoicePanel.SetActive(true);
        aiOverlay.SetActive(false);
        statusText.text = "";
        SetPlayhead(0f);

        foreach (Track track in tracks)
        {
            track.slider.SetValueWithoutNotify(0.5f);
            track.slider.interactable = false;
            track.previousValue = track.slider.normalizedValue;
            track.progress = 0f;

            SetupImage(track.blocks, Image.FillMethod.Horizontal);
            SetupImage(track.meter, Image.FillMethod.Vertical);
        }
    }

    private void SetupImage(Image image, Image.FillMethod method)
    {
        image.type = Image.Type.Filled;
        image.fillMethod = method;
        image.fillOrigin = 0; 
        image.fillAmount = 0f;
        image.preserveAspect = false;
        image.raycastTarget = false;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public bool CanBegin()
    {
        return isActiveAndEnabled && !started && tracks.Length > 0;
    }

    public void Begin(bool useAI)
    {
        if (!CanBegin()) return;

        started = true;
        automatic = useAI;

        modeChoicePanel.SetActive(false);
        aiOverlay.SetActive(useAI);

        foreach (Track track in tracks)
        {
            track.slider.interactable = !useAI;
            track.previousValue = track.slider.normalizedValue;
        }
    }

    private void Update()
    {
        if (!Application.isFocused || Time.timeScale == 0f)
            return;

        if (!started)
        {
            if (Keyboard.current == null) return;

            if (Keyboard.current.mKey.wasPressedThisFrame)
                flowManager.ChooseManual();
            else if (Keyboard.current.aKey.wasPressedThisFrame)
                flowManager.ChooseAI();

            return;
        }

        if (finished) return;

        elapsed += Time.deltaTime;

        if (automatic)
            UpdateAI();
        else
            ReadSliders();

        int completeCount = 0;

        for (int i = 0; i < tracks.Length; i++)
        {
            Track track = tracks[i];

            track.blocks.fillAmount = Mathf.MoveTowards(
                track.blocks.fillAmount,
                track.progress,
                fillSpeed * Time.deltaTime);

            float pulse = 0.7f + 0.3f *
                Mathf.PerlinNoise(elapsed * 4f, i * 3f);

            track.meter.fillAmount =
                track.slider.normalizedValue * pulse;

            if (track.blocks.fillAmount >= 1f)
                completeCount++;
        }

        SetPlayhead(tracks[activeTrack].blocks.fillAmount);

        string message = automatic
            ? "AI Working..."
            : "Move all sliders!";

        statusText.text =
            $"{message} {completeCount}/{tracks.Length}";

        if (completeCount == tracks.Length)
        {
            finished = true;
            StartCoroutine(Complete());
        }
    }

    private void ReadSliders()
    {
        for (int i = 0; i < tracks.Length; i++)
        {
            Track track = tracks[i];
            float value = track.slider.normalizedValue;
            float movement = Mathf.Abs(value - track.previousValue);

            if (movement > 0f)
            {
                activeTrack = i;

                track.progress = Mathf.Clamp01(
                    track.progress + movement / requiredMovement);
            }

            track.previousValue = value;
        }
    }

    private void UpdateAI()
    {
        Track track = tracks[activeTrack];

        if (track.blocks.fillAmount >= 1f &&
            activeTrack < tracks.Length - 1)
        {
            activeTrack++;
            track = tracks[activeTrack];
        }

        float value = 0.5f +
            Mathf.Sin(elapsed * 3f + activeTrack) * 0.45f;

        track.slider.SetValueWithoutNotify(value);

        track.progress = Mathf.Clamp01(
            track.progress + Time.deltaTime / aiSecondsPerTrack);
    }

    private void SetPlayhead(float progress)
    {
        Vector2 min = playhead.anchorMin;
        Vector2 max = playhead.anchorMax;

        min.x = progress;
        max.x = progress;

        playhead.anchorMin = min;
        playhead.anchorMax = max;

        Vector2 position = playhead.anchoredPosition;
        position.x = 0f;
        playhead.anchoredPosition = position;
    }

    private IEnumerator Complete()
    {
        foreach (Track track in tracks)
            track.slider.interactable = false;

        aiOverlay.SetActive(false);
        statusText.text = "Complete!";

        yield return new WaitForSeconds(2f);
        flowManager.CompleteSound();
    }
}