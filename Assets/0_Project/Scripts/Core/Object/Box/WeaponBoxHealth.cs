using Cysharp.Threading.Tasks;
using UnityEngine;

public class WeaponBoxHealth : BoxHealth
{
    public async override UniTask Spawn()
    {
        ZenManager.Instance.itemPoolManager.SpawnRandomWeapon(transform.position);

        await UniTask.Delay(200);
        ZenManager.Instance.objInGamePoolManager.DeSpawn(box);
    }
}
