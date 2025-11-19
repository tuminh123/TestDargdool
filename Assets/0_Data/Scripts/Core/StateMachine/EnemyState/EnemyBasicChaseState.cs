using UnityEngine;

public class EnemyBasicChaseState : EnemyBasicState
{
    public EnemyBasicChaseState(StateMachine stateMachine, EnemyAI enemyAI, GameObject player, float attackDistance, Balance bodyBalance) : base(stateMachine, enemyAI, player, attackDistance, bodyBalance)
    {
    }
    public override void Update()
    {
        base.Update();
        
        enemyAI.move.MoveHandle(attackDir.x);
        
        if (distanceToPlayer <= attackDistance)
        {
            stateMachine.ChangeState(enemyAI.enemyAttackState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemyAI.move.StopMoveCoroutine();
        enemyAI.idle.IdelHandle();
    }
}