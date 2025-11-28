using System.Collections;
using UnityEngine;

public class DamagePartEnemyAttackState : DamagePartEnemyState
{
    public DamagePartEnemyAttackState(DamagePartEnemy partEnemy, StateMachine stateMachine) : base(partEnemy, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

       partEnemy.attack.HandleAttack(partEnemy.AttackDir);

        if (partEnemy.attack.currentAttackData == null) return;
       partEnemy.attack.currentAttackData.OnAttackEnd += OnAttackEnd;
    }


    public override void Exit()
    {
       partEnemy.attack.StopAttack();

        if (partEnemy.attack.currentAttackData == null) return;
       partEnemy.attack.currentAttackData.OnAttackEnd -= OnAttackEnd;
    }

    private void OnAttackEnd()
    {
       partEnemy.SendDamage();

        stateMachine.ChangeState(partEnemy.damagePartEnemyCombatState);
    }
}