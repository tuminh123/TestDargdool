using System.Collections;
using UnityEngine;

public class GameStateParent : IGameState
{
    protected GameManager gameManager;
    public GameStateParent(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }   
    public virtual void EnterState()
    {
        
    }

    public virtual void ExitState()
    {
        
    }

    public virtual void UpdateState()
    {
        
    }
}