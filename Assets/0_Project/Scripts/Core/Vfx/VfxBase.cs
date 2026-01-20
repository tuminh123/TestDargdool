using UnityEngine;

public abstract class VfxBase : MonoBehaviour, IObjectPool,IResettable
{
    [SerializeField] protected ParticleSystem[] systems;
    
    public abstract string GetObjectName();

    public void PlayVfx()
    {
        if(systems.Length <= 0) return;
        foreach (var item in systems)
        {
            if (item == null) continue;
            item.Play();
        }
    }
    public void StopVfx()
    {
        if (systems.Length <= 0) return;
        foreach (var item in systems)
        {
            if (item == null) continue;
            item.Stop();
        }
    }

    public void ResetOnGameRestart()
    {
        ZenManager.Instance.vfxPoolManager.DeSpawn(this);
    }
}
