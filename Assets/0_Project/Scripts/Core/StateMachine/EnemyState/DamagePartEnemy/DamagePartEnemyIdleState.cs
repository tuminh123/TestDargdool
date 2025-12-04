using System.Collections;
using UnityEngine;

public class DamagePartEnemyIdleState : DamagePartEnemyState
{
    public DamagePartEnemyIdleState(DamagePartEnemy partEnemy, StateMachine stateMachine) : base(partEnemy, stateMachine)
    {
    }
    public override void UpdatePhysic()
    {
        base.UpdatePhysic();
        partEnemy.idle.IdelHandle();
    }
}