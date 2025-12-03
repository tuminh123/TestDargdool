
using UnityEngine;
public abstract class DamageBase : MonoBehaviour
{
    [SerializeField] protected float damageBase;
    [SerializeField] protected float radius;
    [SerializeField] protected LayerMask layer;

    //get
    public float Radius => radius;
    public LayerMask Layer => layer;

    public void SetDamageBase(float damageBase)
    {
        this.damageBase = damageBase;
    }

    public bool SenderDamageTo()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius,layer);
        foreach (Collider2D collider in colliders)
        {
            if(collider == null) continue;
            if(collider.TryGetComponent(out IDamageable health))
            {
                if (health.IsDead) return false;
                DamageHandle(health);
                return true;
            }
        }
        return false; 
    }

    private void DamageHandle(IDamageable health)
    {
        //Debug.Log($"1 ");
        health.TakeDamaged(damageBase);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
