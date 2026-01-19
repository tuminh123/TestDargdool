using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private IPostAction postAction;
    private VfxBase vfx = null;
    public EnemyAttackState(StateMachine stateMachine, EnemyBasic enemyBasic) : base(stateMachine, enemyBasic)
    {
        postAction = new SmoothPostAction(enemyBasic?.ragdollController?.ActionsDataSO, enemyBasic?.ragdollController?.Balances, enemyBasic?.attack?.ConfigSO);
    }

    public override void Enter()
    {
        base.Enter();

        string name = enemyBasic.AttackDir.x > 0 ? StringConst.RIGHT_PUNCH : StringConst.LEFT_PUNCH;
        enemyBasic.attackContext.EnableAttack();
        enemyBasic?.ragdollController?.postContext.SetPostAction(postAction);

        GameObject hand = enemyBasic.AttackDir.x > 0 ? enemyBasic.RightHand : enemyBasic.LeftHand;
        enemyBasic.EffectSpawns(hand, out vfx);

        enemyBasic?.attack?.attackContext?.ExecuteAttack(enemyBasic.AttackDir,enemyBasic.ragdollController.actionBase,name,enemyBasic.gameObject);
        enemyBasic.SendDamageBase();

        enemyBasic.attack.currentAttack.OnAttackEnd += EndAttack;
    }

    
    public override void Exit()
    {
        base.Exit();

        enemyBasic.attack.currentAttack.OnAttackEnd -= EndAttack;
    }

    private void EndAttack()
    {
        if (vfx != null)
        {
            enemyBasic.EffectDeSpawns(vfx);
        }
        enemyBasic.attackContext.DisableAttack();
        stateMachine.ChangeState(enemyBasic.enemyCombatState);
    }

}