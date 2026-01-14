using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class MainWeaponAttackState : MainCharacterState
{
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

      /*  AttackIntent intent = new AttackIntent
        {
            direction = characterCtrl.AttackDir,
            strength = 1f
        };*/
        string name = characterCtrl.AttackDir.x > 0 ? StringConst.WEAPON_RIGHT_PHYSIC : StringConst.WEAPON_LEFT_PHYSIC;

        // Init
        //characterCtrl?.ragdollController?.actionBase?.DisableBalance();
       
        //characterCtrl.attack.Init(characterCtrl?.ragdollController.actionBase, characterCtrl.gameObject, intent, name);
  
        //characterCtrl?.attack.SetAttack(new WeaponAttackPhysicOriginal(characterCtrl.weaponEquip.CurrentWeapon,characterCtrl.gameObject,characterCtrl?.attack.attackSystem,name));

        // Action
        //characterCtrl?.attack?.ExecuteAttack(characterCtrl.AttackDir);
        //characterCtrl.weaponEquip.SetFaceWeaponAttack(characterCtrl.AttackDir);
        characterCtrl.SendWeaponDamageBase();
       

        //characterCtrl.attack.attackSystem.OnAttackEnd += EndAttack;

        characterCtrl.weaponEquip.OnDrop += WeaponEquip_OnDrop;
    }

   
    public override void Exit()
    {
      
        base.Exit();
       /* characterCtrl.weaponEquip.CurrentWeapon.DisableAttack();
        characterCtrl?.ragdollController?.actionBase?.EnableBalance();
        characterCtrl.attack.attackSystem.OnAttackEnd -= EndAttack;
*/
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
