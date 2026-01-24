using System.Collections;
using UnityEngine;

public class DamagePartEnemyAttackState : DamagePartEnemyState
{
    private IPostAction postAction;
    private VfxBase vfx = null;
    public DamagePartEnemyAttackState(DamagePartEnemy partEnemy, StateMachine stateMachine) : base(partEnemy, stateMachine)
    {
        postAction = new SmoothPostAction(partEnemy?.ragdollController?.ActionsDataSO, partEnemy?.ragdollController?.Balances, partEnemy?.attack?.ConfigSO);
    }

    public override void Enter()
    {
        base.Enter();

        string name = partEnemy.AttackDir.x > 0 ? StringConst.RIGHT_PUNCH : StringConst.LEFT_PUNCH;
        partEnemy.attackContext.EnableAttack();
        partEnemy?.ragdollController?.postContext.SetPostAction(postAction);

        GameObject hand = partEnemy.AttackDir.x > 0 ? partEnemy.RightHand : partEnemy.LeftHand;
        partEnemy.EffectSpawns(hand, out vfx);

        partEnemy?.attack?.attackContext?.ExecuteAttack(partEnemy.AttackDir, partEnemy.ragdollController.actionBase, name, partEnemy.gameObject);
        partEnemy.SendDamageBase();

        partEnemy.attack.currentAttack.OnAttackEnd += EndAttack;
    }


    public override void Exit()
    {
        base.Exit();
        partEnemy?.attack?.attackContext?.CancelAttack();
        if (partEnemy.healthBase.IsDead || partEnemy.IsStunned)
        {
            partEnemy.attackContext.DisableAttack();
        }

        partEnemy.attack.currentAttack.OnAttackEnd -= EndAttack;
    }

    private void EndAttack()
    {
      
        if (vfx != null)
        {
            partEnemy.EffectDeSpawns(vfx);
        }
        partEnemy.attackContext.DisableAttack();
        stateMachine.ChangeState(partEnemy.damagePartEnemyCombatState);
    }
}