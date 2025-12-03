using System.Collections;
using UnityEngine;

public class AgileEnemyCombatState : AgileEnemyState
{
    private float attackTime;
    private float attackDuration;
    public AgileEnemyCombatState(AgileEnemy agileEnemy, StateMachine stateMachine,float attackDuration) : base(agileEnemy, stateMachine)
    {
        this.attackDuration = attackDuration;
    }
    public override void Enter()
    {
        base.Enter();
        agileEnemy.idle.IdelHandle();
        attackTime = attackDuration;
        //Debug.Log(attackTime);
    }

    public override void Update()
    {
        base.Update();

        attackTime -= Time.deltaTime;
        

        if (attackTime < 0f)
        {
            if (Random.value < 0.5f)
                stateMachine.ChangeState(agileEnemy.agileEnemyAttackState);
            else
                stateMachine.ChangeState(agileEnemy.agileEnemyJumpState);

        }
        else
        {
            agileEnemy.idle.IdelHandle();
        }

        if (agileEnemy.disBetweenEnemyAndPlayer > agileEnemy.MaxAttackDistance) 
            stateMachine.ChangeState(agileEnemy.agileEnemyChaseState);

    }
}