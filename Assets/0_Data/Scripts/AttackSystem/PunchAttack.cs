using UnityEngine;

public class PunchAttack : BaseAttack
{
    private Balance arm, elbow, hand;
    private float targetArmRot, targetElbowRot, targetHandRot;
    private float attackForce, handForce;
    private float initArm, initElbow, initHand;

    public PunchAttack(Balance arm, Balance elbow, Balance hand,
        Balance body, float attackForce, float handForce, float bodyForce,
        float damping, float stiffness, float duration,
        float targetArmRot, float targetElbowRot, float targetHandRot)
        : base(body, bodyForce, damping, stiffness, duration)
    {
        this.arm = arm;
        this.elbow = elbow;
        this.hand = hand;
        this.attackForce = attackForce;
        this.handForce = handForce;
        this.targetArmRot = targetArmRot;
        this.targetElbowRot = targetElbowRot;
        this.targetHandRot = targetHandRot;
    }

    protected override void SaveInitialState()
    {
        initArm = arm.TargetRotation;
        initElbow = elbow.TargetRotation;
        initHand = hand.TargetRotation;
    }

    protected override void ApplyAttack(Vector2 attackDir)
    {
        arm.SetTargetRotation(Mathf.LerpAngle(arm.TargetRotation, targetArmRot, stiffness * Time.deltaTime));
        elbow.SetTargetRotation(Mathf.LerpAngle(elbow.TargetRotation, targetElbowRot, stiffness * Time.deltaTime));
        hand.SetTargetRotation(Mathf.LerpAngle(hand.TargetRotation, targetHandRot, stiffness * Time.deltaTime));

        arm.Rb.linearVelocity = Vector2.Lerp(arm.Rb.linearVelocity, attackDir * attackForce, 0.15f);
        elbow.Rb.linearVelocity = Vector2.Lerp(elbow.Rb.linearVelocity, attackDir * attackForce * 0.8f, 0.15f);
        hand.Rb.linearVelocity = Vector2.Lerp(hand.Rb.linearVelocity, attackDir * handForce, 0.15f);

        body.Rb.linearVelocity = Vector2.Lerp(body.Rb.linearVelocity, attackDir * bodyForce, 0.1f);

        arm.Rb.linearVelocity *= damping;
        elbow.Rb.linearVelocity *= damping;
        hand.Rb.linearVelocity *= damping;
        body.Rb.linearVelocity *= damping;
    }

    protected override void ResetAttack()
    {
        arm.SetTargetRotation(initArm);
        elbow.SetTargetRotation(initElbow);
        hand.SetTargetRotation(initHand);

        arm.Rb.linearVelocity = Vector2.zero;
        elbow.Rb.linearVelocity = Vector2.zero;
        hand.Rb.linearVelocity = Vector2.zero;
        body.Rb.linearVelocity = Vector2.zero;
    }
}
