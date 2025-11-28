using UnityEngine;

public class DamagePartEnemyDeadState : DamagePartEnemyState
{
    private float deadTime;
    private float deadDuration;
    public DamagePartEnemyDeadState(DamagePartEnemy partEnemy, StateMachine stateMachine, float deadDuration) : base(partEnemy, stateMachine)
    {
        this.deadDuration = deadDuration;
    }
    public override void Enter()
    {
        base.Enter();

        partEnemy.SetTriggerBalance(false);
        partEnemy.SetKnockBackBalance();

        deadTime = deadDuration;
    }
    public override void Update()
    {
        base.Update();
        deadTime -= Time.deltaTime;
        if (deadTime < 0)
        {
            partEnemy.OnDead();
        }
    }
}
