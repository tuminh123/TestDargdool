using Cysharp.Threading.Tasks;
using UnityEngine;

public class ButtonUpgrade : ButtonBase
{
    public override void Clicked()
    {
        base.Clicked();
        /*if (!AdsManager.Instance.IsFirstCheck)
        {
            AdsManager.Instance.InterAdsHandle().Forget();
        }*/
        SingletonManager.Instance.gameManager.ChangeUpgradeScene();
    }
}
