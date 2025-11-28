using System.Collections;
using UnityEngine;

public class AgileEnemyAttackState : AgileEnemyState
{
    public AgileEnemyAttackState(AgileEnemy agileEnemy, StateMachine stateMachine) : base(agileEnemy, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        agileEnemy.attack.HandleAttack(agileEnemy.AttackDir);

        if (agileEnemy.attack.currentAttackData == null) return;
        agileEnemy.attack.currentAttackData.OnAttackEnd += OnAttackEnd;
    }


    public override void Exit()
    {
        agileEnemy.attack.StopAttack();

        if (agileEnemy.attack.currentAttackData == null) return;
        agileEnemy.attack.currentAttackData.OnAttackEnd -= OnAttackEnd;
    }

    private void OnAttackEnd()
    {
        agileEnemy.SendDamage();

        stateMachine.ChangeState(agileEnemy.agileEnemyCombatState);
    }
}