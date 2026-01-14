using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(StateMachine stateMachine, EnemyBasic enemyBasic) : base(stateMachine, enemyBasic)
    {
    }

    public override void Enter()
    {
        base.Enter();
        string name = enemyBasic.AttackDir.x > 0 ? StringConst.RIGHT_PUNCH : StringConst.LEFT_PUNCH;

        enemyBasic.attackContext.EnableAttack();

        enemyBasic?.attack?.ExecuteAttack(enemyBasic.AttackDir,enemyBasic.ragdollController.actionBase,name,enemyBasic.gameObject);
        enemyBasic.SendDamageBase();

        enemyBasic.attack.currentAttack.OnAttackEnd += EndAttack;
    }

    
    public override void Exit()
    {
        base.Exit();

        enemyBasic.attackContext.DisableAttack();

        enemyBasic?.attack?.CancelAttack();
        enemyBasic.attack.currentAttack.OnAttackEnd -= EndAttack;
    }

    private void EndAttack()
    {

        stateMachine.ChangeState(enemyBasic.enemyCombatState);
    }

}