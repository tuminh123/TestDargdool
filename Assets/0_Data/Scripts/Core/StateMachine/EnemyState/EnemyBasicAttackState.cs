using UnityEngine;

public class EnemyBasicAttackState : EnemyBasicState
{
    public EnemyBasicAttackState(StateMachine stateMachine, EnemyAI enemyAI, GameObject player, float attackDistance, Balance bodyBalance) : base(stateMachine, enemyAI, player, attackDistance, bodyBalance)
    {
    }
    public override void Update()
    {
        base.Update();
        Debug.Log("Attack");
        enemyAI.move.StopMove();
        if (distanceToPlayer > attackDistance)
        {
            stateMachine.ChangeState(enemyAI.enemyChaseState);
        }
    }

   
}