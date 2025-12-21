using UnityEngine;

public class ResetDataButton : ButtonBase
{
    public override void Clicked()
    {
        base.Clicked();
        DataManager.Instance.ResetData();
    }
}
