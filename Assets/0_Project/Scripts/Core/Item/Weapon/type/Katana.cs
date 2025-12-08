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
            Transform holder = SingletonManager.Instance.itemPoolManager.Holder;
            SingletonManager.Instance.itemPoolManager.DeSpawn(this);
            SingletonManager.Instance.itemPoolManager.SetParent(this, holder);
        }
    }
}
