using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePartEnemyCombatState : DamagePartEnemyState
{
    private float attackTime;
    private float attackDuration;

    public DamagePartEnemyCombatState(DamagePartEnemy partEnemy, StateMachine stateMachine,float attackDuration) : base(partEnemy, stateMachine)
    {
        this.attackDuration = attackDuration;
    }

    public override void Enter()
    {
        base.Enter();
        attackTime = attackDuration;
    }

    public override void Update()
    {
        base.Update();

        attackTime -= Time.deltaTime;

        if (partEnemy.disBetweenEnemyAndPlayer > partEnemy.MaxAttackDistance)
        {
            stateMachine.ChangeState(partEnemy.damagePartEnemyChaseState);
            return;
        }

        if (attackTime <= 0f)
        {
            stateMachine.ChangeState(partEnemy.damagePartEnemyAttackState);
            return;
        }

        partEnemy.idle.IdleHandle(partEnemy.ragdollController.actionBase);

            
    }
}