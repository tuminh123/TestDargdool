using System.Collections;
using UnityEngine;

public class Attack_Test_3 : ActionBase
{
    private enum AttackType
    {
        RightPunch,
        LeftPunch,
        RightKick,
        LeftKick,
        RightElbow,
        LeftElbow,
        RightPillow,
        LeftPillow
    }

    [Header("Attack Settings")]
    [SerializeField] private float attackForce = 100f;        // Lực tay
    [SerializeField] private float handForce = 100f;          // Lực bàn tay
    [SerializeField] private float legForce = 150f;           // Lực đá
    [SerializeField] private float pillowForce = 120f;        // Lực gối
    [SerializeField] private float bodyForce = 70f;           // Lực thân nhẹ
    [SerializeField] private float damping = 1.5f;            // Giảm tốc độ
    [SerializeField] private float stiffness = 50f;           // Độ đàn hồi tay
    [SerializeField] private float attackDuration = 0.25f;    // Thời gian cú tấn công

    [Header("References - Right side")]
    [SerializeField] private RightArmBalance rightArmBalance;
    [SerializeField] private RightElbowBalance rightElbowBalance;
    [SerializeField] private RightHandBalance rightHandBalance; // Bàn tay
    [SerializeField] private RightLegBalance rightLegBalance;
    [SerializeField] private RightFootBalance rightFootBalance;
    [SerializeField] private RightPillowBalance rightPillowBalance; // đầu gối / cẳng chân

    [Header("References - Left side")]
    [SerializeField] private LeftArmBalance leftArmBalance;
    [SerializeField] private LeftElbowBalance leftElbowBalance;
    [SerializeField] private LeftHandBalance leftHandBalance;
    [SerializeField] private LeftLegBalance leftLegBalance;
    [SerializeField] private LeftFootBalance leftFootBalance;
    [SerializeField] private LeftPillowBalance leftPillowBalance; // đầu gối / cẳng chân

    private BodyBalance body;
    private Vector2 attackDir;
    private bool isAttacking;

    // Lưu trạng thái ban đầu
    private float armInitialRot;
    private float elbowInitialRot;
    private float handInitialRot;
    private float leftArmInitialRot;
    private float leftElbowInitialRot;
    private float leftHandInitialRot;
    private float rightLegInitialRot;
    private float rightFootInitialRot;
    private float rightPillowInitialRot;
    private float leftLegInitialRot;
    private float leftFootInitialRot;
    private float leftPillowInitialRot;

    protected override void Awake()
    {
        base.Awake();
        body = GetComponentInChildren<BodyBalance>();

        SetBeginBody();
    }

    private void SetBeginBody()
    {
        // Lưu trạng thái ban đầu (null-safe)
        if (rightArmBalance != null) armInitialRot = rightArmBalance.TargetRotation;
        if (rightElbowBalance != null) elbowInitialRot = rightElbowBalance.TargetRotation;
        if (rightHandBalance != null) handInitialRot = rightHandBalance.TargetRotation;

        if (leftArmBalance != null) leftArmInitialRot = leftArmBalance.TargetRotation;
        if (leftElbowBalance != null) leftElbowInitialRot = leftElbowBalance.TargetRotation;
        if (leftHandBalance != null) leftHandInitialRot = leftHandBalance.TargetRotation;

        if (rightLegBalance != null) rightLegInitialRot = rightLegBalance.TargetRotation;
        if (rightFootBalance != null) rightFootInitialRot = rightFootBalance.TargetRotation;
        if (rightPillowBalance != null) rightPillowInitialRot = rightPillowBalance.TargetRotation;

        if (leftLegBalance != null) leftLegInitialRot = leftLegBalance.TargetRotation;
        if (leftFootBalance != null) leftFootInitialRot = leftFootBalance.TargetRotation;
        if (leftPillowBalance != null) leftPillowInitialRot = leftPillowBalance.TargetRotation;
    }

    private void Update()
    {
        bool flowControl = SetAttackDir();
        if (!flowControl)
        {
            return;
        }
    }

