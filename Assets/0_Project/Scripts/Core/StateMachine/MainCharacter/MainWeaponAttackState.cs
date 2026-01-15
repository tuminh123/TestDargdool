using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class MainWeaponAttackState : MainCharacterState
{
    private IPostAction postAction;
    public MainWeaponAttackState(StateMachine stateMachine,CharacterCtrl characterCtrl) : base(stateMachine, characterCtrl)
    {
        postAction = new SmoothPostAction(characterCtrl?.ragdollController?.ActionsDataSO, characterCtrl?.ragdollController?.Balances, characterCtrl?.attack?.ConfigSO);
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

        string name = characterCtrl.AttackDir.x > 0 ? StringConst.WEAPON_RIGHT_ATTACK : StringConst.WEAPON_LEFT_ATTACK;
   
        if (characterCtrl.weaponEquip.CurrentWeapon == null) return;
        characterCtrl.weaponEquip.CurrentWeapon.EnableAttack();

        characterCtrl?.ragdollController?.postContext.SetPostAction(postAction);

        // Action
        characterCtrl?.attack?.attackContext?.ExecuteAttack(characterCtrl.AttackDir, characterCtrl?.ragdollController?.actionBase, name, characterCtrl.gameObject);
        characterCtrl.weaponEquip.SetFaceWeaponAttack(characterCtrl.AttackDir);
        characterCtrl.SendWeaponDamageBase();
       

        characterCtrl.attack.currentAttack.OnAttackEnd += EndAttack;
        characterCtrl.weaponEquip.OnDrop += WeaponEquip_OnDrop;
    }

   
    public override void Exit()
    {
      
        base.Exit();

        if (characterCtrl.weaponEquip.CurrentWeapon != null)
        {
            characterCtrl.weaponEquip.CurrentWeapon.DisableAttack();
        }

        characterCtrl.attack.currentAttack.OnAttackEnd -= EndAttack;
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
