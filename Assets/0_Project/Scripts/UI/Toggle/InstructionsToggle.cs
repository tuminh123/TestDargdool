using UnityEngine;

public class InstructionsToggle : ToggleBase
{
    protected override void Start()
    {
        base.Start();
        //SingletonManager.Instance.uiManager.GameInstructionsPanel.SetActive(toggle.isOn);
    }
    protected override void OnToggleChanged(bool isOn)
    {
        SingletonManager.Instance.uiManager.GameInstructionsPanel.SetActive(isOn);
    }
}
