using UnityEngine;

public class RoomUpgradeManager : MonoBehaviour
{
    public static RoomUpgradeManager Instance { get; private set; }


    [Header("PC Upgrade")]
    [SerializeField] private GameObject oldComputer;
    [SerializeField] private GameObject newComputer;


    [Header("Light Upgrade")]
    [SerializeField] private GameObject oldRoom;
    [SerializeField] private GameObject newRoom;


    [Header("Clean Up")]
    [SerializeField] private GameObject oldTable;
    [SerializeField] private GameObject newTable;

    [SerializeField] private GameObject oldBed;
    [SerializeField] private GameObject newBed;

    [SerializeField] private GameObject rubbish;


    [Header("Window Upgrade")]
    [SerializeField] private GameObject oldWindow;
    [SerializeField] private GameObject newWindow;


    [Header("Decor Upgrade")]
    [SerializeField] private GameObject oldDecoration;
    [SerializeField] private GameObject newDecoration;


    private const string PC_KEY = "Upgrade_PC";
    private const string LIGHT_KEY = "Upgrade_Light";
    private const string CLEAN_KEY = "Upgrade_Clean";
    private const string WINDOW_KEY = "Upgrade_Window";
    private const string DECOR_KEY = "Upgrade_Decor";


    private void Awake()
{
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
        return;
    }

    Instance = this;

    // TEST MODE:
    // Every Play Mode starts with no upgrades purchased.
    ResetAllRoomUpgrades();

    Debug.Log("RoomUpgradeManager ready.");
}


    // =========================================================
    // PURCHASE METHODS
    // StoreUIController calls THESE names
    // =========================================================

    public void PurchasePCUpgrade()
    {
        if (IsPCUpgradePurchased())
            return;

        PlayerPrefs.SetInt(PC_KEY, 1);
        PlayerPrefs.Save();

        RefreshRoomVisuals();

        Debug.Log("PC Upgrade purchased.");
    }


    public void PurchaseLightUpgrade()
    {
        if (IsLightUpgradePurchased())
            return;

        PlayerPrefs.SetInt(LIGHT_KEY, 1);
        PlayerPrefs.Save();

        RefreshRoomVisuals();

        Debug.Log("Light Upgrade purchased.");
    }


    public void PurchaseCleanUp()
    {
        if (IsCleanUpPurchased())
            return;

        PlayerPrefs.SetInt(CLEAN_KEY, 1);
        PlayerPrefs.Save();

        RefreshRoomVisuals();

        Debug.Log("Clean Up purchased.");
    }


    public void PurchaseWindowUpgrade()
    {
        if (IsWindowUpgradePurchased())
            return;

        PlayerPrefs.SetInt(WINDOW_KEY, 1);
        PlayerPrefs.Save();

        RefreshRoomVisuals();

        Debug.Log("Window Upgrade purchased.");
    }


    public void PurchaseDecorUpgrade()
    {
        if (IsDecorUpgradePurchased())
            return;

        PlayerPrefs.SetInt(DECOR_KEY, 1);
        PlayerPrefs.Save();

        RefreshRoomVisuals();

        Debug.Log("Decor Upgrade purchased.");
    }


    // =========================================================
    // PURCHASE CHECK METHODS
    // StoreUIController calls THESE names too
    // =========================================================

    public bool IsPCUpgradePurchased()
    {
        return PlayerPrefs.GetInt(PC_KEY, 0) == 1;
    }


    public bool IsLightUpgradePurchased()
    {
        return PlayerPrefs.GetInt(LIGHT_KEY, 0) == 1;
    }


    public bool IsCleanUpPurchased()
    {
        return PlayerPrefs.GetInt(CLEAN_KEY, 0) == 1;
    }


    public bool IsWindowUpgradePurchased()
    {
        return PlayerPrefs.GetInt(WINDOW_KEY, 0) == 1;
    }


    public bool IsDecorUpgradePurchased()
    {
        return PlayerPrefs.GetInt(DECOR_KEY, 0) == 1;
    }


    // =========================================================
    // APPLY VISUALS
    // =========================================================

    public void RefreshRoomVisuals()
    {
        bool pcPurchased =
            IsPCUpgradePurchased();

        bool lightPurchased =
            IsLightUpgradePurchased();

        bool cleanPurchased =
            IsCleanUpPurchased();

        bool windowPurchased =
            IsWindowUpgradePurchased();

        bool decorPurchased =
            IsDecorUpgradePurchased();


        // PC
        SetActive(oldComputer, !pcPurchased);
        SetActive(newComputer, pcPurchased);


        // ROOM / LIGHT
        SetActive(oldRoom, !lightPurchased);
        SetActive(newRoom, lightPurchased);


        // CLEAN UP
        SetActive(oldTable, !cleanPurchased);
        SetActive(newTable, cleanPurchased);

        SetActive(oldBed, !cleanPurchased);
        SetActive(newBed, cleanPurchased);

        SetActive(rubbish, !cleanPurchased);


        // WINDOW
        SetActive(oldWindow, !windowPurchased);
        SetActive(newWindow, windowPurchased);


        // DECOR
        SetActive(oldDecoration, !decorPurchased);
        SetActive(newDecoration, decorPurchased);
    }


    // =========================================================
    // HELPER
    // =========================================================

    private void SetActive(
        GameObject target,
        bool state)
    {
        if (target != null)
        {
            target.SetActive(state);
        }
    }


    // =========================================================
    // TEST RESET
    // =========================================================

    [ContextMenu("Reset All Room Upgrades")]
    public void ResetAllRoomUpgrades()
    {
        PlayerPrefs.DeleteKey(PC_KEY);
        PlayerPrefs.DeleteKey(LIGHT_KEY);
        PlayerPrefs.DeleteKey(CLEAN_KEY);
        PlayerPrefs.DeleteKey(WINDOW_KEY);
        PlayerPrefs.DeleteKey(DECOR_KEY);

        PlayerPrefs.Save();

        RefreshRoomVisuals();

        Debug.Log("All room upgrades reset.");
    }


    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}