using UnityEngine;

public interface IPhysicReceiveDamage
{
    public GameObject Owner { get; }
    public void ReceiveHit(float rawDamage, Vector2 force);
}
