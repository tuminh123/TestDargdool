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
        enemyBasic.attackContext.EnableAttack();
        enemyBasic.attackContext.EnableAttackPhysics(enemyBasic.AttackDir);
        enemyBasic.attack.HandleAttack(enemyBasic.AttackDir).Forget();

        enemyBasic.SendDamageBase();

        if (enemyBasic.attack.currentAttackData == null) return;
    
        enemyBasic.attack.currentAttackData.OnAttacking += OnAttacking;
        enemyBasic.attack.currentAttackData.OnEndAttack += OnAttackEnd;
    }

    
    public override void Exit()
    {
        base.Exit();
        enemyBasic.attackContext.DisableAttack();
        enemyBasic.attackContext.ResetPhysics();

        if (enemyBasic.attack.currentAttackData == null) return;
        enemyBasic.attack.currentAttackData.OnAttacking -= OnAttacking;
        enemyBasic.attack.currentAttackData.OnEndAttack -= OnAttackEnd;
    }

    private void OnAttackEnd()
    {
        enemyBasic.attack.StopAttack();
        stateMachine.ChangeState(enemyBasic.enemyCombatState);
    }

    private void OnAttacking()
    {
        enemyBasic.SendDamage();

        
    }

}