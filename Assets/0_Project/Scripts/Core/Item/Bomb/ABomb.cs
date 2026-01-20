
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;
public abstract class ABomb : ItemBase
{
    [SerializeField] protected float timeExplosion = 2;
    [SerializeField] protected float timeDeSpawn = 1;
    [Space]
    [Header("Inject")]
    [Space]
    [Header("Component")]
    // component
    [SerializeField] protected Transform model;
    [SerializeField] protected DamageBase damage;

    protected CancellationTokenSource cts;
    protected override void Awake()
    {
        base.Awake();
        model.gameObject.SetActive(true);

        //SetVelocity();
    }
    private void OnEnable()
    {
        cts = new CancellationTokenSource();
        CancellationToken token = CancellationTokenSource.CreateLinkedTokenSource(cts.Token,this.GetCancellationTokenOnDestroy()).Token;
        UniTaskSafe.Forget
        (
            ct => BombHandle(ct),
            token,
            "Bomb!"
        );
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

    public async UniTask BombHandle(CancellationToken token)
    {
        try
        {
            int timeExplosion = Mathf.RoundToInt(1000 * this.timeExplosion);
            int timeDeSpawn = Mathf.RoundToInt(1000*this.timeDeSpawn);
            await UniTask.Delay(timeExplosion, cancellationToken: token);

            model.gameObject.SetActive(false);

            VfxBase vfxExplosion = null;
            VfxBase vfxFire = null;
            ZenManager.Instance?.vfxPoolManager?.SpawnVfx(StringConst.EXPLOSIONVFX, gameObject, out vfxExplosion);

            Collider2D[] col = null; 
            if (damage.SenderDamageTo(out col))
            {
                foreach (var item in col)
                {
                    if (item == null) continue;
                    ZenManager.Instance?.vfxPoolManager?.SpawnVfx(StringConst.FIREVFX,item.gameObject, out vfxFire);
                }
            }

            await UniTask.Delay(timeDeSpawn, cancellationToken:token);

            ZenManager.Instance?.itemPoolManager?.DeSpawn(this);
          
        }
        catch (OperationCanceledException e)
        {
            Debug.LogException(e);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
}
