using UnityEngine;

public abstract class VfxBase : MonoBehaviour, IObjectPool,IResettable
{
    [SerializeField] protected ParticleSystem systems;

    //get
    public ParticleSystem System => systems;

    public abstract string GetObjectName();

    public void ResetOnGameRestart()
    {
        ZenManager.Instance.vfxPoolManager.DeSpawn(this);
    }
}
