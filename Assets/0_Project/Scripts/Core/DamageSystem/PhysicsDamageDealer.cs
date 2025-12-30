using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent (typeof(Collider2D),typeof(Rigidbody2D))]
public class PhysicsDamageDealer : MonoBehaviour
{
    [SerializeField] float minImpact = 3f;
    [SerializeField] float damageMultiplier = 0.05f;
    [SerializeField] float maxDamage = 50f;
    [SerializeField] LayerMask targetLayer;

    CharacterParent owner;
    Rigidbody2D rb;
    IAttackContext attackContext;
    HashSet<GameObject> damagedTargets = new();

    public void Init(CharacterParent owner, IAttackContext attackContext)
    {
        Debug.Log($"[InitWeapons] attackContext = {attackContext}");
        this.owner = owner;
        this.attackContext = attackContext;
    }

    void OnEnable()
    {
        damagedTargets.Clear();
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (attackContext == null || !attackContext.IsAttacking) return;

        if (!owner) return;

        if (!col.transform.TryGetComponent(out IPhysicReceiveDamage hitBox)) return;

        GameObject target = hitBox.Owner;
        if (!target || target == owner) return;
        if (damagedTargets.Contains(target)) return;

        if (!targetLayer.Contains(target.layer)) return;

        // float impact = rb.mass * col.relativeVelocity.sqrMagnitude;
        float impact = rb.mass * col.relativeVelocity.magnitude;

        if (impact < minImpact) return;

        float damage = Mathf.Clamp(impact * damageMultiplier, 0, maxDamage);
        //float rawDamage = impact * damageMultiplier;

        hitBox.ReceiveHit(damage, col.relativeVelocity);

        damagedTargets.Add(target);
    }
}
