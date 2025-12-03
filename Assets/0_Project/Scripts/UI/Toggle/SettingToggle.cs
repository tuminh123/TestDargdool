using UnityEngine;

public class SettingToggle : ToggleBase
{
    protected override void OnToggleChanged(bool isOn)
    {
        SingletonManager.Instance.uiManager.SettingMenuPanel.SetActive(isOn);
    }
}
