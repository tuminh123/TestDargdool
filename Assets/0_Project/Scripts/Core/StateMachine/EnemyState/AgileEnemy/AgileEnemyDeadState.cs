using System.Collections;
using UnityEngine;

public class AgileEnemyDeadState : AgileEnemyState
{
    private float deadTime;
    private float deadDuration;
    public AgileEnemyDeadState(AgileEnemy agileEnemy, StateMachine stateMachine, float deadDuration) : base(agileEnemy, stateMachine)
    {
        this.deadDuration = deadDuration;
    }
    public override void Enter()
    {
        base.Enter();

        agileEnemy.EnemyDieHandle();

        deadTime = deadDuration;
    }
    public override void Update()
    {
        base.Update();
        deadTime -= Time.deltaTime;
        if (deadTime < 0)
        {
            agileEnemy.OnDead();
        }
    }
}