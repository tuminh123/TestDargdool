using UnityEngine;

public class EnemyBasicAttackState : EnemyBasicState
{
    public EnemyBasicAttackState(StateMachine stateMachine, EnemyAI enemyAI, Transform body) : base(stateMachine, enemyAI, body)
    {
    }

    public override void Enter()
    {
        base.Enter();

        

        enemyAI.attack.HandleAttack(attackDir);

       /* enemyAI.attack.currentAttackData.OnAttackEnd += OnAttackEnd;*/
    }

    public override void Exit()
    {
        base.Exit();      
        enemyAI.attack.StopAttack();

       /* enemyAI.attack.currentAttackData.OnAttackEnd -= OnAttackEnd;*/
    }

    private void OnAttackEnd()
    {
        stateMachine.ChangeState(enemyAI.enemyChaseState);
    }
}