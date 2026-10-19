
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerTitleController : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text titleTextMini;

    public TMP_Text progressText;
    public TMP_Text progressTextMini;
    public TMP_Text nextTitleText;
    public TMP_Text skillValueText;
    public Slider titleProgressBar;

    public TMP_Text todoListText;

    public int gamesDeveloped = 0;
    public int manualDevelopment = 0;
    public int totalMoneyEarned = 0;
    public int currentTitleLevel = 0;

    private string[] titles =
    {
        "Beginner Developer",
        "Junior Developer",
        "Developer",
        "Senior Developer",
        "Expert Developer"
    };

    private int[] requiredSkill = { 20, 40, 60, 80 };
    private int[] requiredGames = { 1, 3, 6, 10 };
    private int[] requiredManual = { 3, 8, 15, 25 };
    private int[] requiredMoney = { 100, 500, 1500, 3000 };

    void Update()
    {
        if (ResourceManager.Instance == null)
        {
            return;
        }

        int developmentSkill =
            ResourceManager.Instance.DevelopmentSkill;

        CheckTitleProgression(developmentSkill);
        UpdateTitleUI(developmentSkill);
        UpdateTodoList(developmentSkill);
    }

    void CheckTitleProgression(int developmentSkill)
    {
        while (currentTitleLevel < titles.Length - 1)
        {
            int level = currentTitleLevel;

            if (developmentSkill >= requiredSkill[level] &&
                gamesDeveloped >= requiredGames[level] &&
                manualDevelopment >= requiredManual[level] &&
                totalMoneyEarned >= requiredMoney[level])
            {
                currentTitleLevel++;
            }
            else
            {
                break;
            }
        }
    }

    void UpdateTitleUI(int developmentSkill)
    {
        if (skillValueText != null)
        {
            skillValueText.text = developmentSkill + " / 100";
        }

        if (titleText != null)
        {
            titleText.text = titles[currentTitleLevel];
        }

        if (titleTextMini != null)
        {
            titleTextMini.text = titles[currentTitleLevel];
        }

        if (currentTitleLevel >= titles.Length - 1)
        {
            if (titleProgressBar != null)
            {
                titleProgressBar.maxValue = 20;
                titleProgressBar.value = 20;
            }

            if (progressText != null)
            {
                progressText.text = "MAX";
            }

            if (progressTextMini != null)
            {
                progressTextMini.text = "MAX";
            }

            if (nextTitleText != null)
            {
                nextTitleText.text = "Maximum Title Reached";
            }

            return;
        }

        int previousSkill = currentTitleLevel * 20;

        int progress = Mathf.Clamp(
            developmentSkill - previousSkill, 0, 20
        );

        if (titleProgressBar != null)
        {
            titleProgressBar.maxValue = 20;
            titleProgressBar.value = progress;
        }

        if (progressText != null)
        {
            progressText.text = progress + " / 20";
        }

        if (progressTextMini != null)
        {
            progressTextMini.text = progress + " / 20";
        }

        if (nextTitleText != null)
        {
            nextTitleText.text =
                "Next: " + titles[currentTitleLevel + 1];
        }
    }

    void UpdateTodoList(int developmentSkill)
    {
        if (todoListText == null)
        {
            return;
        }

        if (currentTitleLevel >= titles.Length - 1)
        {
            todoListText.text =
                "All Developer Titles Unlocked!";
            return;
        }

        int level = currentTitleLevel;

        int previousSkill = level * 20;

        int skillProgress = Mathf.Clamp(
            developmentSkill - previousSkill, 0, 20
        );

        todoListText.text =
            GetRequirement(
                skillProgress,
                20,
                "Development Skill") +
            GetRequirement(
                gamesDeveloped,
                requiredGames[level],
                "Games Developed") +
            GetRequirement(
                manualDevelopment,
                requiredManual[level],
                "Manual Development") +
            GetRequirement(
                totalMoneyEarned,
                requiredMoney[level],
                "Dollars Earned");
    }

    string GetRequirement(int current, int required, string name)
    {
        string status = current >= required ? "[X] " : "[ ] ";

        return status + Mathf.Min(current, required) +
            "/" + required + " " + name + "\n";
    }

    public void AddGameDeveloped()
    {
        gamesDeveloped++;
    }

    public void AddManualDevelopment()
    {
        manualDevelopment++;
    }

    public void AddMoneyEarned(int amount)
    {
        if (amount > 0)
        {
            totalMoneyEarned += amount;
        }
    }


    public void AddDevelopmentSkill(int amount)
    {
        if (ResourceManager.Instance == null)
        {
            return;
        }

        ResourceManager.Instance.ChangeDevelopmentSkill(amount);
    }
}

