
using UnityEngine;
public class DamageBase : MonoBehaviour
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

    public virtual bool SenderDamageTo()
    {
        #region test
        /*  Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius,layer);
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
          return false;*/
        #endregion

        Collider2D[] colliders =
       Physics2D.OverlapCircleAll(transform.position, radius, layer);

        foreach (Collider2D collider in colliders)
        {
            Debug.Log("1");
            if (collider == null) continue;

            if (!collider.TryGetComponent(out IDamageable health))
                continue;

            if (health.IsDead)
                continue;
            Debug.Log("2");
            DamageHandle(health);
            return true;
        }

        return false;

    }
    
    protected void DamageHandle(IDamageable health)
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
