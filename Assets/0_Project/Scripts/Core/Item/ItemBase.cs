using System.Collections;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour,IObjectPool
{
    [SerializeField] protected float force = 5f;
    public Rigidbody2D rb { get; private set; }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetVelocity()
    {
        rb.linearVelocity = Vector2.up * force;
    }
    public abstract string GetObjectName();
}