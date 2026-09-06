//Student ID:23208000

using UnityEngine;

/// <summary>
/// Manages the player's permanent studio resources.
/// These resources persist between projects and scenes.
/// </summary>
public class ResourceManager : MonoBehaviour
{
    // Singleton so other systems can access the player's resources.
    public static ResourceManager Instance
    {
        get;
        private set;
    }

    [Header("Player Resources")]

    [SerializeField]
    private int money = 250;

    [SerializeField]
    private int fandom = 0;

    [SerializeField]
    [Range(0, 100)]
    private int developmentSkill = 10;

    [SerializeField]
    [Range(0, 100)]
    private int technicalDebt = 0;

    // Public read-only access to the resources.
    public int Money
    {
        get
        {
            return money;
        }
    }

    public int Fandom
    {
        get
        {
            return fandom;
        }
    }

    public int DevelopmentSkill
    {
        get
        {
            return developmentSkill;
        }
    }

    public int TechnicalDebt
    {
        get
        {
            return technicalDebt;
        }
    }

    private void Awake()
    {
        // Prevent duplicate ResourceManagers.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Keep player resources when changing scenes.
        DontDestroyOnLoad(gameObject);
    }

    // ==============================
    // Resource Update Methods
    // ==============================

    public void ChangeMoney(int amount)
    {
        money += amount;

        // Money should never go below zero.
        money = Mathf.Max(0, money);

        Debug.Log(
            "Money changed by "
            + amount
            + ". Current Money: "
            + money);
    }

    public void ChangeFandom(int amount)
    {
        fandom += amount;

        // Fandom cannot be negative.
        fandom = Mathf.Max(0, fandom);

        Debug.Log(
            "Fandom changed by "
            + amount
            + ". Current Fandom: "
            + fandom);
    }

    public void ChangeDevelopmentSkill(int amount)
    {
        developmentSkill += amount;

        // Development Skill stays between 0 and 100.
        developmentSkill =
            Mathf.Clamp(
                developmentSkill,
                0,
                100);

        Debug.Log(
            "Development Skill changed by "
            + amount
            + ". Current Development Skill: "
            + developmentSkill);
    }

    public void ChangeTechnicalDebt(int amount)
    {
        technicalDebt += amount;

        // Technical Debt stays between 0 and 100.
        technicalDebt =
            Mathf.Clamp(
                technicalDebt,
                0,
                100);

        Debug.Log(
            "Technical Debt changed by "
            + amount
            + ". Current Technical Debt: "
            + technicalDebt);
    }

    // ==============================
    // Testing / Reset
    // ==============================

    [ContextMenu("Reset Resources")]
    public void ResetResources()
    {
        money = 250;
        fandom = 0;
        developmentSkill = 10;
        technicalDebt = 0;

        Debug.Log("Player resources reset.");
    }

    [ContextMenu("Print Resources")]
    public void PrintResources()
    {
        Debug.Log(
            "Money: " + money
            + "\nFandom: " + fandom
            + "\nDevelopment Skill: " + developmentSkill
            + "\nTechnical Debt: " + technicalDebt);
    }
}