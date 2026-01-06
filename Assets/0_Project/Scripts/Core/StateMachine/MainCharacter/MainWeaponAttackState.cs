using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class MainWeaponAttackState : MainCharacterState
{
    CancellationTokenSource cts;

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

        cts = new CancellationTokenSource();

        if (characterCtrl.weaponEquip.CurrentWeapon == null) return;
        characterCtrl.weaponEquip.CurrentWeapon.EnableAttack();
        characterCtrl.SendWeaponDamageBase();

        characterCtrl.attack.HandleWeaponAttack(characterCtrl.AttackDir).Forget();
        characterCtrl.weaponEquip.SetFaceWeaponAttack(characterCtrl.AttackDir);

        if (characterCtrl.attack.currentAttackData == null) return;

        characterCtrl.attack.currentAttackData.OnAttacking += OnAttacking;
        characterCtrl.attack.currentAttackData.OnEndAttack += EndAttack;
        characterCtrl.weaponEquip.OnDrop += EndAttack;
    }

   

    public override void Exit()
    {
      
        base.Exit();

        if (characterCtrl.weaponEquip.CurrentWeapon == null) return;
        characterCtrl.weaponEquip.CurrentWeapon.DisableAttack();

        if (characterCtrl.attack.currentAttackData == null) return;
        characterCtrl.attack.currentAttackData.OnAttacking -= OnAttacking;
        characterCtrl.attack.currentAttackData.OnEndAttack -= EndAttack;
        characterCtrl.weaponEquip.OnDrop -= EndAttack;
    }

    private void OnAttacking()
    {
        //AttackAsync(cts.Token).Forget();
    }
    private void EndAttack()
    {
        //characterCtrl.SendDamage();
        //stateMachine.ChangeState(characterCtrl.idelState);
        //characterCtrl.attack.CancelAttack();

        cts?.Cancel();
        cts?.Dispose();

        stateMachine.ChangeState(characterCtrl.idelState);
    }

}
