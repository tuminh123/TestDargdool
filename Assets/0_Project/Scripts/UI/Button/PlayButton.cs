using UnityEngine;
using Zenject;

public class PlayButton : ButtonBase
{
    public override void Clicked()
    {
        //GameEventBus.RaiseGameStateChanged(GameState.PLAY);
        SingletonManager.Instance.gameManager.SetState(GameState.PLAY);
    }
}
