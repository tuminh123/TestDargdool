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

        if (!characterCtrl.weaponEquip.HasWeapon)
        {
            stateMachine.ChangeState(characterCtrl.idelState);
            return;
        }

        cts = new CancellationTokenSource();

        /*characterCtrl.attack.HandleWeaponAttack(characterCtrl.AttackDir).Forget();
        characterCtrl.weaponEquip.SetRotWhenAttack(characterCtrl.AttackDir);

        characterCtrl.SendDamageBase();

        if (characterCtrl.attack.currentAttackData == null) return;

        characterCtrl.attack.currentAttackData.OnAttacking += OnAttacking;
        characterCtrl.attack.currentAttackData.OnEndAttack += EndAttack;*/
    }

    private void OnAttacking()
    {
        //AttackAsync(cts.Token).Forget();
    }

    public override void Exit()
    {
      
        base.Exit();

      /*  if (characterCtrl.attack.currentAttackData == null) return;
        characterCtrl.attack.currentAttackData.OnAttacking -= OnAttacking;
        characterCtrl.attack.currentAttackData.OnEndAttack -= EndAttack;*/
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
