using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Serialization;

// Reveals existing TMP blocks for the coding activity. DevelopmentFlowManager still owns stage order.
public class CodingStageController : MonoBehaviour
{
    private enum CodingState { Choosing, Manual, AI, Finishing }

    [Header("Connections")]
    [SerializeField] private DevelopmentFlowManager flowManager;
    [SerializeField] private GameObject modeChoicePanel;
    [SerializeField] private GameObject aiOverlay;
    [SerializeField] private TMP_Text statusText;

    [Header("Existing TMP objects, in reveal order (no blank entry)")]
    [SerializeField] private TMP_Text[] codeBlocks;
    [SerializeField] private TMP_Text consoleText;
    [FormerlySerializedAs("pressesPerFrame")]
    [SerializeField, Min(1)] private int pressesPerBlock = 5;
    [FormerlySerializedAs("aiFrameInterval")]
    [SerializeField, Min(0.05f)] private float aiBlockInterval = 0.8f;
    [SerializeField, Min(0f)] private float completionDelay = 0.7f;

    [Header("Audio (optional; use an always-active AudioSource)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip typingSound;
    [SerializeField] private AudioClip completeSound;

    private CodingState state;
    private int visibleBlockCount;
    private int pressCount;
    private int inputStartFrame;

    // Physical letter keys, not text entry. Space, numbers and Delete do not count.
    private static readonly Key[] LetterKeys = {
        Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G,
        Key.H, Key.I, Key.J, Key.K, Key.L, Key.M, Key.N,
        Key.O, Key.P, Key.Q, Key.R, Key.S, Key.T, Key.U,
        Key.V, Key.W, Key.X, Key.Y, Key.Z
    };

    private void OnEnable()
    {
        state = CodingState.Choosing;
        visibleBlockCount = 0;
        pressCount = 0;
        inputStartFrame = Time.frameCount;
        if (modeChoicePanel != null) modeChoicePanel.SetActive(true);
        if (aiOverlay != null) aiOverlay.SetActive(false);
        // Keep the text, colors and positions authored in the Inspector.
        // Only visibility changes; already revealed blocks remain visible.
        if (codeBlocks != null)
        {
            foreach (TMP_Text block in codeBlocks)
                if (block != null) block.enabled = false;
        }
        if (consoleText != null) consoleText.enabled = false;
        SetStatus("[M] Work manually    [A] Use AI");
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null || !Application.isFocused || Time.timeScale <= 0f)
            return;
        // Ignore the click/key frame that opened the panel or selected a mode.
        if (Time.frameCount <= inputStartFrame || IsEditingText()) return;

        if (state == CodingState.Choosing)
        {
            if (flowManager == null) return;
            if (keyboard.mKey.wasPressedThisFrame) flowManager.ChooseManual();
            else if (keyboard.aKey.wasPressedThisFrame) flowManager.ChooseAI();
            return;
        }

        if (state != CodingState.Manual) return;
        if (keyboard.ctrlKey.isPressed || keyboard.altKey.isPressed)
            return;

        foreach (Key key in LetterKeys)
        {
            if (!keyboard[key].wasPressedThisFrame) continue;
            RegisterPress();
            if (state != CodingState.Manual) break;
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

    // Called before saving the choice, so missing Inspector fields cannot save it early.
    public bool CanBegin()
    {
        if (!isActiveAndEnabled || state != CodingState.Choosing) return false;
        if (flowManager == null || modeChoicePanel == null || aiOverlay == null ||
            codeBlocks == null || codeBlocks.Length == 0)
        {
            Debug.LogError("Coding: assign the manager, panels and Code Blocks.", this);
            return false;
        }
        for (int i = 0; i < codeBlocks.Length; i++)
        {
            TMP_Text block = codeBlocks[i];
            if (block == null || !block.gameObject.activeInHierarchy ||
                block == statusText || block == consoleText ||
                block.transform.IsChildOf(modeChoicePanel.transform) ||
                block.transform.IsChildOf(aiOverlay.transform))
            {
                Debug.LogError("Coding: Code Blocks must be active, separate code TMP objects, outside the overlays.", this);
                return false;
            }
            for (int j = 0; j < i; j++)
            {
                if (codeBlocks[j] == block)
                {
                    Debug.LogError("Coding: the same TMP is assigned to more than one Code Blocks slot.", this);
                    return false;
                }
            }
        }
        if (consoleText != null && (consoleText == statusText ||
            !consoleText.gameObject.activeInHierarchy ||
            consoleText.transform.IsChildOf(modeChoicePanel.transform) ||
            consoleText.transform.IsChildOf(aiOverlay.transform)))
        {
            Debug.LogError("Coding: Console Text must be separate from Status Text and outside the overlays.", this);
            return false;
        }
        return true;
    }

    // Only the flow manager calls this, AFTER successfully saving the work method.
    public void Begin(bool useAI)
    {
        if (!CanBegin()) return;
        state = useAI ? CodingState.AI : CodingState.Manual;
        inputStartFrame = Time.frameCount;
        modeChoicePanel.SetActive(false);
        aiOverlay.SetActive(useAI);
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);

        RefreshStatus();
        if (useAI) StartCoroutine(RunAI());
    }

    private void RegisterPress()
    {
        pressCount++;
        PlaySound(typingSound);
        if (pressCount >= Mathf.Max(1, pressesPerBlock))
        {
            pressCount = 0;
            RevealNextBlock();
        }
        if (state == CodingState.Manual) RefreshStatus();
    }

    private void RevealNextBlock()
    {
        if (state != CodingState.Manual && state != CodingState.AI) return;
        if (visibleBlockCount >= codeBlocks.Length) return;

        codeBlocks[visibleBlockCount].enabled = true;
        visibleBlockCount++;
        RefreshStatus();
        if (visibleBlockCount == codeBlocks.Length)
        {
            state = CodingState.Finishing;
            StartCoroutine(FinishCoding());
        }
    }

    private IEnumerator RunAI()
    {
        while (state == CodingState.AI)
        {
            yield return new WaitForSeconds(Mathf.Max(0.05f, aiBlockInterval));
            if (state != CodingState.AI) yield break;
            RevealNextBlock();
        }
    }

    private IEnumerator FinishCoding()
    {
        aiOverlay.SetActive(false);
        if (consoleText != null) consoleText.enabled = true;
        SetStatus("Coding complete!");
        PlaySound(completeSound);
        yield return new WaitForSeconds(Mathf.Max(0f, completionDelay));
        flowManager.CompleteCoding();
    }

    private void RefreshStatus()
    {
        if (state == CodingState.AI)
            SetStatus("AI working...  " + visibleBlockCount + "/" + (codeBlocks.Length));
        else
            SetStatus("Type A-Z: " + pressCount + "/" + Mathf.Max(1, pressesPerBlock)
                + "    Steps: " + visibleBlockCount + "/" + (codeBlocks.Length));
    }

    private void SetStatus(string message)
    {
        if (statusText != null) statusText.text = message;
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null) audioSource.PlayOneShot(clip);
    }
}
