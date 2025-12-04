using System.Collections;
using UnityEngine;

public class AgileEnemyIdleState : AgileEnemyState
{
    public AgileEnemyIdleState(AgileEnemy agileEnemy, StateMachine stateMachine) : base(agileEnemy, stateMachine)
    {
    }

    public override void UpdatePhysic()
    {
        base.UpdatePhysic();
        agileEnemy.idle.IdelHandle();
    }
}