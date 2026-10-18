
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerTitleController : MonoBehaviour
{
    // Display the player's current title
    public TMP_Text titleText;
    public TMP_Text titleTextMini;

    // Display title progression
    public TMP_Text progressText;
    public TMP_Text progressTextMini;
    public TMP_Text nextTitleText;
    public TMP_Text skillValueText;
    public Slider titleProgressBar;

    // To-do list text
    public TMP_Text todoListText;

    // Player's current statistics
    public int developmentSkill = 0;
    public int gamesDeveloped = 0;
    public int manualDevelopment = 0;
    public int totalMoneyEarned = 0;

    // Tracks which Developer Title the player currently has
    // 0 = Beginner, 1 = Junior, 2 = Developer, etc.
    public int currentTitleLevel = 0;

    // List of Developer Titles in order
    private string[] titles =
    {
        "Beginner Developer",
        "Junior Developer",
        "Developer",
        "Senior Developer",
        "Expert Developer"
    };

    // Requirements needed to unlock each new title
    // Each array position corresponds to the current title level
    private int[] requiredSkill = { 20, 40, 60, 80 };
    private int[] requiredGames = { 1, 3, 6, 10 };
    private int[] requiredManual = { 3, 8, 15, 25 };
    private int[] requiredMoney = { 100, 500, 1500, 3000 };

    void Update()
    {
        // Check whether the player can unlock the next title
        CheckTitleProgression();

        // Update the title, progress bar and development skill UI
        UpdateTitleUI();

        // Update the To-Do List
        UpdateTodoList();
    }

    void CheckTitleProgression()
    {
        // Stop checking if the player has reached Expert Developer
        if (currentTitleLevel >= titles.Length - 1)
        {
            return;
        }

        int level = currentTitleLevel;

        // The player must complete every requirement
        // Having enough Development Skill alone is not enough
        if (developmentSkill >= requiredSkill[level] &&
            gamesDeveloped >= requiredGames[level] &&
            manualDevelopment >= requiredManual[level] &&
            totalMoneyEarned >= requiredMoney[level])
        {
            // Unlock the next Developer Title
            currentTitleLevel++;
        }
    }

    void UpdateTitleUI()
    {
        // Display the player's total Development Skill
        skillValueText.text = developmentSkill + " / 100";

        // Update both the full and minimised title displays
        titleText.text = titles[currentTitleLevel];
        titleTextMini.text = titles[currentTitleLevel];

        // Display final progression when Expert Developer is reached
        if (currentTitleLevel >= titles.Length - 1)
        {
            titleProgressBar.maxValue = 20;
            titleProgressBar.value = 20;
            progressText.text = "MAX";
            progressTextMini.text = "MAX";
            nextTitleText.text = "Maximum Title Reached";
            return;
        }

        // Calculate Development Skill earned towards the next title
        int previousSkill = currentTitleLevel * 20;

        int progress = Mathf.Clamp(
            developmentSkill - previousSkill, 0, 20
        );

        // Update the title progression bar
        titleProgressBar.maxValue = 20;
        titleProgressBar.value = progress;

        // Display progression on both UI versions
        progressText.text = progress + " / 20";
        progressTextMini.text = progress + " / 20";

        // Show which Developer Title can be unlocked next
        nextTitleText.text =
            "Next: " + titles[currentTitleLevel + 1];
    }

    void UpdateTodoList()
    {
        // Prevent errors if the To-Do List is not connected
        if (todoListText == null)
        {
            return;
        }

        // Display a completion message at Expert Developer
        if (currentTitleLevel >= titles.Length - 1)
        {
            todoListText.text =
                "All Developer Titles Unlocked!";
            return;
        }

        int level = currentTitleLevel;

        // Calculate Development Skill progress for the next title
        int previousSkill = level * 20;

        int skillProgress = Mathf.Clamp(
            developmentSkill - previousSkill, 0, 20
        );

        // Display Development Skill and other requirements
        // The values update automatically as the player progresses
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
        // Show [X] when a requirement is completed
        // Otherwise display [ ] for an incomplete requirement
        string status = current >= required ? "[X] " : "[ ] ";

        // Prevent completed requirements from displaying
        // values higher than the required amount
        return status + Mathf.Min(current, required) +
            "/" + required + " " + name + "\n";
    }

    public void AddGameDeveloped()
    {
        // Increase the number of completed games
        gamesDeveloped++;
    }

    public void AddManualDevelopment()
    {
        // Increase the number of manual development tasks
        manualDevelopment++;
    }

    public void AddMoneyEarned(int amount)
    {
        // Track money earned rather than current money balance
        // Spending money will not reduce completed progress
        if (amount > 0)
        {
            totalMoneyEarned += amount;
        }
    }

    public void AddDevelopmentSkill(int amount)
    {
        // Increase Development Skill while keeping it between 0 and 100
        developmentSkill =
            Mathf.Clamp(developmentSkill + amount, 0, 100);
    }
}
