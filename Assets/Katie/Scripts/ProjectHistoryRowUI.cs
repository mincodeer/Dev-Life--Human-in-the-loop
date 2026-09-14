using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProjectHistoryRowUI : MonoBehaviour
{
    [Header("Row Button")]
    [SerializeField]
    private Button rowButton;

    [Header("Project Text")]
    [SerializeField]
    private TMP_Text projectNumberText;

    [SerializeField]
    private TMP_Text projectNameText;

    [SerializeField]
    private TMP_Text themeText;

    [SerializeField]
    private TMP_Text genreText;

    [Header("Result Text")]
    [SerializeField]
    private TMP_Text reviewText;

    [SerializeField]
    private TMP_Text moneyEarnedText;

    [SerializeField]
    private TMP_Text completedWeekText;

    private CompletedProjectRecord projectData;

    private void Awake()
    {
        if (rowButton == null)
        {
            rowButton = GetComponent<Button>();
        }
    }

    public void Setup(
        CompletedProjectRecord project,
        int projectNumber)
    {
        projectData = project;

        if (projectData == null)
        {
            Debug.LogWarning(
                "ProjectHistoryRowUI received null project data."
            );

            return;
        }

        SetText(
            projectNumberText,
            projectNumber.ToString()
        );

        SetText(
            projectNameText,
            projectData.projectName
        );

        SetText(
            themeText,
            projectData.theme.ToString().ToUpperInvariant()
        );

        SetText(
            genreText,
            projectData.genre.ToString().ToUpperInvariant()
        );

        int starRating = Mathf.Clamp(
            Mathf.FloorToInt(
                (projectData.finalScore / 20f) + 0.5f
            ),
            0,
            5
        );

        SetText(
            reviewText,
            starRating + " / 5"
        );

        SetText(
            moneyEarnedText,
            "$" + projectData.moneyEarned.ToString("N0")
        );

        SetText(
            completedWeekText,
            projectData.completedWeek.ToString()
        );

        if (rowButton != null)
        {
            rowButton.onClick.RemoveListener(
                SelectThisProject
            );

            rowButton.onClick.AddListener(
                SelectThisProject
            );
        }
    }

    private void SelectThisProject()
    {
        if (projectData == null)
        {
            return;
        }

        if (ProjectHistoryManager.Instance == null)
        {
            Debug.LogError(
                "ProjectHistoryManager could not be found."
            );

            return;
        }

        ProjectHistoryManager.Instance.SelectProject(
            projectData
        );
    }

    private void SetText(
        TMP_Text textField,
        string value)
    {
        if (textField != null)
        {
            textField.text = value;
        }
    }

    private void OnDestroy()
    {
        if (rowButton != null)
        {
            rowButton.onClick.RemoveListener(
                SelectThisProject
            );
        }
    }
}