using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhysicsDamageDealer : MonoBehaviour
{
    [SerializeField] float minImpact = 3f;
    [SerializeField] float damageMultiplier = 0.05f;
    [SerializeField] float maxDamage = 50f;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] Rigidbody2D rb;

    IAttackContext attackContext;
    IObjSendDamage objSendDamage;

    HashSet<GameObject> damagedTargets = new();
    
    public void Init(IObjSendDamage objSendDamage, IAttackContext attackContext)
    {
        Debug.Log($"[InitWeapons] attackContext = {attackContext}");
        this.objSendDamage = objSendDamage;
        this.attackContext = attackContext;
    }

    void OnEnable()
    {
        damagedTargets.Clear();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (attackContext == null || !attackContext.IsAttacking) return;

        if (objSendDamage == null) return;

        if (!col.transform.TryGetComponent(out IPhysicReceiveDamage hitBox)) return;

        GameObject target = hitBox.Owner;
        if (!target || target == objSendDamage.OnjSend) return;
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
