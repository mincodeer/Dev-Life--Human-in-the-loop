using UnityEngine;

public class MarketTrendNavigation : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject dashboardPanel;
    [SerializeField] private GameObject marketTrendsPanel;

    private void Start()
    {
        ShowDashboard();
    }

    public void ShowMarketTrends()
    {
        dashboardPanel.SetActive(false);
        marketTrendsPanel.SetActive(true);
    }

    public void ShowDashboard()
    {
        marketTrendsPanel.SetActive(false);
        dashboardPanel.SetActive(true);
    }
}