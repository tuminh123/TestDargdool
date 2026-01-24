using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;
public class ObjectInGameDeSpawn : MonoBehaviour
{
    [InjectOptional] private ObjInGamePoolManager objInGamePoolManager;
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

        UniTaskSafe.Forget
        (
            tc => WaitForDeSpawn(tc),
            linkedToken,
            $"{gameObject.name} despawn"
        );
        //WaitForDeSpawn(linkedToken).Forget();
    }

    private void OnDisable()
    {
        // Hủy task khi disable
        if (cts != null)
        {
            if (cts.IsCancellationRequested) cts.Cancel();
            cts.Dispose();
            cts = null;
        }
    }
    private void Awake()
    {
        obj = GetComponentInParent<ObjInGameBase>();
    }
    public async UniTask WaitForDeSpawn(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(timeDuration * 1000, cancellationToken: token);

            if (!token.IsCancellationRequested && obj != null) ZenManager.Instance.objInGamePoolManager.DeSpawn(obj);

        }
        catch (OperationCanceledException e)
        {
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ObjInGameDeSpawn] Unexpected error on {name}\n{e}");
        }
    }

}
