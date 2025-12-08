using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using UnityEngine;

public class ItemDeSpawn : MonoBehaviour
{

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
        cts.Cancel();
        cts.Dispose();
    }

    private void Awake()
    {
        itemBase = GetComponentInParent<ItemBase>();
    }

    public async UniTask WaitForDeSpawn(CancellationToken token)
    {
        await UniTask.Delay(timeDuration * 1000, cancellationToken: token);

        ItemDeSpawnHandle();
    }

    private void ItemDeSpawnHandle()
    {
        Transform holder = SingletonManager.Instance.itemPoolManager.Holder;
        SingletonManager.Instance.itemPoolManager.DeSpawn(itemBase);
        SingletonManager.Instance.itemPoolManager.SetParent(itemBase, holder);
    }
}