    private bool SetAttackDir()
    {
        if (Camera.main == null || body == null) return false;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        attackDir = (mouseWorld - body.transform.position).normalized;

        Debug.DrawLine(body.transform.position, mouseWorld, Color.blue);
        return true;
    }

    public void AttackHandle()
    {
        if (isAttacking) return;
        isAttacking = true;

        // Choose side based on attackDir.x:
        // - attackDir.x >= 0  => perform a RIGHT-side attack
        // - attackDir.x <  0  => perform a LEFT-side attack
        AttackType chosen;
        if (attackDir.x >= 0f)
        {
            var rightOptions = new AttackType[] { AttackType.RightPunch, AttackType.RightKick, AttackType.RightElbow, AttackType.RightPillow };
            chosen = rightOptions[Random.Range(0, rightOptions.Length)];
        }
        else
        {
            var leftOptions = new AttackType[] { AttackType.LeftPunch, AttackType.LeftKick, AttackType.LeftElbow, AttackType.LeftPillow };
            chosen = leftOptions[Random.Range(0, leftOptions.Length)];
        }

        StartCoroutine(DoAttack(chosen));
    }

    private IEnumerator DoAttack(AttackType type)
    {
        float elapsed = 0f;

        float targetArmRot, targetElbowRot, targetHandRot;
        float targetLegRot, targetFootRot, targetPillowRot;
        float armImpulse, handImpulse, legImpulse, pillowImpulse, bodyImpulse;
        bool affectRightArm, affectLeftArm, affectRightLeg, affectLeftLeg;
        bool affectRightPillow, affectLeftPillow;
        Vector2 effectiveAttackDir; // Hướng thực tế cho tấn công (luôn là attackDir)

        ResetValue(out targetArmRot, out targetElbowRot, out targetHandRot,
                   out targetLegRot, out targetFootRot, out targetPillowRot,
                   out armImpulse, out handImpulse, out legImpulse, out pillowImpulse, out bodyImpulse,
                   out affectRightArm, out affectLeftArm, out affectRightLeg, out affectLeftLeg,
                   out affectRightPillow, out affectLeftPillow);

        BodyAttackSystem(type,
            ref targetArmRot, ref targetElbowRot, ref targetHandRot,
            ref targetLegRot, ref targetFootRot, ref targetPillowRot,
            ref armImpulse, ref handImpulse, ref legImpulse, ref pillowImpulse, ref bodyImpulse,
            ref affectRightArm, ref affectLeftArm, ref affectRightLeg, ref affectLeftLeg,
            ref affectRightPillow, ref affectLeftPillow,
            out effectiveAttackDir);

        ApplySmoothAndGravity(armImpulse, handImpulse, legImpulse, pillowImpulse, bodyImpulse, affectRightArm, affectLeftArm, affectRightLeg, affectLeftLeg, affectRightPillow, affectLeftPillow, effectiveAttackDir);

        // Main lerp loop to animate target rotations and softly shape velocities
        while (elapsed < attackDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / attackDuration;

            // arms
            if (affectRightArm && rightArmBalance != null)
            {
                rightArmBalance.SetTargetRotation(Mathf.LerpAngle(rightArmBalance.TargetRotation, targetArmRot, stiffness * Time.deltaTime));
                if (rightElbowBalance != null) rightElbowBalance.SetTargetRotation(Mathf.LerpAngle(rightElbowBalance.TargetRotation, targetElbowRot, stiffness * Time.deltaTime));
                if (rightHandBalance != null) rightHandBalance.SetTargetRotation(Mathf.LerpAngle(rightHandBalance.TargetRotation, targetHandRot, stiffness * Time.deltaTime));

                rightArmBalance.Rb.linearVelocity = Vector2.Lerp(rightArmBalance.Rb.linearVelocity, effectiveAttackDir * armImpulse, 0.15f);
                if (rightHandBalance != null) rightHandBalance.Rb.linearVelocity = Vector2.Lerp(rightHandBalance.Rb.linearVelocity, effectiveAttackDir * handImpulse, 0.15f);
            }

            if (affectLeftArm && leftArmBalance != null)
            {
                leftArmBalance.SetTargetRotation(Mathf.LerpAngle(leftArmBalance.TargetRotation, targetArmRot, stiffness * Time.deltaTime));
                if (leftElbowBalance != null) leftElbowBalance.SetTargetRotation(Mathf.LerpAngle(leftElbowBalance.TargetRotation, targetElbowRot, stiffness * Time.deltaTime));
                if (leftHandBalance != null) leftHandBalance.SetTargetRotation(Mathf.LerpAngle(leftHandBalance.TargetRotation, targetHandRot, stiffness * Time.deltaTime));

                leftArmBalance.Rb.linearVelocity = Vector2.Lerp(leftArmBalance.Rb.linearVelocity, effectiveAttackDir * armImpulse, 0.15f);
                if (leftHandBalance != null) leftHandBalance.Rb.linearVelocity = Vector2.Lerp(leftHandBalance.Rb.linearVelocity, effectiveAttackDir * handImpulse, 0.15f);
            }

            // legs - ensure knee (pillow) follows same linear path as leg; foot kept perpendicular during extension
            if ((affectRightLeg || affectRightPillow) && rightLegBalance != null)
            {
                rightLegBalance.SetTargetRotation(Mathf.LerpAngle(rightLegBalance.TargetRotation, targetLegRot, stiffness * Time.deltaTime));

                if (affectRightPillow && rightPillowBalance != null)
                {
                    rightPillowBalance.SetTargetRotation(Mathf.LerpAngle(rightPillowBalance.TargetRotation, targetPillowRot, stiffness * Time.deltaTime));
                    if (rightFootBalance != null)
                    {
                        float perp = targetPillowRot + 90f;
                        rightFootBalance.SetTargetRotation(Mathf.LerpAngle(rightFootBalance.TargetRotation, perp, stiffness * Time.deltaTime));
                    }
                }
                else
                {
                    if (rightFootBalance != null) rightFootBalance.SetTargetRotation(Mathf.LerpAngle(rightFootBalance.TargetRotation, targetFootRot, stiffness * Time.deltaTime));
                }

                rightLegBalance.Rb.linearVelocity = Vector2.Lerp(rightLegBalance.Rb.linearVelocity, effectiveAttackDir * (affectRightPillow ? pillowImpulse : legImpulse), 0.12f);
                if (rightFootBalance != null) rightFootBalance.Rb.linearVelocity = Vector2.Lerp(rightFootBalance.Rb.linearVelocity, effectiveAttackDir * ((affectRightPillow && rightPillowBalance != null) ? pillowImpulse * 0.6f : legImpulse * 0.6f), 0.12f);
                if (affectRightPillow && rightPillowBalance != null) rightPillowBalance.Rb.linearVelocity = Vector2.Lerp(rightPillowBalance.Rb.linearVelocity, effectiveAttackDir * pillowImpulse, 0.14f);
            }

            if ((affectLeftLeg || affectLeftPillow) && leftLegBalance != null)
            {
                leftLegBalance.SetTargetRotation(Mathf.LerpAngle(leftLegBalance.TargetRotation, targetLegRot, stiffness * Time.deltaTime));

                if (affectLeftPillow && leftPillowBalance != null)
                {
                    leftPillowBalance.SetTargetRotation(Mathf.LerpAngle(leftPillowBalance.TargetRotation, targetPillowRot, stiffness * Time.deltaTime));
                    if (leftFootBalance != null)
                    {
                        float perp = targetPillowRot - 90f;
                        leftFootBalance.SetTargetRotation(Mathf.LerpAngle(leftFootBalance.TargetRotation, perp, stiffness * Time.deltaTime));
                    }
                }
                else
                {
                    if (leftFootBalance != null) leftFootBalance.SetTargetRotation(Mathf.LerpAngle(leftFootBalance.TargetRotation, targetFootRot, stiffness * Time.deltaTime));
                }

                leftLegBalance.Rb.linearVelocity = Vector2.Lerp(leftLegBalance.Rb.linearVelocity, effectiveAttackDir * (affectLeftPillow ? pillowImpulse : legImpulse), 0.12f);
                if (leftFootBalance != null) leftFootBalance.Rb.linearVelocity = Vector2.Lerp(leftFootBalance.Rb.linearVelocity, effectiveAttackDir * ((affectLeftPillow && leftPillowBalance != null) ? pillowImpulse * 0.6f : legImpulse * 0.6f), 0.12f);
                if (affectLeftPillow && leftPillowBalance != null) leftPillowBalance.Rb.linearVelocity = Vector2.Lerp(leftPillowBalance.Rb.linearVelocity, effectiveAttackDir * pillowImpulse, 0.14f);
            }

            BodyHandle(bodyImpulse, effectiveAttackDir);

            yield return null;
        }

        ResetAfterAttack();

        isAttacking = false;
        characterCtrl.ChangeState(state.idle);
    }

