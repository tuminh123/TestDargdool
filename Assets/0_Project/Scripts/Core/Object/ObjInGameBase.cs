using UnityEngine;
public abstract class ObjInGameBase : MonoBehaviour, IObjectPool,IResettable
{
    protected Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public abstract string GetObjectName();

    public void ResetOnGameRestart()
    {
        ZenManager.Instance.objInGamePoolManager.DeSpawn(this);
    }
}
