using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class GoldBoxHealth : BoxHealth
{
    public async override UniTask Spawn()
    {
        if (ZenManager.Instance == null || ZenManager.Instance.itemPoolManager == null || ZenManager.Instance.objInGamePoolManager == null) return;

        ZenManager.Instance.itemPoolManager.SpawnGoldItem(transform.position);

        await UniTask.Delay(200);
        ZenManager.Instance.objInGamePoolManager.DeSpawn(box);
    }
}
