using System.Collections;
using UnityEngine;

public abstract class BaseAttack : IAttackAction
{
    protected Balance body;
    protected float bodyForce;
    protected float damping;
    protected float stiffness;
    protected float duration;

    public bool IsAttacking { get; protected set; }

    public BaseAttack(Balance body, float bodyForce, float damping, float stiffness, float duration)
    {
        this.body = body;
        this.bodyForce = bodyForce;
        this.damping = damping;
        this.stiffness = stiffness;
        this.duration = duration;
    }

    protected abstract void SaveInitialState();
    protected abstract void ApplyAttack(Vector2 attackDir);
    protected abstract void ResetAttack();

    public IEnumerator Execute(Vector2 attackDir)
    {
        if (IsAttacking) yield break;
        IsAttacking = true;

        SaveInitialState();

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            ApplyAttack(attackDir);
            yield return null;
        }

        ResetAttack();
        IsAttacking = false;
    }
}
