using System.Collections.Generic;
using UnityEngine;

public class WeaponPhysicDamageDealer : MonoBehaviour
{
    public event System.Action OnIsSendDamage;

    //[SerializeField] float minImpact = 3f;
    [SerializeField] float damageMultiplier = 0.05f;
    [SerializeField] float maxDamage = 50f;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Faction faction;

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
    public void SetFaction(Faction faction)
    {
        this.faction = faction;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (attackContext == null || !attackContext.IsAttacking) return;

        if (objSendDamage == null) return;

        if (!col.transform.TryGetComponent(out IPhysicReceiveDamage hitBox)) return;

        GameObject target = hitBox.Owner;

        //Debug.Log(target.name);

        if (!target || target == objSendDamage.OnjSend) return;
        if (damagedTargets.Contains(target)) return;
        if (faction == hitBox.Faction) return;
        if (!targetLayer.Contains(target.layer)) return;
        damagedTargets.Add(target);

        OnIsSendDamage?.Invoke();

        float impact = rb.mass * rb.linearVelocity.magnitude;
        //if (impact < minImpact) return;
        float damage = Mathf.Clamp(impact * damageMultiplier, 0, maxDamage);
        //float rawDamage = impact * damageMultiplier;
        //Debug.Log("2");
        hitBox.ReceiveHit(damage, Vector2.up);

    }
}
