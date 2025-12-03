using System.Collections;
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
        

        if (attackTime < 0f)
        {
            if (Random.value < 0.5f)
                stateMachine.ChangeState(partEnemy.damagePartEnemyAttackState);
            else
                stateMachine.ChangeState(partEnemy.damagePartEnemyDefenseState);

        }
        else
        {
            partEnemy.idle.IdelHandle();
        }

        if (partEnemy.disBetweenEnemyAndPlayer > partEnemy.MaxAttackDistance) 
            stateMachine.ChangeState(partEnemy.damagePartEnemyChaseState);

    }
}