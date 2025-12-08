using System.Collections;
using UnityEngine;

public class ItemPoolManager : ObjectPoolManager<ItemBase>
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Katana katana= Spawn(StringConst.KATANAWEAPON,transform.position,Quaternion.identity) as Katana;
            if (katana != null) katana.ResetWeapon();
        }
    }
    public void SpawnRandomItem(Vector3 pos)
    {
        ItemBase item = Spawn(data.GetRandomPrefab(),pos,Quaternion.identity);
        if (item == null) return;
        item.SetVelocity();
    }
}