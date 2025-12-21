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
        partEnemy.idle.IdelHandle();
        attackTime = attackDuration;
    }

    public override void Update()
    {
        base.Update();

        attackTime -= Time.deltaTime;


        if (partEnemy.disBetweenEnemyAndPlayer > partEnemy.MaxAttackDistance * 3)
        {
            stateMachine.ChangeState(partEnemy.damagePartEnemyJumpState);
            return;
        }

        if (partEnemy.disBetweenEnemyAndPlayer > partEnemy.MaxAttackDistance)
        {
            stateMachine.ChangeState(partEnemy.damagePartEnemyChaseState);
            return;
        }

        if (attackTime <= 0f)
        {
            IState[] states = new IState[]
            {
            partEnemy.damagePartEnemyAttackState,
            partEnemy.damagePartEnemyDefenseState,
            partEnemy.damagePartEnemyShootState,
            };

            int rand = Random.Range(0, states.Length);
            stateMachine.ChangeState(states[rand]);
            return;
        }

        partEnemy.idle.IdelHandle();
    }
}