using UnityEngine;

public class DamagePartEnemyTeleportState : DamagePartEnemyState
{
    public DamagePartEnemyTeleportState(DamagePartEnemy partEnemy, StateMachine stateMachine) : base(partEnemy, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        partEnemy.Teleport();

        partEnemy.EndTeleport += PartEnemy_EndTeleport;
    }

   

    public override void Exit()
    {
        base.Exit();
        partEnemy.CancelTeleport();
        partEnemy.EndTeleport -= PartEnemy_EndTeleport;
    }

    private void PartEnemy_EndTeleport()
    {
        stateMachine.ChangeState(partEnemy.damagePartEnemyCombatState);
    }

}
