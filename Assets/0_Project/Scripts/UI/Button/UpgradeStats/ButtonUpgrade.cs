using UnityEngine;

public class ButtonUpgrade : ButtonBase
{
    public override void Clicked()
    {
        base.Clicked();
        SingletonManager.Instance.gameManager.ChangeUpgradeScene();
    }
}
