using UnityEngine;

public class MainMenuButton : ButtonBase
{
    public override void Clicked()
    {
        base.Clicked();
        SingletonManager.Instance.gameManager.ChangeMenuScene();
    }
}
