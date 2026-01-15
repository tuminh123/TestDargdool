using UnityEngine;

public enum Faction
{
    None = 0,
    Enemy = 1,
    Ally = 2,
    Obj = 3,
}
public interface IPhysicReceiveDamage
{
    public GameObject Owner { get; }
    public Faction Faction { get; }
    public void ReceiveHit(float rawDamage, Vector2 force);
}
