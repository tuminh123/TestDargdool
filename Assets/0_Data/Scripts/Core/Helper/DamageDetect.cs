using UnityEngine;
public class DamageDetect : MonoBehaviour
{
    [SerializeField] private float damageBase;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layer;
    private CharacterParent characterParent;

    //get
    public float Radius => radius;
    public LayerMask Layer => layer;
    private void Awake()
    {
        characterParent = GetComponentInParent<CharacterParent>();

        damageBase = characterParent.Stats.DamageBase;
    }

    public void SetDamageBase(float damageBase)
    {
        this.damageBase = damageBase;
    }

    public void SenderDamageTo()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, radius,layer);
        if (collider == null) return;

        IDamageable health = collider.GetComponent<IDamageable>();
        if(health == null) return;
        if (health.IsDead) return;

        health.TakeDamaged(damageBase);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
