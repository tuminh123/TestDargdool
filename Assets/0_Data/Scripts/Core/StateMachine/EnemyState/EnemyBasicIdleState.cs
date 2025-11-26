using UnityEngine;

public class EnemyBasicIdleState : EnemyBasicState
{
    private float attackTime = 0f;
    public EnemyBasicIdleState(StateMachine stateMachine, EnemyAI enemyAI, Transform body) : base(stateMachine, enemyAI, body)
    {
    }

    public override void Enter()
    {
        base.Enter();

        attackTime = 3f;

        enemyAI.idle.IdelHandle();
    }

    public override void Update()
    {
        base.Update();

        attackTime -= Time.deltaTime;

        if(attackTime < 0f)
        {
            stateMachine.ChangeState(enemyAI.enemyAttackState);
        }
        else
        {
            enemyAI.idle.IdelHandle();
        }

        if (distance > 7) stateMachine.ChangeState(enemyAI.enemyChaseState);

    }
}