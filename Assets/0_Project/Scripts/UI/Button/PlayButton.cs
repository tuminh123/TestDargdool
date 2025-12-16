using UnityEngine;
using Zenject;

public class PlayButton : ButtonBase
{
    public override void Clicked()
    {
        base.Clicked();
        SingletonManager.Instance.gameManager.ChangePlayScene();
    }
}
