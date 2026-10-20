using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

// Temporary local verification tool. Not part of the submitted game.
public static class ViewportLayoutCheck
{
    [MenuItem("Tools/Dev Life/Check Display Layouts")]
    private static void Run()
    {
        if (!EditorApplication.isPlaying) throw new InvalidOperationException("Enter Play Mode first.");
        new GameObject("Display Check Runner").AddComponent<ViewportLayoutCheckRunner>();
    }
}

public sealed class ViewportLayoutCheckRunner : MonoBehaviour
{
    private const string Output = "C:/Users/User/.codex/.chatgpt-projects/g-p-6a580943c498819189def41382a12d02/tmp/display-fix/screens";
    private readonly StringBuilder report = new StringBuilder();
    private EditorWindow view;
    private PropertyInfo sizeIndex;
    private int originalIndex;

    private IEnumerator Start()
    {
        Directory.CreateDirectory(Output);
        Type viewType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
        view = EditorWindow.GetWindow(viewType);
        sizeIndex = viewType.GetProperty("selectedSizeIndex", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        originalIndex = (int)sizeIndex.GetValue(view);
        PauseController pause = FindFirstObjectByType<PauseController>();
        if (pause != null) pause.PauseGame();
        int[,] sizes = { {1280,720}, {2520,1080}, {1024,768} };
        for (int i = 0; i < sizes.GetLength(0); i++)
        {
            SetResolution(sizes[i,0], sizes[i,1]);
            yield return new WaitForSecondsRealtime(0.8f);
            yield return new WaitForEndOfFrame();
            Capture("pause-" + sizes[i,0] + "x" + sizes[i,1]);
        }
        if (pause != null) pause.ResumeGame();
        if (EscapeInputRouter.Instance != null) EscapeInputRouter.Instance.CloseSettings();
        for (int i = 0; i < sizes.GetLength(0); i++)
        {
            SetResolution(sizes[i,0], sizes[i,1]);
            if (EscapeInputRouter.Instance != null) EscapeInputRouter.Instance.OpenSettings();
            yield return new WaitForSecondsRealtime(0.8f);
            yield return new WaitForEndOfFrame();
            Capture("settings-" + sizes[i,0] + "x" + sizes[i,1]);
            if (EscapeInputRouter.Instance != null) EscapeInputRouter.Instance.CloseSettings();
        }
        FeatureSceneNavigation navigation = FeatureSceneNavigation.Instance;
        if (navigation != null)
        {
            SetResolution(2520,1080);
            navigation.OpenShop();
            yield return new WaitForSecondsRealtime(1.5f);
            if (MainGameTutorialManager.Instance != null) MainGameTutorialManager.Instance.CloseTutorial();
            yield return new WaitForEndOfFrame();
            Capture("shop-2520x1080");
            navigation.BackToRoom();
            yield return new WaitForSecondsRealtime(0.8f);
            SetResolution(1024,768);
            navigation.OpenDashboard();
            yield return new WaitForSecondsRealtime(1.5f);
            if (MainGameTutorialManager.Instance != null) MainGameTutorialManager.Instance.CloseTutorial();
            yield return new WaitForEndOfFrame();
            Capture("dashboard-1024x768");
            navigation.BackToRoom();
            yield return new WaitForSecondsRealtime(0.8f);
        }
        sizeIndex.SetValue(view, originalIndex);
        File.WriteAllText(Path.Combine(Output,"report.txt"),report.ToString());
        Debug.Log("Display layout checks completed. " + Output + "/report.txt");
        Destroy(gameObject);
    }

    private void SetResolution(int width, int height)
    {
        Assembly assembly = typeof(Editor).Assembly;
        Type sizesType = assembly.GetType("UnityEditor.GameViewSizes");
        Type singleton = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
        object sizes = singleton.GetProperty("instance").GetValue(null);
        MethodInfo getGroup = sizesType.GetMethod("GetGroup");
        object group = getGroup.Invoke(sizes,new[]{Enum.Parse(getGroup.GetParameters()[0].ParameterType,"Standalone")});
        Type sizeType = assembly.GetType("UnityEditor.GameViewSize");
        Type kindType = assembly.GetType("UnityEditor.GameViewSizeType");
        object size = Activator.CreateInstance(sizeType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            null,new[]{Enum.Parse(kindType,"FixedResolution"),(object)width,height,"Display Check " + width + "x" + height},null);
        group.GetType().GetMethod("AddCustomSize").Invoke(group,new[]{size});
        int builtin = (int)group.GetType().GetMethod("GetBuiltinCount").Invoke(group,null);
        int custom = (int)group.GetType().GetMethod("GetCustomCount").Invoke(group,null);
        sizeIndex.SetValue(view,builtin+custom-1);
        view.Focus();
        view.Repaint();
    }

    private void Capture(string label)
    {
        Rect expected = FixedGameViewport.CalculateViewport(Screen.width,Screen.height);
        report.AppendLine(label + " actual=" + Screen.width + "x" + Screen.height + " viewport=" + expected);
        foreach (Canvas canvas in FindObjectsByType<Canvas>(FindObjectsInactive.Include,FindObjectsSortMode.None))
        {
            if (!canvas.isActiveAndEnabled) { report.AppendLine("SKIP inactive " + canvas.name); continue; }
            if (!canvas.isRootCanvas || canvas.renderMode != RenderMode.ScreenSpaceOverlay || canvas.name == "Screen Borders") continue;
            Transform area = canvas.transform.Find("16x9 Content");
            if (area == null) { report.AppendLine("FAIL missing content area " + canvas.name); continue; }
            var corners = new Vector3[4];
            ((RectTransform)area).GetWorldCorners(corners);
            Vector2 min = RectTransformUtility.WorldToScreenPoint(null,corners[0]);
            Vector2 max = RectTransformUtility.WorldToScreenPoint(null,corners[2]);
            bool aligned = Vector2.Distance(min,new Vector2(expected.xMin*Screen.width,expected.yMin*Screen.height)) < 1f &&
                Vector2.Distance(max,new Vector2(expected.xMax*Screen.width,expected.yMax*Screen.height)) < 1f;
            report.AppendLine((aligned ? "PASS " : "FAIL ") + canvas.name + " bounds=" + min + " .. " + max);
        }
        Texture2D image = ScreenCapture.CaptureScreenshotAsTexture();
        File.WriteAllBytes(Path.Combine(Output,label+".png"),image.EncodeToPNG());
        Destroy(image);
    }
}

