using UnityEngine;

public class DamagePartEnemyAreaIceSkill : DamagePartEnemyState
{
    public DamagePartEnemyAreaIceSkill(DamagePartEnemy partEnemy, StateMachine stateMachine) : base(partEnemy, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        partEnemy.ActiveIceAreaSkill();

        partEnemy.EndIceSkill += PartEnemy_EndIceSkill;
    }

    public override void Exit()
    {
        base.Exit();
        partEnemy.CancelIceAreaSkill();

        partEnemy.EndIceSkill -= PartEnemy_EndIceSkill;
    }

    private void PartEnemy_EndIceSkill()
    {
        stateMachine.ChangeState(partEnemy.damagePartEnemyCombatState);
    }
}
