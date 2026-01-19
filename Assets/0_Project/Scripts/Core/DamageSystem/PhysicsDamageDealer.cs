using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhysicsDamageDealer : APhysicDamageDeal
{
    [SerializeField] float minImpact = 3f;
    /* 
     [SerializeField] LayerMask targetLayer;
     [SerializeField] Rigidbody2D rb;
     [SerializeField] Faction faction;

     IAttackContext attackContext;
     IObjSendDamage objSendDamage;

     HashSet<GameObject> damagedTargets = new();

     public override void Init(IObjSendDamage objSendDamage, IAttackContext attackContext)
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
     }*/

    void OnCollisionEnter2D(Collision2D col)
    {
        if (attackContext == null || !attackContext.IsAttacking) return;

        if (objSendDamage == null) return;

        if (!col.transform.TryGetComponent(out IPhysicReceiveDamage hitBox)) return;

        GameObject target = hitBox.Owner;

        if (!target || target == objSendDamage.OnjSend) return;
        if (damagedTargets.Contains(target)) return;
        if (!targetLayer.Contains(target.layer)) return;
        if (faction == hitBox.Faction) return;

        //Debug.Log(hitBox.Faction.ToString());

        // float impact = rb.mass * col.relativeVelocity.sqrMagnitude;
        float impact = rb.mass * col.relativeVelocity.magnitude;
        if (impact < minImpact) return;
        float damage = Mathf.Clamp(impact * damageMultiplier, 0, maxDamage);
        //float rawDamage = impact * damageMultiplier;

        damagedTargets.Add(target);
        hitBox.ReceiveHit(damage, col.relativeVelocity);
    }
  /*  private void OnTriggerEnter2D(Collider2D col)
    {
        {
            if (attackContext == null || !attackContext.IsAttacking) return;

            if (objSendDamage == null) return;

            if (!col.transform.TryGetComponent(out IPhysicReceiveDamage hitBox)) return;

            GameObject target = hitBox.Owner;

            if (!target || target == objSendDamage.OnjSend) return;
            if (damagedTargets.Contains(target)) return;
            if (!targetLayer.Contains(target.layer)) return;
            if (faction == hitBox.Faction) return;
            //Debug.Log(hitBox.Faction.ToString());

            Rigidbody2D targetRb = col.attachedRigidbody;

            Vector2 relativeVelocity = rb.linearVelocity;
            if (targetRb != null)
                relativeVelocity -= targetRb.linearVelocity;

            float impact = rb.mass * relativeVelocity.magnitude;

            float damage = Mathf.Clamp(impact * damageMultiplier, 0, maxDamage);

            damagedTargets.Add(target);
            hitBox.ReceiveHit(damage, relativeVelocity);
        }
    }*/
}
