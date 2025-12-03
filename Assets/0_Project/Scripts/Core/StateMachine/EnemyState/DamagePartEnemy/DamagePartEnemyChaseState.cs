using System.Collections;
using UnityEngine;


public class DamagePartEnemyChaseState : DamagePartEnemyState
{
    public DamagePartEnemyChaseState(DamagePartEnemy partEnemy, StateMachine stateMachine) : base(partEnemy, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        if (partEnemy.disBetweenEnemyAndPlayer <= partEnemy.MaxAttackDistance) 
            stateMachine.ChangeState(partEnemy.damagePartEnemyCombatState);
    }
    public override void UpdatePhysic()
    {
        base.UpdatePhysic();

        partEnemy.move.MoveHandle(Mathf.Sign(partEnemy.AttackDir.x));
    }

    public override void Exit()
    {
        partEnemy.move.StopMoveCoroutine();
    }
}