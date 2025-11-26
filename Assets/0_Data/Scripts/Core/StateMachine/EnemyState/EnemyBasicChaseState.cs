using UnityEngine;

public class EnemyBasicChaseState : EnemyBasicState
{
    public EnemyBasicChaseState(StateMachine stateMachine, EnemyAI enemyAI, Transform body)
           : base(stateMachine, enemyAI, body) { }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        if (distance <= 7) stateMachine.ChangeState(enemyAI.enemyIdleState);
    }
    public override void UpdatePhysic()
    {
        base.UpdatePhysic();

        enemyAI.move.MoveHandle(Mathf.Sign(dir.x));
    }

    public override void Exit()
    {
        enemyAI.move.StopMoveCoroutine();
    }
}