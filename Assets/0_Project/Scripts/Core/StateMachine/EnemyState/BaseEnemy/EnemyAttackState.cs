using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(StateMachine stateMachine, EnemyBasic enemyBasic) : base(stateMachine, enemyBasic)
    {
    }

    public override void Enter()
    {
        base.Enter();

       /* UniTaskSafe.Forget
        (
        );*/
        enemyBasic.attack.HandleAttack(enemyBasic.AttackDir);

        if (enemyBasic.attack.currentAttackData == null) return;
        enemyBasic.attack.currentAttackData.OnAttacking += OnAttackEnd;
    }

    
    public override void Exit()
    {
        enemyBasic.attack.StopAttack();

        if (enemyBasic.attack.currentAttackData == null) return;
        enemyBasic.attack.currentAttackData.OnAttacking -= OnAttackEnd;
    }

    private void OnAttackEnd()
    {
        enemyBasic.SendDamage();

        stateMachine.ChangeState(enemyBasic.enemyCombatState);
    }

}