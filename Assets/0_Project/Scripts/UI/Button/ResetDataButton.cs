using UnityEngine;

public class ResetDataButton : ButtonBase
{
    public override void Clicked()
    {
        base.Clicked();
        SingletonManager.Instance.dataManager.ResetData();
    }
}
