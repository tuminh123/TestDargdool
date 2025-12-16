using UnityEngine;

public class RestartGameButton : ButtonBase
{
    public override void Clicked()
    {
        base.Clicked();
        GameEventBus.RaiseGameRestart();
    }
}
