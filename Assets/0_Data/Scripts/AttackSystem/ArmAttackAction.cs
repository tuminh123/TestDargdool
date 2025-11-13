using System.Collections;
using UnityEngine;

public class ArmAttackAction : IAttackAction
{
    private readonly bool isRight;

    public ArmAttackAction(bool isRight)
    {
        this.isRight = isRight;
    }

    public IEnumerator Execute(AttackContext ctx, AttackProfile profile)
    {
        if (ctx == null || profile == null) yield break;

        var arm = isRight ? ctx.rightArm : ctx.leftArm;
        var elbow = isRight ? ctx.rightElbow : ctx.leftElbow;
        var hand = isRight ? ctx.rightHand : ctx.leftHand;
        if (arm == null) yield break;

        Vector2 dir = ctx.attackDir;
        float armImpulse = profile.armImpulse;
        float handImpulse = profile.handImpulse;

        arm.ApplyImpulse(dir, armImpulse);
        if (hand != null) hand.ApplyImpulse(dir, handImpulse);
        arm.ModifyGravityTemporarily(0.5f, profile.attackDuration);

        float elapsed = 0f;
        while (elapsed < profile.attackDuration)
        {
            elapsed += Time.deltaTime;
            float s = profile.stiffness * Time.deltaTime;
            float armTarget = isRight ? profile.armExt : -profile.armExt;
            float elbowTarget = isRight ? profile.elbowExt : -profile.elbowExt;
            float handTarget = isRight ? profile.handExt : -profile.handExt;

            arm.SetTargetRotation(Mathf.LerpAngle(arm.TargetRotation, armTarget, s));
            if (elbow != null) elbow.SetTargetRotation(Mathf.LerpAngle(elbow.TargetRotation, elbowTarget, s));
            if (hand != null) hand.SetTargetRotation(Mathf.LerpAngle(hand.TargetRotation, handTarget, s));

            arm.Rb.linearVelocity = Vector2.Lerp(arm.Rb.linearVelocity, dir * armImpulse, 0.15f);
            if (hand != null) hand.Rb.linearVelocity = Vector2.Lerp(hand.Rb.linearVelocity, dir * handImpulse, 0.15f);

            arm.Rb.linearVelocity *= profile.damping;
            if (elbow != null) elbow.Rb.linearVelocity *= profile.damping;
            if (hand != null) hand.Rb.linearVelocity *= profile.damping;

            yield return null;
        }

        arm.LerpToActive(0.12f);
        if (elbow != null) elbow.LerpToActive(0.12f);
        if (hand != null) hand.LerpToActive(0.12f);
    }
}