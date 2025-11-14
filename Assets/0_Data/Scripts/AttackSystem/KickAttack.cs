using UnityEngine;

public class KickAttack : BaseAttack
{
    private Balance leg, foot;
    private float targetLegRot, targetFootRot;
    private float kickForce;
    private float initLeg, initFoot;

    public KickAttack(Balance leg, Balance foot,
        Balance body, float targetLegRot, float targetFootRot,
        float kickForce, float bodyForce,
        float damping, float stiffness, float duration)
        : base(body, bodyForce, damping, stiffness, duration)
    {
        this.leg = leg;
        this.foot = foot;
        this.targetLegRot = targetLegRot;
        this.targetFootRot = targetFootRot;
        this.kickForce = kickForce;
    }

    protected override void SaveInitialState()
    {
        initLeg = leg.TargetRotation;
        initFoot = foot.TargetRotation;
    }

    protected override void ApplyAttack(Vector2 attackDir)
    {
        leg.SetTargetRotation(Mathf.LerpAngle(leg.TargetRotation, targetLegRot, stiffness * Time.deltaTime));
        foot.SetTargetRotation(Mathf.LerpAngle(foot.TargetRotation, targetFootRot, stiffness * Time.deltaTime));

        leg.Rb.linearVelocity = Vector2.Lerp(leg.Rb.linearVelocity, attackDir * kickForce, 0.15f);
        foot.Rb.linearVelocity = Vector2.Lerp(foot.Rb.linearVelocity, attackDir * kickForce * 0.6f, 0.15f);

        body.Rb.linearVelocity = Vector2.Lerp(body.Rb.linearVelocity, attackDir * bodyForce, 0.1f);

        leg.Rb.linearVelocity *= damping;
        foot.Rb.linearVelocity *= damping;
        body.Rb.linearVelocity *= damping;
    }

    protected override void ResetAttack()
    {
        leg.SetTargetRotation(initLeg);
        foot.SetTargetRotation(initFoot);

        leg.Rb.linearVelocity = Vector2.zero;
        foot.Rb.linearVelocity = Vector2.zero;
        body.Rb.linearVelocity = Vector2.zero;
    }
}
