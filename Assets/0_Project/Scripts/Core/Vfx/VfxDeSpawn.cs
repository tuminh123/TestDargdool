using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Zenject;

public class VfxDeSpawn : MonoBehaviour
{
    [SerializeField] protected int timeDuration = 3;
    [SerializeField] protected VfxBase vfxBase;

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
        if (cts != null)
        {
            cts.Cancel();
            cts.Dispose();
            cts = null;
        }
    }

    public async UniTask WaitForDeSpawn(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(timeDuration * 1000, cancellationToken: token);

            ZenManager.Instance?.vfxPoolManager?.DeSpawnVfx(vfxBase);
        }
        catch (OperationCanceledException) { }
    }
}