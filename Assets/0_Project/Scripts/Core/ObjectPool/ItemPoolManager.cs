using System.Collections;
using UnityEngine;


public class ItemPoolManager : ObjectPoolManager<ItemBase>
{
    public void SpawnRandomItem(Vector3 pos)
    {
        ItemBase item = Spawn(data.GetRandomPrefab(),pos,Quaternion.identity);
        if (item == null) return;
        item.SetVelocity();
    }
    public void SpawnGoldItem(Vector3 pos)
    {
        for (int i = 0; i < 5; i++)
        {
            Gold item = Spawn(StringConst.GOLD, pos, Quaternion.identity) as Gold;
            if (item == null) return;
            item.SetVelocity();
        }
      
    }
    public void SpawnRandomWeapon(Vector3 pos)
    {
        WeaponBase weapon = GetWeapon(pos);
        if (weapon == null) return;
        weapon.SetVelocity();
    }

    public WeaponBase GetWeapon(Vector3 pos)
    {
        string[] weaponKeys =
        {
        StringConst.ANCHORWEAPON,
        StringConst.WARAXEWEAPON,
        StringConst.IRONSTICKWEAPON,
        StringConst.KANABOWEAPON,
        StringConst.KATANAWEAPON,
        StringConst.LITTLEHAMMERWEAPON,
        StringConst.MACEWEAPON,
        StringConst.SHORTSWORDWEAPON,
        StringConst.SMALLAXEWEAPON,
        StringConst.SPEARWEAPON,
        StringConst.TWINSSWORDWEAPON
    };

        int rand = Random.Range(0, weaponKeys.Length);

        return Spawn(weaponKeys[rand], pos, Quaternion.identity) as WeaponBase;
    }
}