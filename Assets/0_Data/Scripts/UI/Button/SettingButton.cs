using UnityEngine;
using UnityEngine.UI;

public class SettingToggle : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel;
    private Toggle settingsToggle;
    private void Awake()
    {
        settingsToggle = GetComponent<Toggle>();
    }
    void Start()
    {
        settingPanel.SetActive(settingsToggle.isOn);
        SetGamePause(settingsToggle.isOn);

        settingsToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool isOn)
    {
        settingPanel.SetActive(isOn);
    }
    void SetGamePause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
