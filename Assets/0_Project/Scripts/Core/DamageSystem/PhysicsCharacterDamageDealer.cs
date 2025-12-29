 using UnityEngine;

[RequireComponent (typeof(Collider2D),typeof(Rigidbody2D))]
public class PhysicsCharacterDamageDealer : MonoBehaviour
{
    [SerializeField] float minImpact = 3f;
    [SerializeField] float damageMultiplier = 0.05f;
    [SerializeField] float maxDamage = 50f;
    [SerializeField] LayerMask targetLayer;

    Rigidbody2D rb;
    AttackContext attackContext;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        attackContext = GetComponentInParent<AttackContext>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!attackContext.IsAttacking) return;

        if (!col.collider.TryGetComponent(out LimbHitBox hitBox)) return;

        if (!targetLayer.Contains(hitBox.gameObject.layer)) return;

        // float impact = rb.mass * col.relativeVelocity.sqrMagnitude;
        float impact = rb.mass * col.relativeVelocity.magnitude;

        if (impact < minImpact) return;

        float damage = Mathf.Clamp( impact * damageMultiplier,0,maxDamage);
        //float rawDamage = impact * damageMultiplier;

        hitBox.ReceiveHit(damage, col.relativeVelocity);
    }
}
