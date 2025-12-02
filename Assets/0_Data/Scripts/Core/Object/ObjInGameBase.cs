using UnityEngine;
public abstract class ObjInGameBase : MonoBehaviour, IObjectPool
{
    [SerializeField] protected float speed = 2f;
    protected Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetVelocity()
    {
        rb.linearVelocity = Vector2.up * speed;
    }
    public abstract string GetObjectName();
}
