
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
    [Header("Component")]
    // component
    [SerializeField] protected Transform model;
    [SerializeField] protected BombDamageBase damage;

    protected CancellationTokenSource cts;

    public abstract void EffectSpawm();
    protected virtual void OnEnable()
    {
        SetVelocity();
        model.gameObject.SetActive(true);
        damage.DisableDamage();

        cts = new CancellationTokenSource();
        CancellationToken token = CancellationTokenSource.CreateLinkedTokenSource(cts.Token,this.GetCancellationTokenOnDestroy()).Token;
        UniTaskSafe.Forget
        (
            ct => BombHandle(ct),
            token,
            "Bomb!"
        );
    }
    protected virtual void OnDisable()
    {
        if (cts != null)
        {
            if (!cts.IsCancellationRequested) cts.Cancel();
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

            EffectSpawm();

            damage.EnableDamage();

            await UniTask.Delay(timeDeSpawn, cancellationToken:token);

            //vfxExplosion.StopVfx();
            damage.DisableDamage();
            ZenManager.Instance?.itemPoolManager?.DeSpawn(this);
          
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
