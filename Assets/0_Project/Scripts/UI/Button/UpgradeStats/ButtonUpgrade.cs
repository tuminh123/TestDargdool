using UnityEngine;

public class ButtonUpgrade : ButtonBase
{
    public override void Clicked()
    {
        SingletonManager.Instance.gameManager.ChangeUpgradeScene();
    }
}
