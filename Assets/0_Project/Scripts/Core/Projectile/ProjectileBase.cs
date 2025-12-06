using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour, IObjectPool
{
    [SerializeField] protected float speed = 5;
    public Rigidbody2D rb { get; private set; }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void ProjectileMoving(Vector2 dir)
    {
        rb.linearVelocity = dir*speed;
    }
    public abstract string GetObjectName();
}
