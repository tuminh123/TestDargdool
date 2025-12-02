using UnityEngine;

public class QuitButton : ButtonBase
{
    public override void Clicked()
    {
        SingletonManager.Instance.gameManager.SetState(GameState.QUITGAME);
    }
}
