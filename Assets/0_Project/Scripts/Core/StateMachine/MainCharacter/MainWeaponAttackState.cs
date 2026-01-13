using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class MainWeaponAttackState : MainCharacterState
{
    string name;
    public MainWeaponAttackState(
        StateMachine stateMachine,
        CharacterCtrl characterCtrl
    ) : base(stateMachine, characterCtrl) { }

    public override void Enter()
    {
        base.Enter();

        if (characterCtrl.weaponEquip == null || characterCtrl.attack == null) return;
        if (!characterCtrl.weaponEquip.HasWeapon)
        {
            stateMachine.ChangeState(characterCtrl.idelState);
            return;
        }

        if (characterCtrl.weaponEquip.CurrentWeapon == null) return;
        characterCtrl.weaponEquip.CurrentWeapon.EnableAttack();
       

        string name = characterCtrl.AttackDir.x > 0 ? "PhysicWeaponRightPunch" : "PhysicWeaponLeftPunch";
        this.name = name;

        // Init
        characterCtrl?.ragdollController?.actionBase?.DisableBalance(name);
        characterCtrl.InitAttack(name);
        characterCtrl?.attack.SetAttack(new WeaponAttackPhysicOriginal(characterCtrl.weaponEquip.CurrentWeapon,characterCtrl.gameObject,characterCtrl?.attack.attackSystem,name));

        // Action

        characterCtrl.weaponEquip.SetFaceWeaponAttack(characterCtrl.AttackDir);
        characterCtrl?.attack?.ExecuteAttack(characterCtrl.AttackDir);
        characterCtrl.SendWeaponDamageBase();
       

        characterCtrl.attack.attackSystem.OnAttackEnd += EndAttack;

        characterCtrl.weaponEquip.OnDrop += WeaponEquip_OnDrop;
    }

   
    public override void Exit()
    {
      
        base.Exit();
        characterCtrl.weaponEquip.CurrentWeapon.DisableAttack();
        characterCtrl?.ragdollController?.actionBase?.EnableBalance(name);
        characterCtrl.attack.attackSystem.OnAttackEnd -= EndAttack;

        characterCtrl.weaponEquip.OnDrop -= WeaponEquip_OnDrop; 
    }
    private void WeaponEquip_OnDrop()
    {
        stateMachine.ChangeState(characterCtrl.idelState);
    }
    private void EndAttack()
    {
        stateMachine.ChangeState(characterCtrl.idelState);
    }

}
