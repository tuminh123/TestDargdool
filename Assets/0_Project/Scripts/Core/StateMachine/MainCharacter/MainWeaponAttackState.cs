using Cysharp.Threading.Tasks;
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

        characterCtrl.attack.HandleWeaponAttack(characterCtrl.AttackDir).Forget();
        AttackAsync(cts.Token).Forget();
       

        if (characterCtrl.attack.currentAttackData == null) return;

        characterCtrl.attack.currentAttackData.OnEndAttack += EndAttack;
    }

    public override void Exit()
    {
      
        base.Exit();

        if (characterCtrl.attack.currentAttackData == null) return;

        characterCtrl.attack.currentAttackData.OnEndAttack -= EndAttack;
    }
    private void EndAttack()
    {
        //characterCtrl.SendDamage();
        //stateMachine.ChangeState(characterCtrl.idelState);
        characterCtrl.attack.StopAttack();

        cts?.Cancel();
        cts?.Dispose();

        stateMachine.ChangeState(characterCtrl.idelState);
    }

    async UniTaskVoid AttackAsync(CancellationToken token)
    {
        HandController hand = characterCtrl.weaponEquip.ActiveHand;

        float direction = characterCtrl.AttackDir.x;
        float force = 50f;

        // Vung tay
        hand.Swing(direction, force);

        // Thời gian chờ attack kết thúc
        await UniTask.Delay(300, cancellationToken: token);

        /*// Nếu vẫn ở state này → quay lại idle
        if (!token.IsCancellationRequested)
            stateMachine.ChangeState(characterCtrl.idelState);*/
    }
}
