using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Keep the room and all overlay UI inside the same 16:9 play area.
// Preserve each Canvas's authored scale instead of changing scene layouts.
[DefaultExecutionOrder(10000)]
public sealed class FixedGameViewport : MonoBehaviour
{
    private const float DesignWidth = 1920f;
    private const float DesignHeight = 1080f;
    private static FixedGameViewport instance;

    private sealed class CanvasLayout
    {
        public Canvas canvas;
        public float designScale;
    }

    private readonly List<CanvasLayout> layouts = new List<CanvasLayout>();
    private readonly Dictionary<Camera, Rect> cameraRects = new Dictionary<Camera, Rect>();
    private Canvas borderCanvas;
    private RectTransform[] borders;
    private int lastWidth;
    private int lastHeight;
    private float nextScan;
    private bool needsScan = true;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatic() => instance = null;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Install()
    {
        if (instance == null)
            new GameObject("Game Viewport").AddComponent<FixedGameViewport>();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        CreateBorders();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => needsScan = true;

    private void LateUpdate()
    {
        if (Screen.width <= 0 || Screen.height <= 0) return;
        bool resized = lastWidth != Screen.width || lastHeight != Screen.height;
        if (needsScan || resized || Time.unscaledTime >= nextScan)
        {
            RegisterSceneObjects();
            nextScan = Time.unscaledTime + 0.5f;
            needsScan = false;
        }

        Rect viewport = CalculateViewport(Screen.width, Screen.height);
        float fitScale = Mathf.Min(Screen.width / DesignWidth, Screen.height / DesignHeight);
        layouts.RemoveAll(layout => layout.canvas == null);
        foreach (CanvasLayout layout in layouts)
            layout.canvas.scaleFactor = layout.designScale * fitScale;

        // Reapply after other camera scripts, without changing zoom or position.
        foreach (var pair in cameraRects)
        {
            if (pair.Key == null) continue;
            Rect original = pair.Value;
            pair.Key.rect = new Rect(viewport.x + original.x * viewport.width,
                viewport.y + original.y * viewport.height,
                original.width * viewport.width, original.height * viewport.height);
        }
        SetBorders(viewport);
        lastWidth = Screen.width;
        lastHeight = Screen.height;
    }

    public static Rect CalculateViewport(int width, int height)
    {
        if (width <= 0 || height <= 0) return new Rect(0f, 0f, 1f, 1f);
        float scale = Mathf.Min(width / DesignWidth, height / DesignHeight);
        float w = DesignWidth * scale / width;
        float h = DesignHeight * scale / height;
        return new Rect((1f - w) * 0.5f, (1f - h) * 0.5f, w, h);
    }

    private void RegisterSceneObjects()
    {
        foreach (Camera camera in FindObjectsByType<Camera>(FindObjectsInactive.Include,
            FindObjectsSortMode.None))
        {
            if (camera.cameraType == CameraType.Game && camera.targetTexture == null &&
                camera.targetDisplay == 0 && !cameraRects.ContainsKey(camera))
                cameraRects.Add(camera, camera.rect);
        }

        foreach (Canvas canvas in FindObjectsByType<Canvas>(FindObjectsInactive.Include,
            FindObjectsSortMode.None))
        {
            if (canvas == borderCanvas || !canvas.isRootCanvas || canvas.targetDisplay != 0 ||
                canvas.renderMode != RenderMode.ScreenSpaceOverlay ||
                layouts.Exists(layout => layout.canvas == canvas)) continue;

            CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
            float designScale = GetDesignScale(scaler, canvas.scaleFactor);
            float pixelsPerUnit = scaler != null ? scaler.referencePixelsPerUnit : canvas.referencePixelsPerUnit;

            // Save direct children before inserting the new, fixed-size parent.
            var children = new List<Transform>();
            foreach (Transform child in canvas.transform) children.Add(child);
            RectTransform area = new GameObject("16x9 Content", typeof(RectTransform)).GetComponent<RectTransform>();
            area.SetParent(canvas.transform, false);
            area.anchorMin = area.anchorMax = area.pivot = new Vector2(0.5f, 0.5f);
            area.sizeDelta = new Vector2(DesignWidth, DesignHeight) / designScale;
            area.anchoredPosition = Vector2.zero;
            foreach (Transform child in children) MoveWithoutChangingLayout(child, area);

            // CanvasScaler otherwise scales using the entire monitor, including the borders.
            if (scaler != null) scaler.enabled = false;
            canvas.referencePixelsPerUnit = pixelsPerUnit;
            layouts.Add(new CanvasLayout { canvas = canvas, designScale = designScale });
        }
    }

    private static float GetDesignScale(CanvasScaler scaler, float fallback)
    {
        if (scaler == null || !scaler.enabled) return Mathf.Max(0.001f, fallback);
        if (scaler.uiScaleMode != CanvasScaler.ScaleMode.ScaleWithScreenSize)
            return Mathf.Max(0.001f, scaler.scaleFactor);
        Vector2 reference = scaler.referenceResolution;
        float width = DesignWidth / Mathf.Max(1f, reference.x);
        float height = DesignHeight / Mathf.Max(1f, reference.y);
        switch (scaler.screenMatchMode)
        {
            case CanvasScaler.ScreenMatchMode.Expand: return Mathf.Min(width, height);
            case CanvasScaler.ScreenMatchMode.Shrink: return Mathf.Max(width, height);
            default:
                return Mathf.Pow(2f, Mathf.Lerp(Mathf.Log(width, 2f), Mathf.Log(height, 2f),
                    scaler.matchWidthOrHeight));
        }
    }

    private static void MoveWithoutChangingLayout(Transform child, RectTransform parent)
    {
        RectTransform rect = child as RectTransform;
        if (rect == null) { child.SetParent(parent, false); return; }
        Vector2 min = rect.anchorMin, max = rect.anchorMax, pivot = rect.pivot, size = rect.sizeDelta;
        Vector3 position = rect.anchoredPosition3D, scale = rect.localScale;
        Quaternion rotation = rect.localRotation;
        rect.SetParent(parent, false);
        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.pivot = pivot;
        rect.sizeDelta = size;
        rect.anchoredPosition3D = position;
        rect.localScale = scale;
        rect.localRotation = rotation;
    }

    private void CreateBorders()
    {
        var root = new GameObject("Screen Borders", typeof(RectTransform), typeof(Canvas), typeof(GraphicRaycaster));
        root.transform.SetParent(transform, false);
        borderCanvas = root.GetComponent<Canvas>();
        borderCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        borderCanvas.sortingOrder = short.MaxValue;
        borders = new RectTransform[4];
        string[] names = { "Left", "Right", "Bottom", "Top" };
        for (int i = 0; i < borders.Length; i++)
        {
            var edge = new GameObject(names[i], typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            edge.transform.SetParent(root.transform, false);
            edge.GetComponent<Image>().color = Color.black;
            borders[i] = edge.GetComponent<RectTransform>();
        }
    }

    private void SetBorders(Rect viewport)
    {
        SetBorder(borders[0], new Vector2(0f, 0f), new Vector2(viewport.xMin, 1f));
        SetBorder(borders[1], new Vector2(viewport.xMax, 0f), new Vector2(1f, 1f));
        SetBorder(borders[2], new Vector2(viewport.xMin, 0f), new Vector2(viewport.xMax, viewport.yMin));
        SetBorder(borders[3], new Vector2(viewport.xMin, viewport.yMax), new Vector2(viewport.xMax, 1f));
    }

    private static void SetBorder(RectTransform border, Vector2 min, Vector2 max)
    {
        border.anchorMin = min;
        border.anchorMax = max;
        border.offsetMin = border.offsetMax = Vector2.zero;
        border.gameObject.SetActive(max.x - min.x > 0.0001f && max.y - min.y > 0.0001f);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (instance == this) instance = null;
    }
}
