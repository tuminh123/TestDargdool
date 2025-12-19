using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class GoldBoxHealth : BoxHealth
{
    public async override UniTask Spawn()
    {
        ZenManager.Instance.itemPoolManager.SpawnGoldItem(transform.position);

        await UniTask.Delay(200);
        ZenManager.Instance.objInGamePoolManager.DeSpawn(box);
    }
}
