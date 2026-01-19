using System.Threading;
using UnityEngine;

public class EnemyBaseWeaponAttack : EnemyBaseState
{
    private CancellationTokenSource atc;
    private CancellationTokenSource ltc;
    private IRagdollAttackSystem attackSystem;
    private IPostAction postAction;
    public EnemyBaseWeaponAttack(StateMachine stateMachine, EnemyBasic enemyBasic) : base(stateMachine, enemyBasic)
    {
        InitAttackSystems();
    }

    public override void Enter()
    {
        base.Enter();

        if (!IsValidWeapon())
        {
            stateMachine.ChangeState(enemyBasic.enemyIdleState);
            return;
        }

        CheckConditionsAttack();

        enemyBasic.weaponEquip.CurrentWeapon.EnableAttack();
        enemyBasic.weaponEquip.CurrentWeapon.EnableEffect(true);

        // Action
        StartAttackAsync();

        enemyBasic.SendWeaponDamageBase();

        SubscribeEvents();
    }


    public override void Exit()
    {

        base.Exit();

        /*postAction = null;
        attackSystem = null;*/

        UnsubscribeEvents();

    }
    #region FUNCTION
    // Check Conditions
    private bool IsValidWeapon()
    {
        return enemyBasic.weaponEquip != null &&
               enemyBasic.weaponEquip.HasWeapon &&
               enemyBasic.weaponEquip.CurrentWeapon != null &&
               enemyBasic.attack != null;
    }

    #region Attack system
    // Object Attack Initialization

    private void InitAttackSystems()
    {
        attackSystem ??= new AttackWeaponPhysicSystem(
            enemyBasic.attack.PhysicsProfile,
            null
        );

        postAction ??= new PhysicPostAction(
            enemyBasic.ragdollController.ActionsDataSO,
            enemyBasic.ragdollController.Balances,
            Vector2.zero,
            enemyBasic.attack.PhysicsProfile
        );
    }

    //Set Conditions Attack
    private void CheckConditionsAttack()
    {
        AttackWeaponPhysicSystem physicSystem = attackSystem as AttackWeaponPhysicSystem;
        PhysicPostAction physicPost = postAction as PhysicPostAction;
        physicSystem?.SetWeapon(enemyBasic?.weaponEquip?.CurrentWeapon);
        physicPost.SetDir(enemyBasic.AttackDir);
    }

    // Execute Attack
    private void StartAttackAsync()
    {
        string name = enemyBasic.AttackDir.x > 0
            ? StringConst.WEAPON_RIGHT_PHYSIC
            : StringConst.WEAPON_LEFT_PHYSIC;

        atc = new CancellationTokenSource();
        ltc = CancellationTokenSource.CreateLinkedTokenSource(atc.Token, enemyBasic.destroyCancellationToken);
        var token = ltc.Token;


        UniTaskSafe.Forget(
            tc => attackSystem.ExecuteAttack(
                tc,
                enemyBasic.AttackDir,
                postAction,
                name),
            token,
            "MainWeaponAttackState"
        );
        enemyBasic.weaponEquip.SetFlipWeaponByAttackDir(enemyBasic.AttackDir);
    }

    public void CancelAttack()
    {
        if (ltc != null)
        {
            if (!ltc.IsCancellationRequested) ltc.Cancel();

            ltc.Dispose();
            ltc = null;
        }

        if (atc != null)
        {
            if (!atc.IsCancellationRequested) atc.Cancel();

            atc.Dispose();
            atc = null;
        }
    }
    #endregion

    #region Event
    private void SubscribeEvents()
    {
        attackSystem.OnAttackEnd += EndAttack;
        enemyBasic.weaponEquip.OnDrop += WeaponEquip_OnDrop;

    }

    private void UnsubscribeEvents()
    {
        attackSystem.OnAttackEnd -= EndAttack;
        enemyBasic.weaponEquip.OnDrop -= WeaponEquip_OnDrop;

    }

    private void WeaponEquip_OnDrop()
    {
        stateMachine.ChangeState(enemyBasic.enemyIdleState);
    }
    private void EndAttack()
    {
        CancelAttack();

        if (enemyBasic.weaponEquip.CurrentWeapon != null)
        {
            enemyBasic.weaponEquip.CurrentWeapon.DisableAttack();
            enemyBasic.weaponEquip.CurrentWeapon.EnableEffect(false);
        }

        stateMachine.ChangeState(enemyBasic.enemyIdleState);
    }

    #endregion

    #endregion
}
