using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Uses the existing title position, font and dropdowns. Generated UI is owned by
// this panel, so closing the computer also closes the requirement message.
public class ProjectSetupUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private ProjectSetupDropdown themeDropdown;
    [SerializeField] private ProjectSetupDropdown genreDropdown;

    private TMP_InputField nameInput;
    private GameObject requirementPanel;
    private TMP_Text requirementText;
    private ProjectDataManager dataManager;
    private GameObject returnSelection;

    public bool IsThemeLocked(int index) => themeDropdown != null && themeDropdown.IsOptionLocked(index);
    public bool IsGenreLocked(int index) => genreDropdown != null && genreDropdown.IsOptionLocked(index);

    private void Awake()
    {
        if (titleText == null || themeDropdown == null || genreDropdown == null)
        {
            Debug.LogError("Connect the Project Setup title, Theme and Genre dropdowns.", this);
            enabled = false;
            return;
        }
        CreateNameInput();
        CreateRequirementPanel();
        themeDropdown.LockedOptionClicked = message => ShowRequirement(message, themeDropdown.gameObject);
        genreDropdown.LockedOptionClicked = message => ShowRequirement(message, genreDropdown.gameObject);
    }

    private void OnEnable()
    {
        dataManager = ProjectDataManager.Instance;
        if (dataManager != null) dataManager.ProjectChanged += RefreshFromProject;
        RefreshFromProject();
    }

    private void OnDisable()
    {
        if (dataManager != null) dataManager.ProjectChanged -= RefreshFromProject;
        if (requirementPanel != null) requirementPanel.SetActive(false);
    }

    public void RefreshFromProject()
    {
        if (nameInput == null || ProjectDataManager.Instance == null) return;
        ProjectData project = ProjectDataManager.Instance.CurrentProject;
        nameInput.SetTextWithoutNotify(project.projectName == "Untitled Project" ? "" : project.projectName);
        themeDropdown.SetValueWithoutNotify((int)project.selectedTheme);
        genreDropdown.SetValueWithoutNotify((int)project.selectedGenre);
    }

    private void OnNameChanged(string value)
    {
        if (ProjectDataManager.Instance != null)
            ProjectDataManager.Instance.SetProjectName(value);
    }

    private void OnNameEditEnded(string value)
    {
        OnNameChanged(value);
        string normalized = ProjectDataManager.NormalizeProjectName(value);
        nameInput.SetTextWithoutNotify(normalized == "Untitled Project" ? "" : normalized);
    }

    private void CreateNameInput()
    {
        GameObject field = new GameObject("Project Name Input", typeof(RectTransform), typeof(Image));
        RectTransform rect = (RectTransform)field.transform;
        rect.SetParent(titleText.transform.parent, false);
        RectTransform source = titleText.rectTransform;
        rect.anchorMin = source.anchorMin;
        rect.anchorMax = source.anchorMax;
        rect.pivot = source.pivot;
        rect.anchoredPosition = source.anchoredPosition;
        rect.sizeDelta = source.sizeDelta;
        rect.localScale = source.localScale;
        field.GetComponent<Image>().color = new Color(0.94f, 0.94f, 0.94f, 0.95f);

        RectTransform viewport = NewRect("Text Area", rect);
        Stretch(viewport, 0f);
        viewport.offsetMin = new Vector2(12f, 4f);
        viewport.offsetMax = new Vector2(-12f, -4f);
        viewport.gameObject.AddComponent<RectMask2D>();
        TMP_Text text = NewText("Text", viewport, "", 30f);
        text.textWrappingMode = TextWrappingModes.NoWrap;
        text.richText = false;
        TMP_Text placeholder = NewText("Placeholder", viewport, "Untitled Project", 30f);
        placeholder.color = new Color(0.2f, 0.2f, 0.2f, 0.55f);
        placeholder.textWrappingMode = TextWrappingModes.NoWrap;

        nameInput = field.AddComponent<TMP_InputField>();
        nameInput.textViewport = viewport;
        nameInput.textComponent = (TextMeshProUGUI)text;
        nameInput.placeholder = placeholder;
        nameInput.targetGraphic = field.GetComponent<Image>();
        nameInput.lineType = TMP_InputField.LineType.SingleLine;
        nameInput.richText = false;
        nameInput.onValueChanged.AddListener(OnNameChanged);
        nameInput.onEndEdit.AddListener(OnNameEditEnded);
        titleText.gameObject.SetActive(false);
    }

    private void CreateRequirementPanel()
    {
        RectTransform overlay = NewRect("Unlock Requirement Panel", (RectTransform)transform);
        Stretch(overlay, 0f);
        overlay.gameObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.45f);
        requirementPanel = overlay.gameObject;

        RectTransform card = NewRect("Card", overlay);
        card.sizeDelta = new Vector2(500f, 220f);
        card.gameObject.AddComponent<Image>().color = new Color(0.94f, 0.94f, 0.94f, 1f);
        requirementText = NewText("Requirement", card, "", 24f);
        requirementText.rectTransform.offsetMin = new Vector2(24f, 70f);
        requirementText.rectTransform.offsetMax = new Vector2(-24f, -20f);

        RectTransform closeRect = NewRect("Close", card);
        closeRect.anchorMin = closeRect.anchorMax = new Vector2(0.5f, 0f);
        closeRect.anchoredPosition = new Vector2(0f, 35f);
        closeRect.sizeDelta = new Vector2(130f, 40f);
        Image closeImage = closeRect.gameObject.AddComponent<Image>();
        closeImage.color = new Color(0.75f, 0.75f, 0.75f, 1f);
        Button closeButton = closeRect.gameObject.AddComponent<Button>();
        closeButton.targetGraphic = closeImage;
        closeButton.onClick.AddListener(CloseRequirement);
        NewText("Label", closeRect, "Close", 22f);
        requirementPanel.SetActive(false);
    }

    private void ShowRequirement(string message, GameObject source)
    {
        returnSelection = source;
        requirementText.text = message;
        requirementPanel.SetActive(true);
        requirementPanel.transform.SetAsLastSibling();
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(requirementPanel.GetComponentInChildren<Button>().gameObject);
    }

    public void CloseRequirement()
    {
        requirementPanel.SetActive(false);
        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(returnSelection);
    }

    private TMP_Text NewText(string objectName, RectTransform parent, string value, float size)
    {
        RectTransform rect = NewRect(objectName, parent);
        Stretch(rect, 0f);
        TextMeshProUGUI text = rect.gameObject.AddComponent<TextMeshProUGUI>();
        text.font = titleText.font;
        text.fontSize = size;
        text.fontStyle = FontStyles.Normal;
        text.alignment = TextAlignmentOptions.Center;
        text.color = new Color(0.15f, 0.15f, 0.15f, 1f);
        text.raycastTarget = false;
        text.richText = false;
        text.text = value;
        return text;
    }

    private static RectTransform NewRect(string objectName, RectTransform parent)
    {
        var rect = (RectTransform)new GameObject(objectName, typeof(RectTransform)).transform;
        rect.SetParent(parent, false);
        return rect;
    }

    private static void Stretch(RectTransform rect, float padding)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.one * padding;
        rect.offsetMax = -Vector2.one * padding;
    }
}
