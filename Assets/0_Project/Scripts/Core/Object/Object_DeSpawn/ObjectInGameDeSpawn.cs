using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
public class ObjectInGameDeSpawn : MonoBehaviour
{
    [SerializeField] protected int timeDuration;
    protected ObjInGameBase obj;
    private void Awake()
    {
        obj = GetComponentInParent<ObjInGameBase>();
    }
    private void Start()
    {
        WaitForDeSpawn().Forget();
    }
    public async UniTask WaitForDeSpawn()
    {
        while (true)
        {
            await UniTask.Delay(timeDuration * 1000);
            SingletonManager.Instance.objInGamePoolManager.DeSpawn(obj);
        }
    }

}
