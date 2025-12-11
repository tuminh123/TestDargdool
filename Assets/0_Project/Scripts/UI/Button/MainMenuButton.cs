using UnityEngine;

public class MainMenuButton : ButtonBase
{
    public override void Clicked()
    {
        SingletonManager.Instance.gameManager.ChangeMenuScene();
    }
}
