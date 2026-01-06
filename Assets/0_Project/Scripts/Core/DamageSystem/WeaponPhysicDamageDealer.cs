using System.Collections.Generic;
using UnityEngine;

public class WeaponPhysicDamageDealer : MonoBehaviour
{
    //[SerializeField] float minImpact = 3f;
    [SerializeField] float damageMultiplier = 0.05f;
    [SerializeField] float maxDamage = 50f;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] Rigidbody2D rb;

    IAttackContext attackContext;
    IObjSendDamage objSendDamage;

    HashSet<GameObject> damagedTargets = new();

    public void Init(IObjSendDamage objSendDamage, IAttackContext attackContext)
    {
        //Debug.Log($"[InitWeapons] attackContext = {attackContext}");
        this.objSendDamage = objSendDamage;
        this.attackContext = attackContext;

        attackContext.OnAttackStart += AttackContext_OnAttackStart;
    }
    private void OnDestroy()
    {
        attackContext.OnAttackStart -= AttackContext_OnAttackStart;
    }
    private void AttackContext_OnAttackStart()
    {
        ClearTarget();
    }

    void OnEnable()
    {
        ClearTarget();
    }

    public void ClearTarget()
    {
        damagedTargets.Clear();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (attackContext == null || !attackContext.IsAttacking) return;

        if (objSendDamage == null) return;

        if (!col.transform.TryGetComponent(out IPhysicReceiveDamage hitBox)) return;

        GameObject target = hitBox.Owner;

        Debug.Log(target.name);

        if (!target || target == objSendDamage.OnjSend) return;
        if (damagedTargets.Contains(target)) return;

        if (!targetLayer.Contains(target.layer)) return;

        // float impact = rb.mass * col.relativeVelocity.sqrMagnitude;
        float impact = rb.mass * col.forceReceiveLayers;

        //if (impact < minImpact) return;

        float damage = Mathf.Clamp(impact * damageMultiplier, 0, maxDamage);
        //float rawDamage = impact * damageMultiplier;

        Debug.Log("1");
        damagedTargets.Add(target);

        Debug.Log("2");

        hitBox.ReceiveHit(damage, Vector2.up);
    }
}
