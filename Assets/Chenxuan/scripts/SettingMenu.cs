using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    public GameObject settingspanel;
    public void OpenSettings()
    { settingspanel.SetActive(true); }
    public void CloseSettings()
    { settingspanel.SetActive(false); }
}
