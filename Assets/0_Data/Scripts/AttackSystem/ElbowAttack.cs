using UnityEngine;

public class ElbowAttack : BaseAttack
{
    private Balance arm, elbow;
    private float targetArmRot, targetElbowRot;
    private float initArm, initElbow;
    private float force;

    public ElbowAttack(Balance arm, Balance elbow,
        Balance body, float targetArmRot, float targetElbowRot,
        float force, float bodyForce,
        float damping, float stiffness, float duration)
        : base(body, bodyForce, damping, stiffness, duration)
    {
        this.arm = arm;
        this.elbow = elbow;
        this.targetArmRot = targetArmRot;
        this.targetElbowRot = targetElbowRot;
        this.force = force;
    }

    protected override void SaveInitialState()
    {
        initArm = arm.TargetRotation;
        initElbow = elbow.TargetRotation;
    }

    protected override void ApplyAttack(Vector2 attackDir)
    {
        arm.SetTargetRotation(Mathf.LerpAngle(arm.TargetRotation, targetArmRot, stiffness * Time.deltaTime));
        elbow.SetTargetRotation(Mathf.LerpAngle(elbow.TargetRotation, targetElbowRot, stiffness * Time.deltaTime));

        arm.Rb.linearVelocity = Vector2.Lerp(arm.Rb.linearVelocity, attackDir * force, 0.15f);
        elbow.Rb.linearVelocity = Vector2.Lerp(elbow.Rb.linearVelocity, attackDir * force * 0.8f, 0.15f);

        body.Rb.linearVelocity = Vector2.Lerp(body.Rb.linearVelocity, attackDir * bodyForce, 0.1f);

        arm.Rb.linearVelocity *= damping;
        elbow.Rb.linearVelocity *= damping;
        body.Rb.linearVelocity *= damping;
    }

    protected override void ResetAttack()
    {
        arm.SetTargetRotation(initArm);
        elbow.SetTargetRotation(initElbow);

        arm.Rb.linearVelocity = Vector2.zero;
        elbow.Rb.linearVelocity = Vector2.zero;
        body.Rb.linearVelocity = Vector2.zero;
    }
}
