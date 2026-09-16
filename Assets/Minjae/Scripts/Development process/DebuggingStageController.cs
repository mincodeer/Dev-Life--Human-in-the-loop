using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DebuggingStageController : MonoBehaviour
{
    [SerializeField] private DevelopmentFlowManager flowManager;
    [SerializeField] private ProjectResourceController projectResourceController;
    [SerializeField] private GameObject modeChoicePanel;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private Button[] errorLines = new Button[4];
    [SerializeField, Range(0f, 1f)] private float manualFixChance = 0.8f;
    [SerializeField, Range(0f, 1f)] private float aiFixChance = 0.5f;
    [SerializeField, Min(1)] private int bugsPerSuccess = 1;
    [SerializeField, Min(0.01f)] private float aiInterval = 0.8f;
    [SerializeField, Min(0f)] private float completionDelay = 2f;
    [SerializeField] private GameObject aiOverlay;

    private bool[] resolved = new bool[4];
    private int resolvedCount;
    private bool running;
    private bool useAI;
    private bool finishing;
    private float elapsed;
    private int inputStartFrame;

    public bool CanBegin()
    {
        if (!isActiveAndEnabled || running || finishing || resolvedCount != 0)
            return false;
        if (flowManager == null || projectResourceController == null ||
            errorLines == null || errorLines.Length != 4) return false;
        for (int i = 0; i < 4; i++)
        {
            if (errorLines[i] == null) return false;
            for (int j = 0; j < i; j++)
                if (errorLines[i] == errorLines[j]) return false;
        }
        return true;
    }

    private void OnEnable()
    {
        inputStartFrame = Time.frameCount;
        RefreshView();
    }

    private void OnDisable()
    {
        if (aiOverlay != null)
            aiOverlay.SetActive(false);
    }

    public void Begin(bool ai)
    {
        if (!CanBegin()) return;
        useAI = ai;
        running = true;
        inputStartFrame = Time.frameCount;
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
        elapsed = 0f;
        RefreshView();
    }

    public void ClickLine(int index)
    {
        if (!isActiveAndEnabled || !running || useAI ||
            Time.frameCount <= inputStartFrame || Time.timeScale <= 0f) return;
        ResolveLine(index);
    }

    private void Update()
    {
        // Keyboard shortcuts choose a mode, but never resolve a red line.
        if (!running && !finishing && resolvedCount == 0)
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null || !Application.isFocused || Time.timeScale <= 0f ||
                Time.frameCount <= inputStartFrame || IsEditingText() || flowManager == null)
                return;

            if (keyboard.mKey.wasPressedThisFrame) flowManager.ChooseManual();
            else if (keyboard.aKey.wasPressedThisFrame) flowManager.ChooseAI();
            return;
        }

        if (finishing)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= completionDelay)
            {
                finishing = false;
                flowManager.CompleteDebugging();
            }
            return;
        }
        if (!running || !useAI) return;

        elapsed += Time.deltaTime;
        if (elapsed < aiInterval) return;
        elapsed = 0f;
        for (int i = 0; i < 4; i++)
        {
            if (resolved[i]) continue;
            ResolveLine(i);
            break;
        }
    }

    private void ResolveLine(int index)
    {
        if (index < 0 || index >= 4 || resolved[index]) return;
        resolved[index] = true;
        resolvedCount++;

        // The line always disappears. Actual bug removal is probabilistic.
        float chance = useAI ? aiFixChance : manualFixChance;
        if (Random.value < chance)
            projectResourceController.CurrentProjectConditions.ChangeBugs(-bugsPerSuccess);

        if (resolvedCount == 4)
        {
            running = false;
            finishing = true;
            elapsed = 0f;
        }
        RefreshView();
    }

    private void RefreshView()
    {
        if (aiOverlay != null)
            aiOverlay.SetActive(running && useAI);
        if (modeChoicePanel != null)
            modeChoicePanel.SetActive(!running && !finishing && resolvedCount == 0);
        if (errorLines != null)
        {
            for (int i = 0; i < Mathf.Min(4, errorLines.Length); i++)
            {
                if (errorLines[i] == null) continue;
                errorLines[i].gameObject.SetActive(!resolved[i]);
                errorLines[i].interactable = running && !useAI && !resolved[i];
            }
        }
        if (statusText != null)
        {
            statusText.enabled = true;
            statusText.text = resolvedCount == 4 ? "Debugging complete!" :
                !running ? "[M] Work manually    [A] Use AI" :
                useAI ? $"AI working...  {resolvedCount}/4" :
                $"Click red lines    Steps: {resolvedCount}/4";
        }
    }

    private bool IsEditingText()
    {
        if (EventSystem.current == null) return false;
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        return selected != null &&
            (selected.GetComponent<TMP_InputField>() != null ||
             selected.GetComponent<InputField>() != null);
    }

    public void ResetDebugging()
    {
        inputStartFrame = Time.frameCount;
        resolved = new bool[4];
        resolvedCount = 0;
        running = false;
        finishing = false;
        useAI = false;
        elapsed = 0f;
        RefreshView();
    }
}