    private void ApplySmoothAndGravity(float armImpulse, float handImpulse, float legImpulse, float pillowImpulse, float bodyImpulse, bool affectRightArm, bool affectLeftArm, bool affectRightLeg, bool affectLeftLeg, bool affectRightPillow, bool affectLeftPillow, Vector2 effectiveAttackDir)
    {
        // Apply initial impulses (one-shot) using BalanceAbstract.ApplyImpulse
        if (affectRightArm && rightArmBalance != null)
        {
            rightArmBalance.ApplyImpulse(effectiveAttackDir, armImpulse);
            if (rightHandBalance != null) rightHandBalance.ApplyImpulse(effectiveAttackDir, handImpulse);
            rightArmBalance.ModifyGravityTemporarily(0.5f, attackDuration);
        }
        if (affectLeftArm && leftArmBalance != null)
        {
            leftArmBalance.ApplyImpulse(effectiveAttackDir, armImpulse);
            if (leftHandBalance != null) leftHandBalance.ApplyImpulse(effectiveAttackDir, handImpulse);
            leftArmBalance.ModifyGravityTemporarily(0.5f, attackDuration);
        }
        if (affectRightLeg && rightLegBalance != null)
        {
            rightLegBalance.ApplyImpulse(effectiveAttackDir, legImpulse);
            if (rightFootBalance != null) rightFootBalance.ApplyImpulse(effectiveAttackDir, legImpulse * 0.6f);
            rightLegBalance.ModifyGravityTemporarily(0.6f, attackDuration);
        }
        if (affectLeftLeg && leftLegBalance != null)
        {
            leftLegBalance.ApplyImpulse(effectiveAttackDir, legImpulse);
            if (leftFootBalance != null) leftFootBalance.ApplyImpulse(effectiveAttackDir, legImpulse * 0.6f);
            leftLegBalance.ModifyGravityTemporarily(0.6f, attackDuration);
        }

        // pillow-specific impulses (knee / shin)
        if (affectRightPillow && rightPillowBalance != null)
        {
            rightPillowBalance.ApplyImpulse(effectiveAttackDir, pillowImpulse);
            rightPillowBalance.ModifyGravityTemporarily(0.5f, attackDuration);
            if (rightLegBalance != null) rightLegBalance.ApplyImpulse(effectiveAttackDir, pillowImpulse * 0.6f);
        }
        if (affectLeftPillow && leftPillowBalance != null)
        {
            leftPillowBalance.ApplyImpulse(effectiveAttackDir, pillowImpulse);
            leftPillowBalance.ModifyGravityTemporarily(0.5f, attackDuration);
            if (leftLegBalance != null) leftLegBalance.ApplyImpulse(effectiveAttackDir, pillowImpulse * 0.6f);
        }

        // body impulse (one-shot)
        if (body != null)
        {
            body.Rb.AddForce(effectiveAttackDir * bodyImpulse, ForceMode2D.Impulse);
            body.ModifyGravityTemporarily(0.9f, attackDuration);
        }
    }

