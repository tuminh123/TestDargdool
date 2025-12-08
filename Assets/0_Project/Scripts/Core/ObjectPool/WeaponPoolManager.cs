using System.Collections;
using UnityEngine;

public class WeaponPoolManager : ObjectPoolManager<WeaponBase>
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Katana katana= Spawn(StringConst.KATANAWEAPON,transform.position,Quaternion.identity) as Katana;
            if (katana != null) katana.ResetWeapon();
        }
    }
}