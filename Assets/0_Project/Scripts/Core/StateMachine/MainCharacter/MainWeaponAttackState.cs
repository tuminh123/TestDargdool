using System.Collections;
using UnityEngine;

public class MainWeaponAttackState : MainCharacterState
{
    public MainWeaponAttackState(StateMachine stateMachine, CharacterCtrl characterCtrl) : base(stateMachine, characterCtrl)
    {
    }
    public override void Enter()
    {
        base.Enter();
        characterCtrl.attack.HandleWeaponAttack(characterCtrl.AttackDir);

        WeaponBase weapon = characterCtrl?.currentWeaponBase;
        if (weapon == null) return;

        WeaponRotationHandle(weapon);

        if (characterCtrl.attack.currentAttackData == null) return;

        characterCtrl.attack.currentAttackData.OnAttacking += Attacking;
        characterCtrl.attack.currentAttackData.OnAttackEnd += EndAttack;

    }

    public override void Exit()
    {
        base.Exit();

        characterCtrl.attack.StopAttack();

        if (characterCtrl.attack.currentAttackData == null) return;

        characterCtrl.attack.currentAttackData.OnAttacking -= Attacking;
        characterCtrl.attack.currentAttackData.OnAttackEnd -= EndAttack;

    }
    private void Attacking()
    {
        Debug.Log("Attack By Weapon");
        //characterCtrl.SendDamage();
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
        if(weapon.Type == WeaponType.MELE)
        {
            WeaponDamage damage = weapon.weaponDamage;
            if (damage == null) return;

            if (damage.SenderDamageTo())
            {
                RemoveWeapon(weapon);
            }
        }
        else if(weapon.Type == WeaponType.RANGE)
        {
            GetIShootByType(RangeWeaponType.Shuriken, weapon).Shoot(characterCtrl.AttackDir);
            RemoveWeapon(weapon);
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
        characterCtrl.weaponEquip.SetIsEquipping(false);
        SingletonManager.Instance.weaponPoolManager.DeSpawn(weapon);
    }
    private IShoot GetIShootByType(RangeWeaponType type,WeaponBase weapon)
    {
        if(weapon.TryGetComponent(out IShoot shoot))
        {
            if(shoot.type == type) return shoot;
        }
        return null;
    }
}