using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
public class ObjectInGameDeSpawn : MonoBehaviour
{
    [SerializeField] protected int timeDuration;
    protected ObjInGameBase obj;
    private CancellationTokenSource cts;

    private void OnEnable()
    {
        // Tạo token hủy khi Disable
        cts = new CancellationTokenSource();

        // Kết hợp token Disable + Destroy
        var linkedToken = CancellationTokenSource
            .CreateLinkedTokenSource(cts.Token, this.GetCancellationTokenOnDestroy())
            .Token;

        WaitForDeSpawn(linkedToken).Forget();
    }

    private void OnDisable()
    {
        // Hủy task khi disable
        cts.Cancel();
        cts.Dispose();
    }
    private void Awake()
    {
        obj = GetComponentInParent<ObjInGameBase>();
    }
    public async UniTask WaitForDeSpawn(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await UniTask.Delay(timeDuration * 1000, cancellationToken: token);

            SingletonManager.Instance.objInGamePoolManager.DeSpawn(obj);
        }
    }

}
