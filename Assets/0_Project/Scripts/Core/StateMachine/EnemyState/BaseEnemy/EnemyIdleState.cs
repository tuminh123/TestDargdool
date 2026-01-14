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

        //enemyBasic?.ragdollController?.actionBase.SetPostAction(new ActionPostNormal());
        enemyBasic.idle.IdleHandle(enemyBasic.ragdollController.actionBase);
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