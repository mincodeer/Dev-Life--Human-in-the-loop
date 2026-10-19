
using System;
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

    // Notifies the UI whenever resources change.
    public Action OnResourcesChanged;

    [Header("Player Resources")]

    [SerializeField]
    private int money = 250;

    [SerializeField]
    private int fandom = 0;

    [SerializeField]
    private int aiCredits = 10;

    // Tracks AI Credit purchases for scaling prices.
    [SerializeField]
    private int aiCreditPurchaseCount = 0;

    [SerializeField]
    private bool aiSubscriptionActive = false;

    [SerializeField]
    private bool aiSubscriptionPaused = false;

    [SerializeField]
    private int nextAISubscriptionDay = 0;

    public bool AISubscriptionActive => aiSubscriptionActive;

    public bool AISubscriptionPaused => aiSubscriptionPaused;

    public int NextAISubscriptionDay => nextAISubscriptionDay;

    // Stores the visual and text data for the Fandom ending.
    public EndingData fandomEnding;

    [SerializeField]
    [Range(0, 100)]
    private int developmentSkill = 10;

    [SerializeField]
    [Range(0, 100)]
    private int technicalDebt = 0;

    // Stores the visual and text data for the Technical Debt ending.
    public EndingData technicalDebtEnding;

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

    public int AICredits
    {
        get
        {
            return aiCredits;
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

    // Returns the current AI Credit pack price.
    public int AICreditPackCost
    {
        get
        {
            if (aiCreditPurchaseCount == 0)
            {
                return 50;
            }
            else if (aiCreditPurchaseCount == 1)
            {
                return 100;
            }
            else if (aiCreditPurchaseCount == 2)
            {
                return 200;
            }
            else
            {
                return 300;
            }
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

        if (GetComponent<ProjectUnlockProgression>() == null)
        {
            gameObject.AddComponent<ProjectUnlockProgression>();
        }

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

        OnResourcesChanged?.Invoke();
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

        // Check if the player has reached maximum Fandom.
        if (fandom >= 100)
        {
            EndingController endingController =
                FindFirstObjectByType<EndingController>(
                    FindObjectsInactive.Include);

            if (endingController != null
                && fandomEnding != null)
            {
                endingController.ShowEnding(fandomEnding);
            }
        }

        OnResourcesChanged?.Invoke();
    }

    public void ChangeAICredits(int amount)
    {
        aiCredits += amount;

        // AI Credits cannot be negative.
        aiCredits = Mathf.Max(0, aiCredits);

        Debug.Log(
            "AI Credits changed by "
            + amount
            + ". Current AI Credits: "
            + aiCredits);

        OnResourcesChanged?.Invoke();
    }

    // Increases the AI Credit pack price after a purchase.
    public void IncreaseAICreditPurchaseCount()
    {
        aiCreditPurchaseCount++;

        Debug.Log(
            "AI Credit packs purchased: "
            + aiCreditPurchaseCount
            + ". Next pack price: $"
            + AICreditPackCost);

        OnResourcesChanged?.Invoke();
    }

    public void ActivateAISubscription(int nextPaymentDay)
    {
        aiSubscriptionActive = true;
        aiSubscriptionPaused = false;
        nextAISubscriptionDay = nextPaymentDay;

        OnResourcesChanged?.Invoke();
    }

    public void PauseAISubscription()
    {
        aiSubscriptionActive = false;
        aiSubscriptionPaused = true;

        OnResourcesChanged?.Invoke();
    }

    public void UpdateAISubscriptionDay(int nextPaymentDay)
    {
        nextAISubscriptionDay = nextPaymentDay;

        OnResourcesChanged?.Invoke();
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

        if (technicalDebt >= 100)
        {
            EndingController endingController =
                FindFirstObjectByType<EndingController>(
                    FindObjectsInactive.Include);

            if (endingController != null
                && technicalDebtEnding != null)
            {
                endingController.ShowEnding(technicalDebtEnding);
            }
        }
    }

    // ==============================
    // Testing / Reset
    // ==============================

    [ContextMenu("Reset Resources")]
    public void ResetResources()
    {
        money = 250;
        fandom = 0;
        aiCredits = 10;
        aiCreditPurchaseCount = 0;
        developmentSkill = 10;
        technicalDebt = 0;
        aiSubscriptionActive = false;
        aiSubscriptionPaused = false;
        nextAISubscriptionDay = 0;

        OnResourcesChanged?.Invoke();

        Debug.Log("Player resources reset.");
    }

    [ContextMenu("Print Resources")]
    public void PrintResources()
    {
        Debug.Log(
            "Money: " + money
            + "\nFandom: " + fandom
            + "\nAI Credits: " + aiCredits
            + "\nAI Credit Pack Price: $" + AICreditPackCost
            + "\nAI Credit Packs Purchased: " + aiCreditPurchaseCount
            + "\nDevelopment Skill: " + developmentSkill
            + "\nTechnical Debt: " + technicalDebt);
    }

    // Copies the current resources into the save snapshot.
    public void CaptureSaveData(StudioSaveData data)
    {
        if (data == null)
            return;

        data.money = money;
        data.fandom = fandom;
        data.developmentSkill = developmentSkill;
        data.technicalDebt = technicalDebt;

        data.aiCredits = aiCredits;
        data.aiCreditPurchaseCount = aiCreditPurchaseCount;

        data.aiSubscriptionActive = aiSubscriptionActive;
        data.aiSubscriptionPaused = aiSubscriptionPaused;
        data.nextAISubscriptionDay = nextAISubscriptionDay;
    }

    // Restores saved values directly.
    // Loading does not count as earning resources or buying credits.
    public void RestoreSaveData(StudioSaveData data)
    {
        if (data == null)
            return;

        money = Mathf.Max(0, data.money);
        fandom = Mathf.Max(0, data.fandom);

        developmentSkill = Mathf.Clamp(
            data.developmentSkill, 0, 100);

        technicalDebt = Mathf.Clamp(
            data.technicalDebt, 0, 100);

        aiCredits = Mathf.Max(0, data.aiCredits);

        aiCreditPurchaseCount = Mathf.Max(
            0, data.aiCreditPurchaseCount);

        aiSubscriptionActive = data.aiSubscriptionActive;
        aiSubscriptionPaused = data.aiSubscriptionPaused;

        nextAISubscriptionDay = Mathf.Max(
            0, data.nextAISubscriptionDay);
    }
}
