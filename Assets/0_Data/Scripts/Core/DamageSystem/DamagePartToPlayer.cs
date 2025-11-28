using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePartToPlayer : MonoBehaviour
{
    [SerializeField] private HealthBase myHealth;
    //[SerializeField] private LayerMask layer;
    //[SerializeField] private float radius;

    //public HealthBase GetHealthPlayer()
    //{
    //    Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius,layer);
    //    foreach (Collider2D col in cols)
    //    {
    //        if(col == null) continue;
    //        if(col.TryGetComponent(out HealthBase health))
    //        {
    //            Debug.Log(col.name);
    //            return health;
    //        }
    //    }
    //    return null;
    //}
    public void SetHealthLayer(GameObject target)
    {
        target.layer = LayerMask.NameToLayer("Enemy");
        myHealth.gameObject.layer = LayerMask.NameToLayer("Player");
    }
    public void StopHealthLayer(GameObject target)
    {
        target.layer = LayerMask.NameToLayer("Player");
        myHealth.gameObject.layer = LayerMask.NameToLayer("Enemy");
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, radius);
    //}

}