    private void ResetAfterAttack()
    {
        // Restore initial rotations for all affected parts
        if (rightArmBalance != null) rightArmBalance.SetTargetRotation(armInitialRot);
        if (rightElbowBalance != null) rightElbowBalance.SetTargetRotation(elbowInitialRot);
        if (rightHandBalance != null) rightHandBalance.SetTargetRotation(handInitialRot);

        if (leftArmBalance != null) leftArmBalance.SetTargetRotation(leftArmInitialRot);
        if (leftElbowBalance != null) leftElbowBalance.SetTargetRotation(leftElbowInitialRot);
        if (leftHandBalance != null) leftHandBalance.SetTargetRotation(leftHandInitialRot);

        if (rightLegBalance != null) rightLegBalance.SetTargetRotation(rightLegInitialRot);
        if (rightFootBalance != null) rightFootBalance.SetTargetRotation(rightFootInitialRot);
        if (rightPillowBalance != null) rightPillowBalance.SetTargetRotation(rightPillowInitialRot);

        if (leftLegBalance != null) leftLegBalance.SetTargetRotation(leftLegInitialRot);
        if (leftFootBalance != null) leftFootBalance.SetTargetRotation(leftFootInitialRot);
        if (leftPillowBalance != null) leftPillowBalance.SetTargetRotation(leftPillowInitialRot);

        // Softly zero velocities using BalanceAbstract.LerpToActive where available
        if (rightArmBalance != null) rightArmBalance.LerpToActive(0.12f);
        if (rightElbowBalance != null) rightElbowBalance.LerpToActive(0.12f);
        if (rightHandBalance != null) rightHandBalance.LerpToActive(0.12f);

        if (leftArmBalance != null) leftArmBalance.LerpToActive(0.12f);
        if (leftElbowBalance != null) leftElbowBalance.LerpToActive(0.12f);
        if (leftHandBalance != null) leftHandBalance.LerpToActive(0.12f);

        if (rightLegBalance != null) rightLegBalance.LerpToActive(0.12f);
        if (rightFootBalance != null) rightFootBalance.LerpToActive(0.12f);
        if (rightPillowBalance != null) rightPillowBalance.LerpToActive(0.12f);

        if (leftLegBalance != null) leftLegBalance.LerpToActive(0.12f);
        if (leftFootBalance != null) leftFootBalance.LerpToActive(0.12f);
        if (leftPillowBalance != null) leftPillowBalance.LerpToActive(0.12f);

        if (body != null) body.Rb.linearVelocity = Vector2.zero;
    }

