using System.Collections;
using UnityEngine;

public class GamePlayState : GameStateParent
{
    public GamePlayState(GameManager gameManager) : base(gameManager)
    {
    }
    public override void EnterState()
    {
        base.EnterState();
        SingletonManager.Instance.gameManager.ChangePlayScene();
    }
}