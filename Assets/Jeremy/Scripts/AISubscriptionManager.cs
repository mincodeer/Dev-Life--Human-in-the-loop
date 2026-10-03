using UnityEngine;

public class AISubscriptionManager : MonoBehaviour
{
    public int subscriptionCost = 100;
    public int creditPackCost = 50;
    public int creditsPerPack = 10;
    public int creditsPerUse = 1;

    public GameTimeManager gameTime;

    private int nextSubscriptionDay = 7;

    private void Start()
    {
        if (gameTime != null)
        {
            gameTime.OnDayPassed += CheckSubscription;
        }
    }

    private void OnDestroy()
    {
        if (gameTime != null)
        {
            gameTime.OnDayPassed -= CheckSubscription;
        }
    }

    public bool UseAI()
    {
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
        if (ResourceManager.Instance.Money < creditPackCost)
        {
            Debug.Log("Not enough money to buy AI credits.");
            return;
        }

        ResourceManager.Instance.ChangeMoney(-creditPackCost);
        ResourceManager.Instance.ChangeAICredits(creditsPerPack);
    }

    public void PaySubscription()
    {
        if (ResourceManager.Instance.Money < subscriptionCost)
        {
            Debug.Log("Not enough money to pay AI subscription.");
            return;
        }

        ResourceManager.Instance.ChangeMoney(-subscriptionCost);

        Debug.Log("AI subscription paid.");
    }

    private void CheckSubscription()
    {
        if (gameTime.currentDay >= nextSubscriptionDay)
        {
            PaySubscription();

            nextSubscriptionDay += 7;
        }
    }
}