    private void BodyHandle(float bodyImpulse, Vector2 effectiveAttackDir)
    {
        // body follow
        if (body != null) body.Rb.linearVelocity = Vector2.Lerp(body.Rb.linearVelocity, effectiveAttackDir * bodyImpulse, 0.1f);

        // damping
        if (body != null) body.Rb.linearVelocity *= damping;
        if (rightArmBalance != null) rightArmBalance.Rb.linearVelocity *= damping;
        if (rightElbowBalance != null) rightElbowBalance.Rb.linearVelocity *= damping;
        if (rightHandBalance != null) rightHandBalance.Rb.linearVelocity *= damping;
        if (leftArmBalance != null) leftArmBalance.Rb.linearVelocity *= damping;
        if (leftElbowBalance != null) leftElbowBalance.Rb.linearVelocity *= damping;
        if (leftHandBalance != null) leftHandBalance.Rb.linearVelocity *= damping;
        if (rightLegBalance != null) rightLegBalance.Rb.linearVelocity *= damping;
        if (rightFootBalance != null) rightFootBalance.Rb.linearVelocity *= damping;
        if (leftLegBalance != null) leftLegBalance.Rb.linearVelocity *= damping;
        if (leftFootBalance != null) leftFootBalance.Rb.linearVelocity *= damping;
        if (rightPillowBalance != null) rightPillowBalance.Rb.linearVelocity *= damping;
        if (leftPillowBalance != null) leftPillowBalance.Rb.linearVelocity *= damping;
    }

