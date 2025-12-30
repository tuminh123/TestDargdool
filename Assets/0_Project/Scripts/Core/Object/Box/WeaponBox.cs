using UnityEngine;

public class WeaponBox : Box
{
    private float damage;
    public override string GetObjectName()
    {
        return StringConst.WEAPONBOX;
    }

    public override GameObject GetOwner()
    {
        return transform.gameObject;
    }

    public override void Receive(in SignalSendDamage signal)
    {
        damage = signal.damaged;
    }

    public override void ReceiveHit(float rawDamage, Vector2 force)
    {
        float finalDamage = damage * rawDamage;

        boxHealth.TakeDamaged(finalDamage);
    }
}
