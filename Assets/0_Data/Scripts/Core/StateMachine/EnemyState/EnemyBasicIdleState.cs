using UnityEngine;

public class EnemyBasicIdleState : EnemyBasicState
{
    private float time;
    public EnemyBasicIdleState(StateMachine stateMachine, EnemyAI enemyAI, Transform body) : base(stateMachine, enemyAI, body)
    {
    }

    public override void Enter()
    {
        base.Enter();
        time = 1.5f;
        enemyAI.idle.IdelHandle();
    }

    public override void Update()
    {
        base.Update();

        time -= Time.fixedDeltaTime;
        //enemyAI.idle.IdelHandle();

        if (enemyAI.playerDetect.IsPlayer == true && time < 0)
        {
            stateMachine.ChangeState(enemyAI.enemyAttackState);

        }
        if (enemyAI.playerDetect.IsPlayer == false)
        {
            stateMachine.ChangeState(enemyAI.enemyChaseState);
        }
    }
}