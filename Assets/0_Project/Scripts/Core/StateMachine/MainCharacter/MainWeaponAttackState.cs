using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using UnityEngine;
public class MainWeaponAttackState : MainCharacterState
{
    private CancellationTokenSource unWeapon;
    public MainWeaponAttackState(StateMachine stateMachine, CharacterCtrl characterCtrl) : base(stateMachine, characterCtrl)
    {
    }
    public override void Enter()
    {
        base.Enter();
        characterCtrl.attackContext.EnableAttack();
        unWeapon = new CancellationTokenSource();

        characterCtrl.attack.HandleWeaponAttack(characterCtrl.AttackDir).Forget();

        WeaponBase weapon = characterCtrl?.currentWeaponBase;
        if (weapon == null) return;

        WeaponRotationHandle(weapon);

        weapon.EnableAttack();
        weapon.rb.linearVelocity = characterCtrl.AttackDir * 15f;

        if (characterCtrl.attack.currentAttackData == null) return;

        characterCtrl.attack.currentAttackData.OnAttacking += Attacking;
        characterCtrl.attack.currentAttackData.OnEndAttack += EndAttack;

    }

    public override void Exit()
    {
        base.Exit();

        characterCtrl.attackContext.DisableAttack();

        WeaponBase weapon = characterCtrl?.currentWeaponBase;
        if (weapon == null) return;
        weapon.DisableAttack();

        unWeapon?.Cancel();
        unWeapon?.Dispose();

        characterCtrl.attack.StopAttack();

        if (characterCtrl.attack.currentAttackData == null) return;

        characterCtrl.attack.currentAttackData.OnAttacking -= Attacking;
        characterCtrl.attack.currentAttackData.OnEndAttack -= EndAttack;

    }
    private void Attacking()
    {
       /* Debug.Log("Attack By Weapon");
        //characterCtrl.SendDamage();
        ZenManager.Instance.cameraShaker.ShakeCam();*/
    }
    private void EndAttack()
    {
        WeaponBase weapon = characterCtrl?.currentWeaponBase;
        if (weapon == null) return;

        WeaponAttackHandle(weapon);
        
        stateMachine.ChangeState(characterCtrl.idelState);
    }

    private void WeaponAttackHandle(WeaponBase weapon)
    {
        switch (weapon.Type)
        {
            default:
            case WeaponType.MELE:

                if (!weapon.IsAttacking)
                {
                    RemoveWeapon(weapon);
                }

                break;
            case WeaponType.RANGE:

                IShoot shoot = GetIShootByType(weapon);
                if (shoot == null) return;
                shoot.Shoot(characterCtrl.AttackDir);
                RemoveWeapon(weapon);

                break;
        }
    }

    private void WeaponRotationHandle(WeaponBase weapon)
    {
        if (characterCtrl.AttackDir.x < 0)
        {
            weapon.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            weapon.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    private void RemoveWeapon(WeaponBase weapon)
    {
        if (weapon == null)
        {
            Debug.LogWarning("RemoveWeapon called but weapon is NULL");
            return;
        }
        // Cancel task cũ nếu có
        unWeapon?.Cancel();
        unWeapon?.Dispose();

        unWeapon = new CancellationTokenSource();

        // Link với lifecycle Character
        var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(
            unWeapon.Token,
            characterCtrl.GetCancellationTokenOnDestroy()
        ).Token;

        characterCtrl.weaponEquip.UnEquipping();

        UniTaskSafe.Forget(
            weapon.UnEquip,
            linkedToken,
            "Remove Weapon"
        );
    }

    private IShoot GetIShootByType(WeaponBase weapon)
    {
        if(weapon.TryGetComponent(out IShoot shoot)) return shoot;
        return null;
    }
}