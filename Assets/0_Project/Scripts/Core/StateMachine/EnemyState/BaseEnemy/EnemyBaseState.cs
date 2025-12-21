using System;
using UnityEngine;

public class EnemyBaseState : IState
{
    protected EnemyBasic enemyBasic;
    protected StateMachine stateMachine;
    protected bool isGround;
    public EnemyBaseState(StateMachine stateMachine,EnemyBasic enemyBasic)
    {
        this.stateMachine = stateMachine;
        this.enemyBasic = enemyBasic;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }

    public virtual void Update()
    {
        isGround = enemyBasic.groundDetect.IsGround();
        if (enemyBasic.characterCtrl == null || enemyBasic.characterCtrl.healthBase.IsDead)
        {
            stateMachine.ChangeState(enemyBasic.enemyIdleState);
            return;
        }

        if (enemyBasic.healthBase.IsDead)
        {
            stateMachine.ChangeState(enemyBasic.enemyDieState);
        }
        else if(enemyBasic.IsStunned)
        {
            stateMachine.ChangeState(enemyBasic.enemyStunState);
        }
    }

    public virtual void UpdatePhysic() 
    {
    }
}