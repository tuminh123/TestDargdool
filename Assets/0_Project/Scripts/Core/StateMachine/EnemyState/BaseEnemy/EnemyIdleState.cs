using System.Collections;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(StateMachine stateMachine, EnemyBasic enemyBasic) : base(stateMachine, enemyBasic)
    {
    }
    public override void UpdatePhysic()
    {
        base.UpdatePhysic();
        enemyBasic.idle.IdelHandle();
    }
}