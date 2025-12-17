using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class PlayerRegenerationButton : ButtonBase
{
    public override void Clicked()
    {
        base.Clicked();

        AdsManager.Instance.RewardAdsHandle().Forget();
        if (AdsManager.Instance.IsReward)
        {
            GameEventBus.RaisePlayerRegeneration();
        }
        
    }
}