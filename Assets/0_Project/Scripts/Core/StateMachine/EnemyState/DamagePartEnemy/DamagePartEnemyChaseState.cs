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

        partEnemy.FlipSystem(partEnemy.AttackDir.x, partEnemy.Head);
        if (partEnemy.disBetweenEnemyAndPlayer <= partEnemy.MaxAttackDistance)
        {
            stateMachine.ChangeState(partEnemy.damagePartEnemyCombatState);
            return;
        }
    }
    public override void UpdatePhysic()
    {
        base.UpdatePhysic();

        partEnemy.move.MoveHandle(Mathf.Sign(partEnemy.AttackDir.x), partEnemy.ragdollController.actionBase);
    }

    public override void Exit()
    {
        partEnemy.move.StopMoveCoroutine();
    }
}