using System.Collections;
using UnityEngine;

public class AgileEnemyIdleState : AgileEnemyState
{
    private float time;
    public AgileEnemyIdleState(AgileEnemy agileEnemy, StateMachine stateMachine) : base(agileEnemy, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        time = 2f;
        agileEnemy.idle.IdelHandle();
    }
    public override void Update()
    {
        base.Update();
        time -= Time.deltaTime;
        if(time <= 0f)
        {
            stateMachine.ChangeState(agileEnemy.agileEnemyChaseState);
        }
    }
}