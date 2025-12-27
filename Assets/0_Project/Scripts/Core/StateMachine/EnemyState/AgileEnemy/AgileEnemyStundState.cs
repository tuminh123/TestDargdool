using UnityEngine;

public class AgileEnemyStunnedState : AgileEnemyState
{
    private float stunTime;
    private float stunDuration;
    public AgileEnemyStunnedState(AgileEnemy agileEnemy, StateMachine stateMachine,float stunDuration) : base(agileEnemy, stateMachine)
    {
        this.stunDuration = stunDuration;
    }
    public override void Enter()
    {
        base.Enter();
        stunTime = stunDuration;
        agileEnemy.SetKnockBackBalance();
        agileEnemy.DisableBalance(); 
    }
    public override void Update()
    {
        base.Update();
        stunTime -= Time.deltaTime;

        if (stunTime <= 0)
        {
            stateMachine.ChangeState(agileEnemy.agileEnemyCombatState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        agileEnemy.SetIsStunned(false);
        agileEnemy.EnableBalance();
    }
}