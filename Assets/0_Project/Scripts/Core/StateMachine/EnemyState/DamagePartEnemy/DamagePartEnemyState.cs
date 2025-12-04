using System.Collections;
using UnityEngine;

public class DamagePartEnemyState : IState
{
    protected DamagePartEnemy partEnemy;
    protected StateMachine stateMachine;

    public DamagePartEnemyState(DamagePartEnemy partEnemy, StateMachine stateMachine)
    {
        this.partEnemy = partEnemy;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }

    public virtual void Update()
    {
        GameState currentGameState = SingletonManager.Instance.gameManager.CurrentState;
        if (currentGameState == GameState.LOSE)
        {
            stateMachine.ChangeState(partEnemy.damagePartEnemyIdleState);
            return;
        }

        if (partEnemy.healthBase.IsDead)
        {
            stateMachine.ChangeState(partEnemy.damagePartEnemyDeadState);
        }
        else if (partEnemy.IsStunned)
        {
            stateMachine.ChangeState(partEnemy.damagePartEnemyStund);
        }
    }

    public virtual void UpdatePhysic()
    {
        GameState currentGameState = SingletonManager.Instance.gameManager.CurrentState;
        if (currentGameState == GameState.LOSE) return;
    }
}