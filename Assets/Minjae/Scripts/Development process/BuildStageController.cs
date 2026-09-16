using UnityEngine;
using UnityEngine.UI;

public class BuildStageController : MonoBehaviour
{
    [SerializeField] private DevelopmentFlowManager flowManager;
    [SerializeField] private Image animationImage;
    [SerializeField] private Button buildButton;
    [SerializeField] private Sprite[] frames;

    [SerializeField, Min(0.01f)] private float frameInterval = 0.3f;
    [SerializeField, Min(0f)] private float completionDelay = 2f;

    private int frameIndex;
    private float elapsed;
    private bool isBuilding;
    private bool isComplete;

    private void OnEnable()
    {
        if (frames == null || frames.Length == 0)
            return;

        animationImage.sprite = frames[frameIndex];
        buildButton.interactable = !isBuilding && !isComplete;
    }

    public void StartBuild()
    {
        if (isBuilding || isComplete ||
            frames == null || frames.Length == 0)
            return;

        isBuilding = true;
        elapsed = 0f;
        buildButton.interactable = false;
    }

    private void Update()
    {
        if (!isBuilding)
            return;

        elapsed += Time.deltaTime;

        // 마지막 이미지에서는 2초 기다린 뒤 결과로 이동.
        if (frameIndex == frames.Length - 1)
        {
            if (elapsed >= completionDelay)
            {
                isBuilding = false;
                isComplete = true;
                flowManager.GotoResult();
            }

            return;
        }

        if (elapsed >= frameInterval)
        {
            elapsed = 0f;
            frameIndex++;
            animationImage.sprite = frames[frameIndex];
        }
    }

    public void ResetBuild()
    {
        frameIndex = 0;
        elapsed = 0f;
        isBuilding = false;
        isComplete = false;

        if (frames != null && frames.Length > 0)
            animationImage.sprite = frames[0];

        buildButton.interactable = true;
    }
}