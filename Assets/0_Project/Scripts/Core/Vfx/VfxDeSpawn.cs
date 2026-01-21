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
    [SerializeField] protected float timeDuration = 3;
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
            if (!cts.IsCancellationRequested) cts.Cancel();
            cts.Dispose();
            cts = null;
        }
    }

    public async UniTask WaitForDeSpawn(CancellationToken token)
    {
        try
        {
            int time = Mathf.RoundToInt(timeDuration * 1000);
            await UniTask.Delay(time, cancellationToken: token);

            vfxBase.StopVfx();
            ZenManager.Instance?.vfxPoolManager?.DeSpawnVfx(vfxBase);
           
        }
        catch (OperationCanceledException e)
        {
            //Debug.LogException(e);
            Debug.LogWarning(e);
        }
        catch (System.Exception e)
        {
            //Debug.LogException(e);
            Debug.LogWarning(e);
        }
    }
}