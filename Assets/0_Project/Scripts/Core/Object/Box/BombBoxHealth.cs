using Cysharp.Threading.Tasks;
using UnityEngine;

public class BombBoxHealth : BoxHealth
{
    public async override UniTask Spawn()
    {
        if (ZenManager.Instance == null || ZenManager.Instance.itemPoolManager == null || ZenManager.Instance.objInGamePoolManager == null) return;

        ZenManager.Instance?.itemPoolManager?.Spawn(StringConst.BOMB_BASE,transform.position,Quaternion.identity);

        await UniTask.Delay(200);
        ZenManager.Instance.objInGamePoolManager.DeSpawn(box);
    }
}
