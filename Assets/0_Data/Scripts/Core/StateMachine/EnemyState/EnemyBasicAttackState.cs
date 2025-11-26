using UnityEngine;

public class EnemyBasicAttackState : EnemyBasicState
{
    public EnemyBasicAttackState(StateMachine stateMachine, EnemyAI enemyAI, Transform body)
        : base(stateMachine, enemyAI, body) { }

    public override void Enter()
    {
        base.Enter();

        enemyAI.attack.HandleAttack(dir);

        if (enemyAI.attack.currentAttackData == null) return;
        enemyAI.attack.currentAttackData.OnAttackEnd += OnAttackEnd;
    }

    
    public override void Exit()
    {
        enemyAI.attack.StopAttack();

        if (enemyAI.attack.currentAttackData == null) return;
        enemyAI.attack.currentAttackData.OnAttackEnd -= OnAttackEnd;
    }

    private void OnAttackEnd()
    {
        enemyAI.SendDamage();

        stateMachine.ChangeState(enemyAI.enemyIdleState);
    }

}