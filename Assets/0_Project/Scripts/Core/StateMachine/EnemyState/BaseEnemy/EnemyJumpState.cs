using System.Collections;
using UnityEngine;

public class EnemyJumpState : EnemyBaseState
{
    public EnemyJumpState(StateMachine stateMachine, EnemyBasic enemyBasic) : base(stateMachine, enemyBasic)
    {
    }
    public override void Enter()
    {
        base.Enter();
        float x = enemyBasic.AttackDir.x;
        //enemyBasic.jump.JumpHandle(x);
    }
    public override void Update()
    {
        base.Update();

        if (isGround)
        {
            stateMachine.ChangeState(enemyBasic.enemyCombatState);
        }
    }
}