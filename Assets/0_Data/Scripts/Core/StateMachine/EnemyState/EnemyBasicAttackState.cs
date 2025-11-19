using UnityEngine;

public class EnemyBasicAttackState : EnemyBasicState
{
    public EnemyBasicAttackState(StateMachine stateMachine, EnemyAI enemyAI, GameObject player, float attackDistance, Balance bodyBalance) : base(stateMachine, enemyAI, player, attackDistance, bodyBalance)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        enemyAI.attack.HandleAttack(attackDir);
        
        enemyAI.attack.currentAttackData.OnAttackEnd += () =>
        {
            stateMachine.ChangeState(enemyAI.enemyIdleState);
        };
    }

    public override void Update()
    {
        base.Update();
        if (distanceToPlayer > attackDistance)
        {
            stateMachine.ChangeState(enemyAI.enemyChaseState);
        }
    }

    public override void Exit()
    {
        base.Exit();      
        enemyAI.attack.StopAttack();
        
    }

   
}