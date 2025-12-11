using System.Collections;
using UnityEngine;

public class MainMenuState : GameStateParent
{
    public MainMenuState(GameManager gameManager) : base(gameManager)
    {
    }
    public override void EnterState()
    {
        base.EnterState();
        SingletonManager.Instance.gameManager.ChangeMenuScene();
    }
}