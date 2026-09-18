using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomUpgradeManager : MonoBehaviour
{
    public static RoomUpgradeManager Instance
    {
        get;
        private set;
    }

    private bool pcUpgradePurchased;
    private bool lightUpgradePurchased;
    private bool cleanUpPurchased;
    private bool windowUpgradePurchased;
    private bool decorUpgradePurchased;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Minjae's Scene")
        {
            UpdateRoom();
        }
    }

    public void PurchasePCUpgrade()
    {
        pcUpgradePurchased = true;
    }

    public void PurchaseLightUpgrade()
    {
        lightUpgradePurchased = true;
    }

    public void PurchaseCleanUp()
    {
        cleanUpPurchased = true;
    }

    public void PurchaseWindowUpgrade()
    {
        windowUpgradePurchased = true;
    }

    public void PurchaseDecorUpgrade()
    {
        decorUpgradePurchased = true;
    }

    public void UpdateRoom()
    {
        SetUpgradeObject(
            "PCUpgradePlaceholder",
            pcUpgradePurchased);

        SetUpgradeObject(
            "LightUpgradePlaceholder",
            lightUpgradePurchased);

        SetUpgradeObject(
            "CleanUpPlaceholder",
            cleanUpPurchased);

        SetUpgradeObject(
            "WindowUpgradePlaceholder",
            windowUpgradePurchased);

        SetUpgradeObject(
            "DecorUpgradePlaceholder",
            decorUpgradePurchased);
    }

    private void SetUpgradeObject(string objectName, bool purchased)
    {
        GameObject upgradeObject = FindSceneObject(objectName);

        if (upgradeObject != null)
        {
            upgradeObject.SetActive(purchased);

            Debug.Log(
                objectName
                + " found. Purchased: "
                + purchased);
        }
        else
        {
            Debug.LogWarning(
                objectName
                + " was not found in the room.");
        }
    }

    private GameObject FindSceneObject(string objectName)
    {
        GameObject[] rootObjects =
            SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject rootObject in rootObjects)
        {
            Transform[] children =
                rootObject.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in children)
            {
                if (child.name == objectName)
                {
                    return child.gameObject;
                }
            }
        }

        return null;
    }

    public bool IsPCUpgradePurchased()
    {
        return pcUpgradePurchased;
    }

    public bool IsLightUpgradePurchased()
    {
        return lightUpgradePurchased;
    }

    public bool IsCleanUpPurchased()
    {
        return cleanUpPurchased;
    }

    public bool IsWindowUpgradePurchased()
    {
        return windowUpgradePurchased;
    }

    public bool IsDecorUpgradePurchased()
    {
        return decorUpgradePurchased;
    }
}