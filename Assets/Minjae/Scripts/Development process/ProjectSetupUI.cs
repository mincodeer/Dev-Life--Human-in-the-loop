using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Bind scene-authored UI. Appearance and layout belong to the Inspector.
public class ProjectSetupUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private ProjectSetupDropdown themeDropdown;
    [SerializeField] private ProjectSetupDropdown genreDropdown;
    [SerializeField] private GameObject requirementPanel;
    [SerializeField] private TMP_Text requirementText;
    [SerializeField] private Button requirementCloseButton;
    private ProjectDataManager dataManager;
    private GameObject returnSelection;

    public bool IsThemeLocked(int index) => themeDropdown != null && themeDropdown.IsOptionLocked(index);
    public bool IsGenreLocked(int index) => genreDropdown != null && genreDropdown.IsOptionLocked(index);

    private void Awake()
    {
        if (requirementPanel != null) requirementPanel.SetActive(false);
        if (nameInput == null || requirementPanel == null || requirementText == null ||
            requirementCloseButton == null || themeDropdown == null || genreDropdown == null)
            Debug.LogWarning("ProjectSetupUI: connect the scene-authored name input, dropdowns and requirement panel/text/close button in the Inspector.", this);
    }

    private void OnEnable()
    {
        if (nameInput != null)
        {
            nameInput.onValueChanged.AddListener(OnNameChanged);
            nameInput.onEndEdit.AddListener(OnNameEditEnded);
        }
        if (requirementCloseButton != null) requirementCloseButton.onClick.AddListener(CloseRequirement);
        if (themeDropdown != null) themeDropdown.LockedOptionClicked += OnThemeLocked;
        if (genreDropdown != null) genreDropdown.LockedOptionClicked += OnGenreLocked;
        dataManager = ProjectDataManager.Instance;
        if (dataManager != null) dataManager.ProjectChanged += RefreshFromProject;
        RefreshFromProject();
    }

    private void Start() => RefreshFromProject();

    private void OnDisable()
    {
        if (nameInput != null)
        {
            nameInput.onValueChanged.RemoveListener(OnNameChanged);
            nameInput.onEndEdit.RemoveListener(OnNameEditEnded);
        }
        if (requirementCloseButton != null) requirementCloseButton.onClick.RemoveListener(CloseRequirement);
        if (themeDropdown != null) themeDropdown.LockedOptionClicked -= OnThemeLocked;
        if (genreDropdown != null) genreDropdown.LockedOptionClicked -= OnGenreLocked;
        if (dataManager != null) dataManager.ProjectChanged -= RefreshFromProject;
        if (requirementPanel != null) requirementPanel.SetActive(false);
    }

    public void RefreshFromProject()
    {
        if (ProjectDataManager.Instance == null) return;
        ProjectData project = ProjectDataManager.Instance.CurrentProject;
        if (nameInput != null) nameInput.SetTextWithoutNotify(project.projectName == "Untitled Project" ? "" : project.projectName);
        if (themeDropdown != null) themeDropdown.SetValueWithoutNotify((int)project.selectedTheme);
        if (genreDropdown != null) genreDropdown.SetValueWithoutNotify((int)project.selectedGenre);
    }

    private void OnNameChanged(string value)
    {
        if (ProjectDataManager.Instance != null) ProjectDataManager.Instance.SetProjectName(value);
    }

    private void OnNameEditEnded(string value)
    {
        OnNameChanged(value);
        string normalized = ProjectDataManager.NormalizeProjectName(value);
        nameInput.SetTextWithoutNotify(normalized == "Untitled Project" ? "" : normalized);
    }

    private void OnThemeLocked(string message) => ShowRequirement(message, themeDropdown.gameObject);
    private void OnGenreLocked(string message) => ShowRequirement(message, genreDropdown.gameObject);

    private void ShowRequirement(string message, GameObject source)
    {
        if (requirementPanel == null || requirementText == null) return;
        returnSelection = source;
        requirementText.text = message;
        requirementPanel.SetActive(true);
        if (EventSystem.current != null && requirementCloseButton != null)
            EventSystem.current.SetSelectedGameObject(requirementCloseButton.gameObject);
    }

    public void CloseRequirement()
    {
        if (requirementPanel != null) requirementPanel.SetActive(false);
        if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(returnSelection);
    }
}
