// Student ID: 23208000

using TMPro;
using UnityEngine;

// Updates the Project Conditions UI with the latest values
// and highlights dangerous conditions.
public class ProjectConditionsUI : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField]
    private TextMeshProUGUI qualityText;

    [SerializeField]
    private TextMeshProUGUI workloadText;

    [SerializeField]
    private TextMeshProUGUI technicalDebtText;

    [SerializeField]
    private TextMeshProUGUI timeText;

    [Header("Resource References")]
    [SerializeField]
    private ProjectResourceController projectResourceController;

    private void Update()
    {
        UpdateConditionsUI();
    }

    // Updates all project condition values shown on the UI.
    private void UpdateConditionsUI()
    {
        if (projectResourceController == null)
        {
            return;
        }

        ProjectConditions conditions =
            projectResourceController.CurrentProjectConditions;

        qualityText.text =
            "Quality: "
            + conditions.quality
            + " / 100";

        workloadText.text =
            "Workload: "
            + conditions.workload
            + " / 100";

        timeText.text =
            "Development Time: "
            + conditions.totalTime
            + " days";

        if (ResourceManager.Instance != null)
        {
            technicalDebtText.text =
                "Technical Debt: "
                + ResourceManager.Instance.TechnicalDebt
                + " / 100";
        }

        UpdateWarningColours(
            conditions.quality,
            conditions.workload);
    }

    // Updates the colours based on the current condition values.
    private void UpdateWarningColours(
        float quality,
        float workload)
    {
        UpdateQualityColour(quality);
        UpdateWorkloadColour(workload);

        if (ResourceManager.Instance != null)
        {
            UpdateTechnicalDebtColour(
                ResourceManager.Instance.TechnicalDebt);
        }
    }

    // Quality becomes dangerous when the value is low.
    private void UpdateQualityColour(
        float quality)
    {
        if (quality < 30)
        {
            qualityText.color = Color.red;
        }
        else if (quality < 60)
        {
            qualityText.color = Color.yellow;
        }
        else
        {
            qualityText.color = Color.white;
        }
    }

    // Workload becomes dangerous when the value is high.
    private void UpdateWorkloadColour(
        float workload)
    {
        if (workload >= 80)
        {
            workloadText.color = Color.red;
        }
        else if (workload >= 60)
        {
            workloadText.color = Color.yellow;
        }
        else
        {
            workloadText.color = Color.white;
        }
    }

    // Technical Debt becomes dangerous when the value is high.
    private void UpdateTechnicalDebtColour(
        int technicalDebt)
    {
        if (technicalDebt >= 80)
        {
            technicalDebtText.color = Color.red;
        }
        else if (technicalDebt >= 60)
        {
            technicalDebtText.color = Color.yellow;
        }
        else
        {
            technicalDebtText.color = Color.white;
        }
    }
}