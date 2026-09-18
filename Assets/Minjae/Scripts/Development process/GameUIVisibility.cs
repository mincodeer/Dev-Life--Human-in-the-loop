using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class GameUIVisibility : MonoBehaviour
{
    [SerializeField, Min(0f)] private float fadeDuration = 0.35f;
    private CanvasGroup group;
    private Coroutine fade;

    private void Awake() { Hide(); }

    public void Hide()
    {
        if (group == null) group = GetComponent<CanvasGroup>();
        if (fade != null) StopCoroutine(fade);
        fade = null;
        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;
    }

    public void Show()
    {
        if (group == null) group = GetComponent<CanvasGroup>();
        if (fade != null) StopCoroutine(fade);
        fade = StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float startAlpha = group.alpha;
        float elapsed = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(startAlpha, 1f,
                Mathf.Clamp01(elapsed / fadeDuration));
            yield return null;
        }
        group.alpha = 1f;
        group.interactable = true;
        group.blocksRaycasts = true;
        fade = null;
    }
}
