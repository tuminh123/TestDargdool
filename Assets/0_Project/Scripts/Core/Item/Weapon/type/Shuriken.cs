using UnityEngine;
using Zenject;

public class Shuriken : WeaponBase,IShoot
{
    [InjectOptional] private ProjectilePoolManager projectilePoolManager;
    //RangeWeaponType IShoot.type => RangeWeaponType.Shuriken;

    public override string GetObjectName()
    {
        return StringConst.SHURIKENWEAPON;
    }

    public void Shoot(Vector2 dir)
    {
        ShurikenProjectile shuriken = ZenManager.Instance. projectilePoolManager.Spawn(StringConst.SHURIKENPROJECTILE, transform.position, Quaternion.identity) as ShurikenProjectile;
        shuriken.ProjectileMoving(dir);
    }
}
