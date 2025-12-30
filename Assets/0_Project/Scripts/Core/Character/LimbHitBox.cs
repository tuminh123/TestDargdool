using Core;
using UnityEngine;

public class LimbHitBox : GameElement,IReceive<SignalSendDamage>,IPhysicReceiveDamage
{
    [SerializeField] float damageScale = 1f;
    private float damage;
    CharacterParent owner;

    GameObject IPhysicReceiveDamage.Owner => owner.gameObject;

    public void Init(CharacterParent owner)
    {
        if (this.owner != null && this.owner != owner)
        {
            Debug.LogError($"[Limb ERROR] Owner changed from {this.owner.name} to {owner.name}");
            return;
        }
        this.owner = owner;
    }

    public void Receive(in SignalSendDamage signal)
    {
        damage = signal.damaged;
    }

    public void ReceiveHit(float rawDamage, Vector2 force)
    {
        if (owner == null) return;
        float finalDamage = damage * damageScale * rawDamage;

        owner.healthBase.TakeDamaged(finalDamage);

        owner.ragdollController.OnHit(force, rawDamage);
        
    }

}
