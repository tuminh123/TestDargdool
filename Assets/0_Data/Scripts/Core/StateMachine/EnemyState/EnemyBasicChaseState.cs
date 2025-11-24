using UnityEngine;

public class EnemyBasicChaseState : EnemyBasicState
{
   
    public EnemyBasicChaseState(StateMachine stateMachine, EnemyAI enemyAI, Transform body) : base(stateMachine, enemyAI, body)
    {
    }

    public override void Enter()
    {
        base.Enter();
      
    }

    public override void Update()
    {
        base.Update();

        enemyAI.move.MoveHandle(attackDir.x);
        
        if (enemyAI.playerDetect.IsPlayer == true )
        {
            stateMachine.ChangeState(enemyAI.enemyIdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemyAI.move.StopMoveCoroutine();
    }
}