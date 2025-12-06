using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using UnityEngine;

public class WeaponDeSpawn : MonoBehaviour
{

    [SerializeField] protected int timeDuration = 5;
    protected WeaponBase weapon;

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
        weapon = GetComponentInParent<WeaponBase>();
    }

    public async UniTask WaitForDeSpawn(CancellationToken token)
    {
        await UniTask.Delay(timeDuration * 1000, cancellationToken: token);

        WeaponDespawnHandle();
    }

    private void WeaponDespawnHandle()
    {
        Transform holder = SingletonManager.Instance.weaponPoolManager.Holder;
        SingletonManager.Instance.weaponPoolManager.DeSpawn(weapon);
        SingletonManager.Instance.weaponPoolManager.SetParent(weapon, holder);
    }
}