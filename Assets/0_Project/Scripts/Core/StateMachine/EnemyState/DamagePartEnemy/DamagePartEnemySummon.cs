using UnityEngine;

public class DamagePartEnemySummon : DamagePartEnemyState
{

    public DamagePartEnemySummon(DamagePartEnemy partEnemy, StateMachine stateMachine) : base(partEnemy, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        partEnemy.SummonEnemy();
        partEnemy.EndSummon += PartEnemy_EndSummon;

    }

    public override void Update()
    {
        base.Update();
        partEnemy.CancelSummon();
        partEnemy.EndSummon -= PartEnemy_EndSummon;

    }
    private void PartEnemy_EndSummon()
    {
        stateMachine.ChangeState(partEnemy.damagePartEnemyCombatState);
    }

}
