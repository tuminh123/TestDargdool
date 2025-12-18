using UnityEngine;

public class QuitButton : ButtonBase
{
    public override void Clicked()
    {
        SingletonManager.Instance.dataManager.DataSave();
        Application.Quit();
    }
}
