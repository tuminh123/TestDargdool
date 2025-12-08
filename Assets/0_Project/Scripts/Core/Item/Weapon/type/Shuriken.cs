using UnityEngine;

public class Shuriken : WeaponBase,IShoot
{
    //RangeWeaponType IShoot.type => RangeWeaponType.Shuriken;

    public override string GetObjectName()
    {
        return StringConst.SHURIKENWEAPON;
    }

    public void Shoot(Vector2 dir)
    {
        ShurikenProjectile shuriken = SingletonManager.Instance.projectilePoolManager.Spawn(StringConst.SHURIKENPROJECTILE, transform.position, Quaternion.identity) as ShurikenProjectile;
        shuriken.ProjectileMoving(dir);
    }
}
