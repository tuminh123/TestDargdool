using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using Zenject;

public class ItemDeSpawn : MonoBehaviour
{
    [InjectOptional] private ItemPoolManager itemPoolManager;
    [SerializeField] protected int timeDuration = 5;
    protected ItemBase itemBase;

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

    private void Awake()
    {
        itemBase = GetComponentInParent<ItemBase>();
    }

    public async UniTask WaitForDeSpawn(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(timeDuration * 1000, cancellationToken: token);

            if (!token.IsCancellationRequested && itemBase != null) ItemDeSpawnHandle();
        }
        catch (OperationCanceledException) { }
    }

    private void ItemDeSpawnHandle()
    {
        Transform holder = ZenManager.Instance. itemPoolManager.Holder;
        ZenManager.Instance.itemPoolManager.DeSpawn(itemBase);
        ZenManager.Instance. itemPoolManager.SetParent(itemBase, holder);
    }
}