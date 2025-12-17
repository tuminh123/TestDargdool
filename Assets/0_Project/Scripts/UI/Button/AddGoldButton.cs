using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class AddGoldButton : ButtonBase
{
    public override void Clicked()
    {
        base.Clicked();
        AddGoldHandle();
    }

    private void AddGoldHandle()
    {
        AdsManager.Instance.RewardAdsHandle().Forget();
        SingletonManager.Instance.goldManager.AddGold(100);
    }
}
