using UnityEngine;

public abstract class VfxBase : MonoBehaviour, IObjectPool
{
    public abstract string GetObjectName();
}
