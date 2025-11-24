using UnityEngine;
public class DamageDetect : MonoBehaviour
{
    [SerializeField] private float damageBase;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layer;

    //get
    public float Radius => radius;
    public LayerMask Layer => layer;

    public void SenderDamageTo()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, radius,layer);
        if (collider == null) return;

        IDamageable health = collider.GetComponent<IDamageable>();
        if(health == null) return;

        health.TakeDamaged(damageBase);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
