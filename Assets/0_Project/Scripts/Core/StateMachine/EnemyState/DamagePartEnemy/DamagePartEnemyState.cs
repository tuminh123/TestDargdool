using System.Collections;
using UnityEngine;

public class DamagePartEnemyState : IState
{
    protected DamagePartEnemy partEnemy;
    protected StateMachine stateMachine;
    protected bool isGround;

    public DamagePartEnemyState(DamagePartEnemy partEnemy, StateMachine stateMachine)
    {
        this.partEnemy = partEnemy;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }

    public virtual void Update()
    {

        if (partEnemy.groundDetect != null)
        {
            isGround = partEnemy.groundDetect.IsGround();
        }
        else
        {
            return;
        }
        if (partEnemy.characterCtrl == null || partEnemy.characterCtrl.healthBase.IsDead)
        {
            stateMachine.ChangeState(partEnemy.damagePartEnemyIdleState);
            return;
        }

        if (partEnemy.healthBase.IsDead)
        {
            stateMachine.ChangeState(partEnemy.damagePartEnemyDeadState);
            return;
        }

        if (partEnemy.IsStunned)
        {
            stateMachine.ChangeState(partEnemy.damagePartEnemyStund);
            return;
        }
        if (partEnemy.healthBase.CurrentHealth <= 998 && partEnemy.CanSummon())
        {
            stateMachine.ChangeState(partEnemy.damagePartEnemySummon);
            return;
        }
        if (partEnemy.CanTeleport)
        {
            stateMachine.ChangeState(partEnemy.teleportState);
            return;
        }
        if (partEnemy.CanShoot)
        {
            stateMachine.ChangeState(partEnemy.partShooting);
            return;
        }
    }

    public virtual void UpdatePhysic()
    {
    }
}