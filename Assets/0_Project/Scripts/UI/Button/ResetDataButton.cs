using UnityEngine;

public class ResetDataButton : ButtonBase
{
    public override void Clicked()
    {
        SingletonManager.Instance.dataManager.ResetData();
    }
}
