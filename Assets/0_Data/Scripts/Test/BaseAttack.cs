using System.Collections;
using UnityEngine;

public interface IAttack
{
    void Execute(Vector2 direction);
    float Duration { get; }
}

public abstract class BaseAttack : IAttack
{
    protected float attackForce;
    protected float damping;
    protected float stiffness;
    protected float duration;

    public float Duration => duration;

    public BaseAttack(float attackForce, float duration = 0.25f, float damping = 0.9f, float stiffness = 8f)
    {
        this.attackForce = attackForce;
        this.duration = duration;
        this.damping = damping;
        this.stiffness = stiffness;
    }

    public abstract void Execute(Vector2 direction);
}
