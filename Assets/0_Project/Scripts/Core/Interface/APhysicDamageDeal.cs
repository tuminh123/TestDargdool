using System.Collections.Generic;
using UnityEngine;
public abstract class APhysicDamageDeal : MonoBehaviour
{
    [SerializeField] protected float damageMultiplier = 0.05f;
    [SerializeField] protected float maxDamage = 50f;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Faction faction;

    protected IAttackContext attackContext;
    protected IObjSendDamage objSendDamage;

    protected HashSet<GameObject> damagedTargets = new();

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

}
