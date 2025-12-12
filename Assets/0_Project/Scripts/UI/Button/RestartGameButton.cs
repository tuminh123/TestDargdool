using UnityEngine;

public class RestartGameButton : ButtonBase
{
    public override void Clicked()
    {
        GameEventBus.RaiseGameRestart();
    }
}
