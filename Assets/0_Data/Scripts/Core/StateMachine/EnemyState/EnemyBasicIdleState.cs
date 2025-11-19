using UnityEngine;

public class EnemyBasicIdleState : EnemyBasicState
{
    public EnemyBasicIdleState(StateMachine stateMachine, EnemyAI enemyAI, Transform body) : base(stateMachine, enemyAI, body)
    {
    }

    public override void Enter()
    {
        base.Enter();
       
    }

    public override void Update()
    {
        base.Update();
        enemyAI.idle.IdelHandle();
        if (enemyAI.playerDetect.IsPlayer == true)
        {
            stateMachine.ChangeState(enemyAI.enemyAttackState);

        }
        if (enemyAI.playerDetect.IsPlayer == false)
        {
            stateMachine.ChangeState(enemyAI.enemyChaseState);
        }
    }
}