using UnityEngine;

public class EnemyBasicIdleState : EnemyBasicState
{
    public EnemyBasicIdleState(StateMachine stateMachine, EnemyAI enemyAI, GameObject player, float attackDistance, Balance bodyBalance) : base(stateMachine, enemyAI, player, attackDistance, bodyBalance)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemyAI.idle.IdelHandle();
    }

    public override void Update()
    {
        base.Update();
        if (distanceToPlayer <= attackDistance)
        {
            stateMachine.ChangeState(enemyAI.enemyAttackState);
            
        }
        else /*if (distanceToPlayer <= attackDistance)*/
        {
            stateMachine.ChangeState(enemyAI.enemyChaseState);
        }
    }
}