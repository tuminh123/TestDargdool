using System.Collections;
using UnityEngine;


public class AgileEnemyChaseState : AgileEnemyState
{
    public AgileEnemyChaseState(AgileEnemy agileEnemy, StateMachine stateMachine) : base(agileEnemy, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
      
    }
    public override void Update()
    {
        base.Update();
        if (agileEnemy.disBetweenEnemyAndPlayer <= agileEnemy.MaxAttackDistance) 
            stateMachine.ChangeState(agileEnemy.agileEnemyCombatState);
    }
    public override void UpdatePhysic()
    {
        base.UpdatePhysic();

        agileEnemy.move.MoveHandle(Mathf.Sign(agileEnemy.AttackDir.x));
    }

    public override void Exit()
    {
        agileEnemy.move.StopMoveCoroutine();
    }
}