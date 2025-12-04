using System.Collections;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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

        enemyBasic.EnemyDieHandle();

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