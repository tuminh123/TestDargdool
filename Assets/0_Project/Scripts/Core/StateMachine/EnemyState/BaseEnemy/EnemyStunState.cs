using System.Collections;
using UnityEngine;

public class EnemyStunState : EnemyBaseState
{
    private float stunTime;
    private float stunDuration;
    public EnemyStunState(StateMachine stateMachine,EnemyBasic enemyBasic, float stunDuration) : base(stateMachine,enemyBasic)
    {
        this.stunDuration = stunDuration;
    }

    public override void Enter()
    {
        base.Enter();
        stunTime = stunDuration;

        Vector2 dir = Random.value > 0.5f ? Vector2.right : Vector2.left;
        enemyBasic?.weaponEquip?.DropWeapon(dir);

        enemyBasic?.ragdollController?.DisableRagdoll();
        enemyBasic?.ragdollController?.KnockBackCharacter(enemyBasic.GetKnockDir());

    }
    public override void Update()
    {
        base.Update();
        stunTime -= Time.deltaTime;

        if (stunTime <= 0)
        {
            stateMachine.ChangeState(enemyBasic.enemyCombatState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        enemyBasic.SetIsStunned(false);
        enemyBasic?.ragdollController?.EnableRagdoll();
    }


}