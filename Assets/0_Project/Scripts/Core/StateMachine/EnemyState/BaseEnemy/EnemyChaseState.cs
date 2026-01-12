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
        enemyBasic?.ragdollController?.actionBase.SetPostAction(new ActionPostNormal());

        if (enemyBasic.disBetweenEnemyAndPlayer <= enemyBasic.MaxAttackDistance) stateMachine.ChangeState(enemyBasic.enemyCombatState);
    }
    public override void UpdatePhysic()
    {
        base.UpdatePhysic();

        enemyBasic.move.MoveHandle(Mathf.Sign(enemyBasic.AttackDir.x),enemyBasic.ragdollController.actionBase);
    }

    public override void Exit()
    {
        enemyBasic.move.StopMoveCoroutine();
    }
}