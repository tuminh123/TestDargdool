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
}