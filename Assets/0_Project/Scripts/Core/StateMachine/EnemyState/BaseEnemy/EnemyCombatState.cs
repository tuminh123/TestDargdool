using UnityEngine;

public class EnemyCombatState : EnemyBaseState
{
    private float attackTime;
    private float attackDuration;
    public EnemyCombatState(StateMachine stateMachine,EnemyBasic enemyBasic,float attackDuraction) : base(stateMachine, enemyBasic)
    {
        this.attackDuration = attackDuraction;
    }

    public override void Enter()
    {
        base.Enter();
        enemyBasic.idle.IdelHandle();
        attackTime = attackDuration;
        //Debug.Log(attackTime);
    }

    public override void Update()
    {
        base.Update();

        attackTime -= Time.deltaTime;

        if (enemyBasic.disBetweenEnemyAndPlayer > enemyBasic.MaxAttackDistance * 3)
        {
            stateMachine.ChangeState(enemyBasic.enemyJumpState);
            return;
        }

        if (enemyBasic.disBetweenEnemyAndPlayer > enemyBasic.MaxAttackDistance)
        {
            stateMachine.ChangeState(enemyBasic.enemyChaseState);
            return;
        }

        if (attackTime <= 0f)
        {
            stateMachine.ChangeState(enemyBasic.enemyAttackState);
            return;
        }

        enemyBasic.idle.IdelHandle();
    }
}