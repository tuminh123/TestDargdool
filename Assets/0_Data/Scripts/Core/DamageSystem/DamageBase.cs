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
                health.TakeDamaged(damageBase);
                Debug.Log(damageBase);
                return true;
            }
        }
        return false; 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
