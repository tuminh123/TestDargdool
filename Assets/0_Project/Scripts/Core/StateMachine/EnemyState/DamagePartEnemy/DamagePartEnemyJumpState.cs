using System.Collections;
using UnityEngine;
public class DamagePartEnemyJumpState : DamagePartEnemyState
{
    public DamagePartEnemyJumpState(DamagePartEnemy partEnemy, StateMachine stateMachine) : base(partEnemy, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        float x = partEnemy.AttackDir.x;
        //partEnemy.jump.JumpHandle(x);
    }
    public override void Update()
    {
        base.Update();

        if (isGround)
        {
            stateMachine.ChangeState(partEnemy.damagePartEnemyCombatState);
        }
    }
}