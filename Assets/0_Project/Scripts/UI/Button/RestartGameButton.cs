using Cysharp.Threading.Tasks;
using UnityEngine;

public class RestartGameButton : ButtonBase
{
    public override void Clicked()
    {
        base.Clicked();
        if (!AdsManager.Instance.IsFirstCheck)
        {
            AdsManager.Instance.InterAdsHandle().Forget();
        }
        GameEventBus.RaiseGameRestart();
    }
}
