using System.Collections;
using UnityEngine;

public class DamagePartEnemyIdleState : DamagePartEnemyState
{
    private float time;
    public DamagePartEnemyIdleState(DamagePartEnemy partEnemy, StateMachine stateMachine) : base(partEnemy, stateMachine)
    {
    }
    override public void Enter()
    {
        base.Enter();
        time = 2f;
        partEnemy.idle.IdelHandle();
    }
    public override void Update()
    {
        base.Update();
        time -= Time.deltaTime;
        if (time <= 0f)
        {
            stateMachine.ChangeState(partEnemy.damagePartEnemyChaseState);
        }
    }
}