using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(StateMachine stateMachine, EnemyBasic enemyBasic) : base(stateMachine, enemyBasic)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        if (enemyBasic.disBetweenEnemyAndPlayer <= enemyBasic.MaxAttackDistance) stateMachine.ChangeState(enemyBasic.enemyCombatState);
    }
    public override void UpdatePhysic()
    {
        base.UpdatePhysic();

        enemyBasic.move.MoveHandle(Mathf.Sign(enemyBasic.AttackDir.x));
    }

    public override void Exit()
    {
        enemyBasic.move.StopMoveCoroutine();
    }
}