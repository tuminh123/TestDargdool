using UnityEngine;

public class KneeAttack : BaseAttack
{
    private Balance leg;
    private float targetLegRot;
    private float initLeg;
    private float force;

    public KneeAttack(Balance leg,
        Balance body, float targetLegRot,
        float force, float bodyForce,
        float damping, float stiffness, float duration)
        : base(body, bodyForce, damping, stiffness, duration)
    {
        this.leg = leg;
        this.targetLegRot = targetLegRot;
        this.force = force;
    }

    protected override void SaveInitialState()
    {
        initLeg = leg.TargetRotation;
    }

    protected override void ApplyAttack(Vector2 attackDir)
    {
        leg.SetTargetRotation(Mathf.LerpAngle(leg.TargetRotation, targetLegRot, stiffness * Time.deltaTime));
        leg.Rb.linearVelocity = Vector2.Lerp(leg.Rb.linearVelocity, attackDir * force, 0.15f);

        body.Rb.linearVelocity = Vector2.Lerp(body.Rb.linearVelocity, attackDir * bodyForce, 0.1f);

        leg.Rb.linearVelocity *= damping;
        body.Rb.linearVelocity *= damping;
    }

    protected override void ResetAttack()
    {
        leg.SetTargetRotation(initLeg);
        leg.Rb.linearVelocity = Vector2.zero;
        body.Rb.linearVelocity = Vector2.zero;
    }
}
