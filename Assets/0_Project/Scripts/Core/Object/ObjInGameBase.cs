using UnityEngine;
public abstract class ObjInGameBase : MonoBehaviour, IObjectPool
{
    protected Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public abstract string GetObjectName();
}
