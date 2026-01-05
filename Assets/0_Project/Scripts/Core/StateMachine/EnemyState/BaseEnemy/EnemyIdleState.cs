using System.Collections;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private float time;
    public EnemyIdleState(StateMachine stateMachine, EnemyBasic enemyBasic) : base(stateMachine, enemyBasic)
    {
    }
    override public void Enter()
    {
        base.Enter();
        time = 2f;
        enemyBasic.idle.IdelHandle();
    }
    public override void Update()
    {
        base.Update();
        time -= Time.deltaTime;
        if(time <= 0f)
        {
            stateMachine.ChangeState(enemyBasic.enemyCombatState);
        }
    }
}