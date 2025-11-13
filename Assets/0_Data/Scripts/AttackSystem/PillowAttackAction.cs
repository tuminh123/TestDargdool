using System.Collections;
using UnityEngine;

public class PillowAttackAction : IAttackAction
{
    private readonly bool isRight;

    public PillowAttackAction(bool isRight)
    {
        this.isRight = isRight;
    }

    public IEnumerator Execute(AttackContext ctx, AttackProfile profile)
    {
        if (ctx == null || profile == null) yield break;

        var leg = isRight ? ctx.rightLeg : ctx.leftLeg;
        var pillow = isRight ? ctx.rightPillow : ctx.leftPillow;
        var foot = isRight ? ctx.rightFoot : ctx.leftFoot;
        if (leg == null || pillow == null) yield break;

        Vector2 dir = ctx.attackDir;
        float pillowImpulse = profile.pillowImpulse;

        pillow.ApplyImpulse(dir, pillowImpulse);
        leg.ApplyImpulse(dir, pillowImpulse * 0.6f);
        pillow.ModifyGravityTemporarily(0.5f, profile.attackDuration);

        float elapsed = 0f;
        while (elapsed < profile.attackDuration)
        {
            elapsed += Time.deltaTime;
            float s = profile.stiffness * Time.deltaTime;

            float legTarget = isRight ? profile.pillowLegExt : -profile.pillowLegExt;
            float pillowTarget = isRight ? profile.pillowPillowExt : -profile.pillowPillowExt;

            leg.SetTargetRotation(Mathf.LerpAngle(leg.TargetRotation, legTarget, s));
            pillow.SetTargetRotation(Mathf.LerpAngle(pillow.TargetRotation, pillowTarget, s));

            if (foot != null)
            {
                float perp = (isRight ? pillowTarget : -pillowTarget) + (isRight ? 90f : -90f);
                foot.SetTargetRotation(Mathf.LerpAngle(foot.TargetRotation, perp, s));
            }

            leg.Rb.linearVelocity = Vector2.Lerp(leg.Rb.linearVelocity, dir * pillowImpulse * 0.7f, 0.14f);
            pillow.Rb.linearVelocity = Vector2.Lerp(pillow.Rb.linearVelocity, dir * pillowImpulse, 0.16f);
            if (foot != null) foot.Rb.linearVelocity = Vector2.Lerp(foot.Rb.linearVelocity, dir * pillowImpulse * 0.6f, 0.14f);

            leg.Rb.linearVelocity *= profile.damping;
            pillow.Rb.linearVelocity *= profile.damping;
            if (foot != null) foot.Rb.linearVelocity *= profile.damping;

            yield return null;
        }

        pillow.LerpToActive(0.12f);
        leg.LerpToActive(0.12f);
        if (foot != null) foot.LerpToActive(0.12f);
    }
}