    private void BodyAttackSystem(AttackType type,
        ref float targetArmRot, ref float targetElbowRot, ref float targetHandRot,
        ref float targetLegRot, ref float targetFootRot, ref float targetPillowRot,
        ref float armImpulse, ref float handImpulse, ref float legImpulse, ref float pillowImpulse, ref float bodyImpulse,
        ref bool affectRightArm, ref bool affectLeftArm, ref bool affectRightLeg, ref bool affectLeftLeg,
        ref bool affectRightPillow, ref bool affectLeftPillow,
        out Vector2 effectiveAttackDir)
    {
        // effectiveAttackDir always follows current attackDir sign;
        // left/right selection already done in AttackHandle.
        effectiveAttackDir = attackDir;

        // Configure attack specifics
        switch (type)
        {
            case AttackType.RightPunch:
                affectRightArm = true;
                targetArmRot = 115f;
                targetElbowRot = 85f;
                targetHandRot = 45f;
                armImpulse = attackForce;
                handImpulse = handForce;
                bodyImpulse = bodyForce;
                break;

            case AttackType.LeftPunch:
                affectLeftArm = true;
                targetArmRot = -115f;
                targetElbowRot = -85f;
                targetHandRot = -45f;
                armImpulse = attackForce;
                handImpulse = handForce;
                bodyImpulse = bodyForce;
                break;

            case AttackType.RightKick:
                affectRightLeg = true;
                targetLegRot = 40f;
                targetFootRot = 25f;
                legImpulse = legForce;
                bodyImpulse = bodyForce * 1.5f;
                break;

            case AttackType.LeftKick:
                affectLeftLeg = true;
                targetLegRot = -40f;
                targetFootRot = -25f;
                legImpulse = legForce;
                bodyImpulse = bodyForce * 1.5f;
                break;

            case AttackType.RightElbow:
                affectRightArm = true;
                targetArmRot = 100f;
                targetElbowRot = 120f;
                armImpulse = attackForce * 0.8f;
                bodyImpulse = bodyForce * 0.8f;
                break;

            case AttackType.LeftElbow:
                affectLeftArm = true;
                targetArmRot = -100f;
                targetElbowRot = -120f;
                armImpulse = attackForce * 0.8f;
                bodyImpulse = bodyForce * 0.8f;
                break;

            case AttackType.RightPillow:
                affectRightPillow = true;
                targetLegRot = 30f;
                targetPillowRot = 30f;
                pillowImpulse = pillowForce;
                bodyImpulse = bodyForce * 1.2f;
                break;

            case AttackType.LeftPillow:
                affectLeftPillow = true;
                targetLegRot = -30f;
                targetPillowRot = -30f;
                pillowImpulse = pillowForce;
                bodyImpulse = bodyForce * 1.2f;
                break;
        }
    }

    private void ResetValue(out float targetArmRot, out float targetElbowRot, out float targetHandRot,
        out float targetLegRot, out float targetFootRot, out float targetPillowRot,
        out float armImpulse, out float handImpulse, out float legImpulse, out float pillowImpulse, out float bodyImpulse,
        out bool affectRightArm, out bool affectLeftArm, out bool affectRightLeg, out bool affectLeftLeg,
        out bool affectRightPillow, out bool affectLeftPillow)
    {
        // target rotations depending on attack
        targetArmRot = 0f;
        targetElbowRot = 0f;
        targetHandRot = 0f;
        targetLegRot = 0f;
        targetFootRot = 0f;
        targetPillowRot = 0f;

        // multipliers for impulses
        armImpulse = 0f;
        handImpulse = 0f;
        legImpulse = 0f;
        pillowImpulse = 0f;
        bodyImpulse = bodyForce;

        // Which parts to affect
        affectRightArm = false;
        affectLeftArm = false;
        affectRightLeg = false;
        affectLeftLeg = false;
        affectRightPillow = false;
        affectLeftPillow = false;
    }
}
