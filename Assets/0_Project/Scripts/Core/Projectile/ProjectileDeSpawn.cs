using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class ProjectileDeSpawn : MonoBehaviour
{
    [SerializeField] protected int timeDuration = 5;
    protected ProjectileBase projectile;

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
        projectile = GetComponentInParent<ProjectileBase>();
    }

    public async UniTask WaitForDeSpawn(CancellationToken token)
    {
        await UniTask.Delay(timeDuration * 1000, cancellationToken: token);

        SingletonManager.Instance.projectilePoolManager.DeSpawn(projectile);
    }
}
