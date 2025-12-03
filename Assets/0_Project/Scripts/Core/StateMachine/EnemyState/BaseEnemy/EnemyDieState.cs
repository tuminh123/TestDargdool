using System.Collections;
using UnityEngine;

public class EnemyDieState : EnemyBaseState
{
    private float deadTime;
    private float deadDuration;
    public EnemyDieState(StateMachine stateMachine,EnemyBasic enemyBasic, float deadDuration) : base(stateMachine, enemyBasic)
    {
        this.deadDuration = deadDuration;
    }
    public override void Enter()
    {
        base.Enter();

        enemyBasic.SetTriggerBalance(false);
        enemyBasic.SetKnockBackBalance();
        deadTime = deadDuration;
    }
    public override void Update()
    {
        base.Update();
        deadTime -= Time.deltaTime;
        if(deadTime < 0)
        {
            enemyBasic.OnDead();
        }
    }
}