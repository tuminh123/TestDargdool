using System.Collections;
using UnityEngine;

public class WeaponPoolManager : ObjectPoolManager<WeaponBase>
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shuriken katana= Spawn(StringConst.SHURIKENWEAPON,transform.position,Quaternion.identity) as Shuriken;
            if (katana != null) katana.ResetWeapon();
        }
    }
}