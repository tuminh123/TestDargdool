using UnityEngine;

public class RestartGameButton : ButtonBase
{
    public override void Clicked()
    {
        SingletonManager.Instance.gameManager.SetState(GameState.RESTART);
    }
}
