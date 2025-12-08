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

        /*PlayerWeaponEquip weaponEquip = characterCtrl?.weaponEquip;
        WeaponBase weapon = weaponEquip?.currentWeapon;
        if (weapon == null) return;

        weapon.RotateByDirection(characterCtrl.AttackDir);*/

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
        PlayerWeaponEquip weaponEquip = characterCtrl?.weaponEquip;
        WeaponBase weapon = weaponEquip?.currentWeapon;
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

                WeaponDamage damage = weapon.weaponDamage;
                if (damage == null) return;

                if (damage.SenderDamageTo())
                {
                    characterCtrl.weaponEquip.ResetEquip();
                }

                break;
            case WeaponType.RANGE:

                IShoot shoot = GetIShootByType(weapon);
                if (shoot == null) return;
                shoot.Shoot(characterCtrl.AttackDir);

                characterCtrl.weaponEquip.ResetEquip();
                break;
        }
    }
    private void WeaponRotationHandle(WeaponBase weapon)
    {
        if (characterCtrl.AttackDir.x < 0)
        {
            weapon.rb.MovePosition (new Vector3(-1, 1, 1));
        }
        else
        {
            weapon.rb.MovePosition(new Vector3(1, 1, 1));
        }
    }
    private IShoot GetIShootByType(WeaponBase weapon)
    {
        if(weapon.TryGetComponent(out IShoot shoot)) return shoot;
        return null;
    }
}