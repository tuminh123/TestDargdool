using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private string name;
    public EnemyAttackState(StateMachine stateMachine, EnemyBasic enemyBasic) : base(stateMachine, enemyBasic)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemyBasic.attackContext.EnableAttack();
        

        string name = enemyBasic.AttackDir.x > 0 ? "PhysicRightPunch" : "PhysicLeftPunch";
        this.name = name;

        // Init
        enemyBasic?.ragdollController?.actionBase?.DisableBalance(name);
        enemyBasic.InitAttack(name);

        // Action
        enemyBasic?.attack?.ExecuteAttack(enemyBasic.AttackDir);
        enemyBasic.SendDamageBase();

        enemyBasic.attack.attackSystem.OnAttackEnd += EndAttack;
    }

    
    public override void Exit()
    {
        base.Exit();
        enemyBasic.attackContext.DisableAttack();
        enemyBasic?.ragdollController?.actionBase?.EnableBalance(name);
        enemyBasic.attack.attackSystem.OnAttackEnd -= EndAttack;
    }

    private void EndAttack()
    {

        stateMachine.ChangeState(enemyBasic.enemyCombatState);
    }

}