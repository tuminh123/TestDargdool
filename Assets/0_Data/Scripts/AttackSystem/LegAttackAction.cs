using System.Collections;
using UnityEngine;

public class LegAttackAction : IAttackAction
{
    private readonly bool isRight;

    public LegAttackAction(bool isRight)
    {
        this.isRight = isRight;
    }

    public IEnumerator Execute(AttackContext ctx, AttackProfile profile)
    {
        if (ctx == null || profile == null) yield break;

        var leg = isRight ? ctx.rightLeg : ctx.leftLeg;
        var foot = isRight ? ctx.rightFoot : ctx.leftFoot;
        if (leg == null) yield break;

        Vector2 dir = ctx.attackDir;
        float legImpulse = profile.legImpulse;

        leg.ApplyImpulse(dir, legImpulse);
        if (foot != null) foot.ApplyImpulse(dir, legImpulse * 0.6f);
        leg.ModifyGravityTemporarily(0.6f, profile.attackDuration);

        float elapsed = 0f;
        while (elapsed < profile.attackDuration)
        {
            elapsed += Time.deltaTime;
            float s = profile.stiffness * Time.deltaTime;

            float legTarget = isRight ? profile.legExt : -profile.legExt;
            float footTarget = isRight ? profile.footExt : -profile.footExt;

            leg.SetTargetRotation(Mathf.LerpAngle(leg.TargetRotation, legTarget, s));
            if (foot != null) foot.SetTargetRotation(Mathf.LerpAngle(foot.TargetRotation, footTarget, s));

            leg.Rb.linearVelocity = Vector2.Lerp(leg.Rb.linearVelocity, dir * legImpulse, 0.14f);
            if (foot != null) foot.Rb.linearVelocity = Vector2.Lerp(foot.Rb.linearVelocity, dir * legImpulse * 0.6f, 0.14f);

            leg.Rb.linearVelocity *= profile.damping;
            if (foot != null) foot.Rb.linearVelocity *= profile.damping;

            yield return null;
        }

        leg.LerpToActive(0.12f);
        if (foot != null) foot.LerpToActive(0.12f);
    }
}