using Unity.VisualScripting;
using UnityEngine;

public class PhysicsObjectDamageDealer : MonoBehaviour
{
    public event System.Action<AttackContext> OnDealDamage;
    [SerializeField] float minImpact = 20f;
    [SerializeField] float damageMultiplier = 0.05f;
    [SerializeField] float maxDamage = 100f;
    [SerializeField] LayerMask layer;

    [SerializeField]Rigidbody2D rb;
    AttackContext attackContext;
    public void ResetLayer()
    {
        SetTargetLayer(StringConst.NOTHING);
    }
    public void SetTargetLayer(string layer)
    {
        this.layer = LayerMask.GetMask(layer);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        OnDealDamage?.Invoke(attackContext);
        if (!attackContext.IsAttacking) return;

        if (!col.collider.TryGetComponent(out LimbHitBox hitBox)) return;

        if (!layer.Contains(hitBox.gameObject.layer)) return;

        float impact = rb.mass * col.relativeVelocity.sqrMagnitude;

        if (impact < minImpact) return;

        float damage = Mathf.Clamp(impact * damageMultiplier, 0, maxDamage);

        hitBox.ReceiveHit(damage, col.relativeVelocity);
    }
}
