using System;
using UnityEngine;
public class Katana : WeaponBase
{
    public override string GetObjectName()
    {
        return StringConst.KATANAWEAPON;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SingletonManager.Instance.weaponPoolManager.DeSpawn(this);
        }
    }
}
