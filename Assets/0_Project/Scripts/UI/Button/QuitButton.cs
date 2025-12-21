using UnityEngine;

public class QuitButton : ButtonBase
{
    public override void Clicked()
    {
        DataManager.Instance.DataSave();
        Application.Quit();
    }
}
