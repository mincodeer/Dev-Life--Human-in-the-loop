using System;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    [Header("Starting Money")]
    [SerializeField] private int startingMoney = 10000;

    public int CurrentMoney { get; private set; }

    public event Action<int> MoneyChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        CurrentMoney = startingMoney;
    }

    private void Start()
    {
        NotifyMoneyChanged();
    }

    public void AddMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning(
                "AddMoney requires a positive amount."
            );

            return;
        }

        CurrentMoney += amount;
        NotifyMoneyChanged();
    }

    public void SpendMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning(
                "SpendMoney requires a positive amount."
            );

            return;
        }

        // The balance is allowed to become negative.
        CurrentMoney -= amount;
        NotifyMoneyChanged();
    }

    public void SetMoney(int amount)
    {
        CurrentMoney = amount;
        NotifyMoneyChanged();
    }

    public bool CanAfford(int amount)
    {
        return amount >= 0 && CurrentMoney >= amount;
    }

    private void NotifyMoneyChanged()
    {
        MoneyChanged?.Invoke(CurrentMoney);
    }

#if UNITY_EDITOR

    [ContextMenu("TEST - Add $1,000")]
    private void TestAddMoney()
    {
        AddMoney(1000);
    }

    [ContextMenu("TEST - Spend $12,000")]
    private void TestSpendMoney()
    {
        SpendMoney(12000);
    }

#endif
}