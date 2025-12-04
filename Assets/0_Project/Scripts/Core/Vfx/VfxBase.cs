using UnityEngine;

public abstract class VfxBase : MonoBehaviour, IObjectPool
{
    [SerializeField] protected ParticleSystem systems;

    //get
    public ParticleSystem System => systems;

    public abstract string GetObjectName();
}
