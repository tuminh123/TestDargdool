using System.Collections;
using UnityEngine;

public class ResumeButton : ButtonBase
{
    public override void Clicked()
    {
        SingletonManager.Instance.gameManager.SetState(GameState.PLAY);
    }
}