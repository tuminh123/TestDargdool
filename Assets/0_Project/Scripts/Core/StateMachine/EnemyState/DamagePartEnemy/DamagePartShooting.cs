using UnityEngine;

public class DamagePartShooting : DamagePartEnemyState
{
    public DamagePartShooting(DamagePartEnemy partEnemy, StateMachine stateMachine) : base(partEnemy, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        partEnemy.ShootHandle();

        partEnemy.EndShooting += PartEnemy_EndShooting;
    }

  
    public override void Exit()
    {
        base.Exit();

        partEnemy.CancelShoot();

        partEnemy.EndShooting += PartEnemy_EndShooting;
    }

    private void PartEnemy_EndShooting()
    {
        stateMachine.ChangeState(partEnemy.damagePartEnemyCombatState);
    }

}
