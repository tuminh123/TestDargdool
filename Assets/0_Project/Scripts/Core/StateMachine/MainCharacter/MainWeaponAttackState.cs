using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

#region Test_1
/*
public class MainWeaponAttackState : MainCharacterState
{
    private CancellationTokenSource atc;
    private CancellationTokenSource ltc;
   
    
    IRagdollAttackSystem currentAttack;
    public MainWeaponAttackState(StateMachine stateMachine,CharacterCtrl characterCtrl) : base(stateMachine, characterCtrl)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (characterCtrl.weaponEquip == null || characterCtrl.attack == null) return;
        if (!characterCtrl.weaponEquip.HasWeapon)
        {
            stateMachine.ChangeState(characterCtrl.idelState);
            return;
        }

        string name = characterCtrl.AttackDir.x > 0 ? StringConst.WEAPON_RIGHT_PHYSIC : StringConst.WEAPON_LEFT_PHYSIC;
   
        if (characterCtrl.weaponEquip.CurrentWeapon == null) return;
        characterCtrl.weaponEquip.CurrentWeapon.EnableAttack();

        IPostAction postAction = new PhysicPostAction(characterCtrl?.ragdollController?.ActionsDataSO, characterCtrl?.ragdollController?.Balances, characterCtrl.AttackDir, characterCtrl?.attack?.PhysicsProfile);
        IRagdollAttackSystem ragdollAttack = new AttackWeaponPhysicSystem(characterCtrl?.attack?.PhysicsProfile, characterCtrl?.weaponEquip?.CurrentWeapon);//ragdollAttack == null

        currentAttack = ragdollAttack;

        atc = new CancellationTokenSource();
        ltc = CancellationTokenSource.CreateLinkedTokenSource(atc.Token,characterCtrl.destroyCancellationToken);
        var token = ltc.Token;
        UniTaskSafe.Forget
        (
            tc => ragdollAttack.ExecuteAttack(tc,characterCtrl.AttackDir, postAction, name),
            token,
            " weapon attack context"
        );
       

        characterCtrl.weaponEquip.SetFaceWeaponAttack(characterCtrl.AttackDir);
        characterCtrl.SendWeaponDamageBase();
       

        currentAttack.OnAttackEnd += EndAttack;
        characterCtrl.weaponEquip.OnDrop += WeaponEquip_OnDrop;
    }

   
    public override void Exit()
    {
      
        base.Exit();
        currentAttack.OnAttackEnd -= EndAttack;
        CancelAttack();
        currentAttack = null;

        if (characterCtrl.weaponEquip.CurrentWeapon != null)
        {
            characterCtrl.weaponEquip.CurrentWeapon.DisableAttack();
        }

        characterCtrl.weaponEquip.OnDrop -= WeaponEquip_OnDrop; 
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

    private void WeaponEquip_OnDrop()
    {
        stateMachine.ChangeState(characterCtrl.idelState);
    }
    private void EndAttack()
    {
        stateMachine.ChangeState(characterCtrl.idelState);
    }

}*/
#endregion

#region Test_2

public class MainWeaponAttackState : MainCharacterState
{
    private CancellationTokenSource atc;
    private CancellationTokenSource ltc;
    private IRagdollAttackSystem attackSystem;
    private IPostAction postAction;

    public MainWeaponAttackState(StateMachine stateMachine, CharacterCtrl characterCtrl) : base(stateMachine, characterCtrl)
    {
        InitAttackSystems();
    }

    public override void Enter()
    {
        base.Enter();

        if (!IsValidWeapon())
        {
            stateMachine.ChangeState(characterCtrl.idelState);
            return;
        }
        
        CheckConditionsAttack();

        characterCtrl.weaponEquip.CurrentWeapon.EnableAttack();
        characterCtrl.weaponEquip.CurrentWeapon.EnableEffect(true);

        // Action
        StartAttackAsync();

        characterCtrl.SendWeaponDamageBase();

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
        return characterCtrl.weaponEquip != null &&
               characterCtrl.weaponEquip.HasWeapon &&
               characterCtrl.weaponEquip.CurrentWeapon != null &&
               characterCtrl.attack != null;
    }

    #region Attack system
    // Object Attack Initialization

    private void InitAttackSystems()
    {
        attackSystem ??= new AttackWeaponPhysicSystem(
            characterCtrl.attack.PhysicsProfile,
            null
        );

        postAction ??= new PhysicPostAction(
            characterCtrl.ragdollController.ActionsDataSO,
            characterCtrl.ragdollController.Balances,
            Vector2.zero,
            characterCtrl.attack.PhysicsProfile
        );
    }

    //Set Conditions Attack
    private void CheckConditionsAttack()
    {
        AttackWeaponPhysicSystem physicSystem = attackSystem as AttackWeaponPhysicSystem;
        PhysicPostAction physicPost = postAction as PhysicPostAction;
        physicSystem?.SetWeapon(characterCtrl?.weaponEquip?.CurrentWeapon);
        physicPost.SetDir(characterCtrl.AttackDir);
    }

    // Execute Attack
    private void StartAttackAsync()
    {
        string name = characterCtrl.AttackDir.x > 0
            ? StringConst.WEAPON_RIGHT_PHYSIC
            : StringConst.WEAPON_LEFT_PHYSIC;

        atc = new CancellationTokenSource();
        ltc = CancellationTokenSource.CreateLinkedTokenSource(atc.Token, characterCtrl.destroyCancellationToken);
        var token = ltc.Token;


        UniTaskSafe.Forget(
            tc => attackSystem.ExecuteAttack(
                tc,
                characterCtrl.AttackDir,
                postAction,
                name),
            token,
            "MainWeaponAttackState"
        );
        characterCtrl.weaponEquip.SetFlipWeaponByAttackDir(characterCtrl.AttackDir);
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
        characterCtrl.weaponEquip.OnDrop += WeaponEquip_OnDrop;

    }

    private void UnsubscribeEvents()
    {
        attackSystem.OnAttackEnd -= EndAttack;
        characterCtrl.weaponEquip.OnDrop -= WeaponEquip_OnDrop;

    }

    private void WeaponEquip_OnDrop()
    {
        stateMachine.ChangeState(characterCtrl.idelState);
    }
    private void EndAttack()
    {
        CancelAttack();

        if (characterCtrl.weaponEquip.CurrentWeapon != null)
        {
            characterCtrl.weaponEquip.CurrentWeapon.EnableEffect(false);
            characterCtrl.weaponEquip.CurrentWeapon.DisableAttack();
        }

        stateMachine.ChangeState(characterCtrl.idelState);
    }

    #endregion

    #endregion
}
#endregion