using System.Collections;
using UnityEngine;

public class AgileEnemyState : IState
{
    protected AgileEnemy agileEnemy;
    protected StateMachine stateMachine;
    protected bool isGround;

    public AgileEnemyState(AgileEnemy agileEnemy, StateMachine stateMachine)
    {
        this.agileEnemy = agileEnemy;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }

    public virtual void Update()
    {

        isGround = agileEnemy.groundDetect.IsGround();


        if (agileEnemy.healthBase.IsDead)
        {
            stateMachine.ChangeState(agileEnemy.agileEnemyDeadState);
        }
        else if (agileEnemy.IsStunned)
        {
            stateMachine.ChangeState(agileEnemy.agileEnemyStunnedState);
        }
    }



    public virtual void UpdatePhysic()
    {
    }
}