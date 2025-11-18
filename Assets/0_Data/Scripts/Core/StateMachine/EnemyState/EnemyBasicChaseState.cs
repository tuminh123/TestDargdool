using UnityEngine;

public class EnemyBasicChaseState : EnemyBasicState
{
    public EnemyBasicChaseState(StateMachine stateMachine, EnemyAI enemyAI, GameObject player, float attackDistance, Balance bodyBalance) : base(stateMachine, enemyAI, player, attackDistance, bodyBalance)
    {
    }
    public override void Update()
    {
        base.Update();
        
        if(attackDir.x > 0) 
            enemyAI.move.MoveRight();
        else if(attackDir.x < 0)
            enemyAI.move.MoveLeft();

        if (distanceToPlayer <= attackDistance)
        {
            stateMachine.ChangeState(enemyAI.enemyAttackState);
        }
    }

  
}