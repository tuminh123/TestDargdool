using UnityEngine;
using UnityEngine.UI;
using Yade.Editor;

public class SettingButton : ButtonBase
{

    public override void Clicked()
    {
        SingletonManager.Instance.uiManager.SettingPanel.SetActive(true);
        SingletonManager.Instance.gameManager.SetState(GameState.PAUSE);
    }

    /*private void OnToggleChanged(bool isOn)
    {
        SingletonManager.Instance.uiManager.SettingPanel.SetActive(isOn);
        if (isOn)
        {
           
        }
        else
        {
            SingletonManager.Instance.gameManager.SetState(GameState.PLAY);
        }
    }*/
}
