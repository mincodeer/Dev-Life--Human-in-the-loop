
using UnityEngine;

public class AISubscriptionManager : MonoBehaviour
{
    public static AISubscriptionManager Instance { get; private set; }

    public int subscriptionCost = 100;
    public int subscriptionCredits = 25;
    public int subscriptionInterval = 7;

    public int creditsPerPack = 10;
    public int creditsPerUse = 1;

    public GameTimeManager gameTime;

    private GameTimeManager subscribedGameTime;

    public int CreditPackCost
    {
        get
        {
            if (ResourceManager.Instance == null)
            {
                return 50;
            }

            return ResourceManager.Instance.AICreditPackCost;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ConnectToGameTime();
    }

    private void Update()
    {
        if (subscribedGameTime == null)
        {
            ConnectToGameTime();
        }
    }

    private void ConnectToGameTime()
    {
        GameTimeManager currentGameTime = GameTimeManager.Instance;

        if (currentGameTime == null)
        {
            currentGameTime = FindFirstObjectByType<GameTimeManager>();
        }

        if (currentGameTime == null)
        {
            return;
        }

        if (subscribedGameTime == currentGameTime)
        {
            return;
        }

        if (subscribedGameTime != null)
        {
            subscribedGameTime.OnDayPassed -= CheckSubscription;
        }

        gameTime = currentGameTime;
        subscribedGameTime = currentGameTime;

        subscribedGameTime.OnDayPassed += CheckSubscription;

        CheckSubscription();
    }

    private void OnDestroy()
    {
        if (subscribedGameTime != null)
        {
            subscribedGameTime.OnDayPassed -= CheckSubscription;
        }

        if (Instance == this)
        {
            Instance = null;
        }
    }

    public bool UseAI()
    {
        if (ResourceManager.Instance == null)
        {
            return false;
        }

        if (ResourceManager.Instance.AICredits < creditsPerUse)
        {
            Debug.Log("Not enough AI credits.");
            return false;
        }

        ResourceManager.Instance.ChangeAICredits(-creditsPerUse);

        return true;
    }

    public void BuyCredits()
    {
        ResourceManager resources = ResourceManager.Instance;

        if (resources == null)
        {
            Debug.LogWarning("ResourceManager not found.");
            return;
        }

        int currentPrice = CreditPackCost;

        if (resources.Money < currentPrice)
        {
            Debug.Log("Not enough money to buy AI credits.");
            return;
        }

        resources.ChangeMoney(-currentPrice);
        resources.ChangeAICredits(creditsPerPack);
        resources.IncreaseAICreditPurchaseCount();

        Debug.Log("AI Credits purchased for $" + currentPrice);
        Debug.Log("Next pack costs $" + CreditPackCost);
    }

    public void Subscribe()
    {
        ResourceManager resources = ResourceManager.Instance;

        if (resources == null)
        {
            Debug.LogWarning("ResourceManager not found.");
            return;
        }

        if (resources.AISubscriptionActive)
        {
            Debug.Log("AI subscription is already active.");
            return;
        }

        ConnectToGameTime();

        if (gameTime == null)
        {
            Debug.LogWarning("GameTimeManager not found.");
            return;
        }

        if (resources.Money < subscriptionCost)
        {
            Debug.Log("Not enough money to subscribe.");
            return;
        }

        resources.ChangeMoney(-subscriptionCost);
        resources.ChangeAICredits(subscriptionCredits);

        int nextPaymentDay = gameTime.currentDay + subscriptionInterval;

        resources.ActivateAISubscription(nextPaymentDay);

        Debug.Log("AI subscription activated.");
        Debug.Log("Next payment on day " + nextPaymentDay);
    }

    public void PaySubscription()
    {
        ResourceManager resources = ResourceManager.Instance;

        if (resources == null || !resources.AISubscriptionActive)
        {
            return;
        }

        if (gameTime == null)
        {
            Debug.LogWarning("GameTimeManager not found.");
            return;
        }

        if (resources.Money < subscriptionCost)
        {
            resources.PauseAISubscription();

            Debug.Log("AI subscription paused. Not enough money.");
            return;
        }

        resources.ChangeMoney(-subscriptionCost);
        resources.ChangeAICredits(subscriptionCredits);

        int nextPaymentDay = gameTime.currentDay + subscriptionInterval;

        resources.UpdateAISubscriptionDay(nextPaymentDay);

        Debug.Log("AI subscription renewed.");
        Debug.Log("Next payment on day " + nextPaymentDay);
    }

    private void CheckSubscription()
    {
        ResourceManager resources = ResourceManager.Instance;

        if (resources == null || gameTime == null)
        {
            return;
        }

        if (!resources.AISubscriptionActive)
        {
            return;
        }

        if (gameTime.currentDay >= resources.NextAISubscriptionDay)
        {
            PaySubscription();
        }
    }
}
