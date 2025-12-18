using UnityEngine;

public class DamagePartEnemyShootState : DamagePartEnemyState
{
    private float time;
    private float durationCombat;

    public DamagePartEnemyShootState(DamagePartEnemy partEnemy, StateMachine stateMachine,float durationCombat) : base(partEnemy, stateMachine)
    {
        this.durationCombat = durationCombat;
    }
    public override void Enter()
    {
        base.Enter();

        partEnemy.BeginShoot();

        time = durationCombat;
    }
    public override void Update()
    {
        base.Update();
        time -= Time.deltaTime;
        if (time < 0)
        {
            stateMachine.ChangeState(partEnemy.damagePartEnemyCombatState);
        }
    }
    public override void Exit()
    {
        base.Exit();

        partEnemy.StopShoot();

    }
}
