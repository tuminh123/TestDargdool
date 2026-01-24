using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

public class ProjectileDeSpawn : MonoBehaviour
{
    [InjectOptional] private ProjectilePoolManager projectilePoolManager;
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
        if (cts != null)
        {
            cts.Cancel();
            cts.Dispose();
            cts = null;
        }
    }

    private void Awake()
    {
        projectile = GetComponentInParent<ProjectileBase>();
    }

    public async UniTask WaitForDeSpawn(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(timeDuration * 1000, cancellationToken: token);

            ZenManager.Instance?.projectilePoolManager?.DeSpawn(projectile);

        }
        catch (OperationCanceledException e)
        {
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ProjectileDeSpawn] Unexpected error on {name}\n{e}");
        }
      
    }
}
