using UnityEngine;

public class AgileEnemyShootState : AgileEnemyState
{
    private float time;
    private float durationCombat;
    public AgileEnemyShootState(AgileEnemy agileEnemy, StateMachine stateMachine,float durationCombat) : base(agileEnemy, stateMachine)
    {
        this.durationCombat = durationCombat;
    }
    public override void Enter()
    {
        base.Enter();

        agileEnemy.BeginShoot();

        time = durationCombat;
    }
    public override void Update()
    {
        base.Update();
        time -= Time.deltaTime;
        if(time < 0)
        {
            stateMachine.ChangeState(agileEnemy.agileEnemyCombatState);
        }
    }
    public override void Exit()
    {
        base.Exit();

        agileEnemy.StopShoot();

    }


}
