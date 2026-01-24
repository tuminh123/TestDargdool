using Unity.VisualScripting;
using UnityEngine;

public class DamagePartEnemyStundState : DamagePartEnemyState
{
    private float stunTime;
    private float stunDuration;

    public DamagePartEnemyStundState(DamagePartEnemy partEnemy, StateMachine stateMachine,float stunDuration) : base(partEnemy, stateMachine)
    {
        this.stunDuration = stunDuration;
    }

    public override void Enter()
    {
        base.Enter();
        stunTime = stunDuration;

        Vector2 dir = Random.value > 0.5f ? Vector2.right : Vector2.left;
        partEnemy?.weaponEquip?.DropWeapon(dir);

        partEnemy?.ragdollController?.DisableRagdoll();
        partEnemy?.ragdollController?.KnockBackCharacter(partEnemy.GetKnockDir());

    }
    public override void Update()
    {
        base.Update();
        stunTime -= Time.deltaTime;

        if (stunTime <= 0)
        {
            stateMachine.ChangeState(partEnemy.damagePartEnemyCombatState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        partEnemy.SetIsStunned(false);
        partEnemy?.ragdollController?.EnableRagdoll();
    }
}