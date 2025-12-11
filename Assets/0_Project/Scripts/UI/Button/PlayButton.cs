using UnityEngine;
using Zenject;

public class PlayButton : ButtonBase
{
    public override void Clicked()
    {
        SingletonManager.Instance.gameManager.ChangePlayScene();
    }
}
