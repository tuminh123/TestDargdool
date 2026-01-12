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
       /* enemyBasic.attackContext.EnableAttack();
        enemyBasic.attackContext.EnableAttackPhysics(enemyBasic.AttackDir);

        string name = enemyBasic.AttackDir.x < 0 ? "LeftPunch" : "RightPunch";
        enemyBasic.attack.HandleAttack(enemyBasic.AttackDir, enemyBasic.ragdollController.actionBase, name).Forget();

        enemyBasic.SendDamageBase();*/

        enemyBasic?.attack?.EnterAttack(enemyBasic, "LeftPunch", "RightPunch");

        if (enemyBasic.attack.currentAttackData == null) return;

       /* enemyBasic.attack.currentAttackData.EnableEffect(true);
        enemyBasic.attack.currentAttackData.OnAttacking += OnAttacking;*/
        enemyBasic.attack.currentAttackData.OnEndAttack += OnAttackEnd;
    }

    
    public override void Exit()
    {
        base.Exit();

        enemyBasic?.attack?.ExitAttack(enemyBasic);

        /*  enemyBasic.attackContext.DisableAttack();
          enemyBasic.attackContext.ResetPhysics();*/

        if (enemyBasic.attack.currentAttackData == null) return;

        /*enemyBasic.attack.currentAttackData.EnableEffect(false);
        enemyBasic.attack.currentAttackData.OnAttacking -= OnAttacking;*/
        enemyBasic.attack.currentAttackData.OnEndAttack -= OnAttackEnd;
    }

    private void OnAttackEnd()
    {
        enemyBasic.attack.CancelAttack();
        stateMachine.ChangeState(enemyBasic.enemyCombatState);
    }

   /* private void OnAttacking()
    {
        enemyBasic.SendDamage();
        
    }
*/
}