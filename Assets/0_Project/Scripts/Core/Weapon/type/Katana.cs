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
            Transform holder = SingletonManager.Instance.weaponPoolManager.Holder;
            SingletonManager.Instance.weaponPoolManager.DeSpawn(this);
            SingletonManager.Instance.weaponPoolManager.SetParent(this, holder);
        }
    }
}
