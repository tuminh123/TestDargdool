using UnityEngine;

public class GoldBox : Box
{
    private float damage;
    public override string GetObjectName()
    {
        return StringConst.GOLDBOX;
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
