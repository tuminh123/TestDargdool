using System.Collections;
using UnityEngine;

public class AttackTest_2 : ActionBase
{
    [Header("Attack Settings")]
    [SerializeField] private float attackForce = 15f;        // Lực tay
    [SerializeField] private float handForce = 15f;          // Lực bàn tay
    [SerializeField] private float bodyForce = 2f;           // Lực thân nhẹ
    [SerializeField] private float damping = 0.9f;           // Giảm tốc độ
    [SerializeField] private float stiffness = 8f;           // Độ đàn hồi tay
    [SerializeField] private float attackDuration = 0.25f;   // Thời gian cú đấm

    [Header("References")]
    [SerializeField] private RightArmBalance rightArmBalance;
    [SerializeField] private RightElbowBalance rightElbowBalance;
    [SerializeField] private RightHandBalance rightHandBalance; // Bàn tay

    private BodyBalance body;
    private Vector2 attackDir;
    private bool isAttacking;

    // Lưu trạng thái ban đầu
    private float armInitialRot;
    private float elbowInitialRot;
    private float handInitialRot;

    protected override void Awake()
    {
        base.Awake();
        body = GetComponentInChildren<BodyBalance>();

        // Lưu trạng thái ban đầu của tay
        armInitialRot = rightArmBalance.TargetRotation;
        elbowInitialRot = rightElbowBalance.TargetRotation;
        handInitialRot = rightHandBalance.TargetRotation;
    }

    private void Update()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        attackDir = (mouseWorld - body.transform.position).normalized;

        Debug.DrawLine(body.transform.position, mouseWorld, Color.blue);
    }

    public void AttackHandle()
    {
        if (isAttacking) return;
        isAttacking = true;

        StartCoroutine(DoPunch());
    }

    private IEnumerator DoPunch()
    {
        float elapsed = 0f;

        // Góc mục tiêu khi đấm
        float targetArmRot = 115f;
        float targetElbowRot = 85f;
        float targetHandRot = 45f;

        while (elapsed < attackDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / attackDuration;

            // 1. Di chuyển tay, cùi trỏ, bàn tay theo đường thẳng
            rightArmBalance.SetTargetRotation(Mathf.LerpAngle(rightArmBalance.TargetRotation, targetArmRot, stiffness * Time.deltaTime));
            rightElbowBalance.SetTargetRotation(Mathf.LerpAngle(rightElbowBalance.TargetRotation, targetElbowRot, stiffness * Time.deltaTime));
            rightHandBalance.SetTargetRotation(Mathf.LerpAngle(rightHandBalance.TargetRotation, targetHandRot, stiffness * Time.deltaTime));

            // 2. Thêm lực bouncy cho tay và bàn tay
            rightArmBalance.Rb.linearVelocity = Vector2.Lerp(rightArmBalance.Rb.linearVelocity, attackDir * attackForce, 0.15f);
            rightElbowBalance.Rb.linearVelocity = Vector2.Lerp(rightElbowBalance.Rb.linearVelocity, attackDir * attackForce * 0.8f, 0.15f);
            rightHandBalance.Rb.linearVelocity = Vector2.Lerp(rightHandBalance.Rb.linearVelocity, attackDir * handForce, 0.15f);

            // 3. Thân người nhẹ nhàng lao theo cú đấm
            body.Rb.linearVelocity = Vector2.Lerp(body.Rb.linearVelocity, attackDir * bodyForce, 0.1f);

            // 4. Giảm tốc lực tổng thể để bouncy
            body.Rb.linearVelocity *= damping;
            rightArmBalance.Rb.linearVelocity *= damping;
            rightElbowBalance.Rb.linearVelocity *= damping;
            rightHandBalance.Rb.linearVelocity *= damping;

            yield return null;
        }

        // 5. Quay lại trạng thái ban đầu
        rightArmBalance.SetTargetRotation(armInitialRot);
        rightElbowBalance.SetTargetRotation(elbowInitialRot);
        rightHandBalance.SetTargetRotation(handInitialRot);

        // Reset linearVelocity
        rightArmBalance.Rb.linearVelocity = Vector2.zero;
        rightElbowBalance.Rb.linearVelocity = Vector2.zero;
        rightHandBalance.Rb.linearVelocity = Vector2.zero;
        body.Rb.linearVelocity = Vector2.zero;

        isAttacking = false;
        characterCtrl.ChangeState(state.idle);
    }
}
#region Test
//using System.Collections;
//using UnityEngine;

//public class AttackTest_2 : ActionBase
//{
//    private enum AttackType
//    {
//        RightPunch,
//        LeftPunch,
//        RightKick,
//        LeftKick,
//        RightElbow,
//        LeftElbow,
//        RightPillow,
//        LeftPillow
//    }

//    [Header("Attack Settings")]
//    [SerializeField] private float attackForce = 15f;        // Lực tay
//    [SerializeField] private float handForce = 15f;          // Lực bàn tay
//    [SerializeField] private float legForce = 20f;           // Lực đá
//    [SerializeField] private float pillowForce = 18f;        // Lực gối
//    [SerializeField] private float bodyForce = 2f;           // Lực thân
//    [SerializeField] private float damping = 0.9f;           // Giảm tốc độ
//    [SerializeField] private float stiffness = 8f;           // Độ đàn hồi
//    [SerializeField] private float attackDuration = 0.25f;   // Thời gian tấn công
//    [SerializeField] private float recoveryDuration = 0.15f; // Thời gian hồi phục sau tấn công

//    [Header("Extension / Retract Angles - Punch")]
//    [SerializeField] private float punchArmExt = 115f;
//    [SerializeField] private float punchElbowExt = 85f;
//    [SerializeField] private float punchHandExt = 45f;
//    [SerializeField] private float punchArmRetract = 45f;
//    [SerializeField] private float punchElbowRetract = -30f;

//    [Header("Extension / Retract Angles - Elbow")]
//    [SerializeField] private float elbowArmExt = 100f;
//    [SerializeField] private float elbowElbowExt = 120f;
//    [SerializeField] private float elbowArmRetract = 30f;
//    [SerializeField] private float elbowElbowRetract = -60f;

//    [Header("Extension / Retract Angles - Kick")]
//    [SerializeField] private float kickLegExt = 40f;
//    [SerializeField] private float kickFootExt = 25f;
//    [SerializeField] private float kickLegRetract = 90f;
//    [SerializeField] private float kickFootRetract = 60f;

//    [Header("Extension / Retract Angles - Pillow")]
//    [SerializeField] private float pillowLegExt = 50f;
//    [SerializeField] private float pillowPillowExt = 35f;
//    [SerializeField] private float pillowLegRetract = 100f;
//    [SerializeField] private float pillowPillowRetract = 80f;

//    [Header("References - Right side")]
//    [SerializeField] private RightArmBalance rightArmBalance;
//    [SerializeField] private RightElbowBalance rightElbowBalance;
//    [SerializeField] private RightHandBalance rightHandBalance;
//    [SerializeField] private RightLegBalance rightLegBalance;
//    [SerializeField] private RightFootBalance rightFootBalance;
//    [SerializeField] private RightPillowBalance rightPillowBalance;

//    [Header("References - Left side")]
//    [SerializeField] private LeftArmBalance leftArmBalance;
//    [SerializeField] private LeftElbowBalance leftElbowBalance;
//    [SerializeField] private LeftHandBalance leftHandBalance;
//    [SerializeField] private LeftLegBalance leftLegBalance;
//    [SerializeField] private LeftFootBalance leftFootBalance;
//    [SerializeField] private LeftPillowBalance leftPillowBalance;

//    private BodyBalance body;
//    private Vector2 attackDir;
//    private bool isAttacking;

//    // Lưu trạng thái ban đầu
//    private float armInitialRot;
//    private float elbowInitialRot;
//    private float handInitialRot;
//    private float leftArmInitialRot;
//    private float leftElbowInitialRot;
//    private float leftHandInitialRot;
//    private float rightLegInitialRot;
//    private float rightFootInitialRot;
//    private float rightPillowInitialRot;
//    private float leftLegInitialRot;
//    private float leftFootInitialRot;
//    private float leftPillowInitialRot;

//    protected override void Awake()
//    {
//        base.Awake();
//        body = GetComponentInChildren<BodyBalance>();

//        // Lưu trạng thái ban đầu (null-safe)
//        if (rightArmBalance != null) armInitialRot = rightArmBalance.TargetRotation;
//        if (rightElbowBalance != null) elbowInitialRot = rightElbowBalance.TargetRotation;
//        if (rightHandBalance != null) handInitialRot = rightHandBalance.TargetRotation;

//        if (leftArmBalance != null) leftArmInitialRot = leftArmBalance.TargetRotation;
//        if (leftElbowBalance != null) leftElbowInitialRot = leftElbowBalance.TargetRotation;
//        if (leftHandBalance != null) leftHandInitialRot = leftHandBalance.TargetRotation;

//        if (rightLegBalance != null) rightLegInitialRot = rightLegBalance.TargetRotation;
//        if (rightFootBalance != null) rightFootInitialRot = rightFootBalance.TargetRotation;
//        if (rightPillowBalance != null) rightPillowInitialRot = rightPillowBalance.TargetRotation;

//        if (leftLegBalance != null) leftLegInitialRot = leftLegBalance.TargetRotation;
//        if (leftFootBalance != null) leftFootInitialRot = leftFootBalance.TargetRotation;
//        if (leftPillowBalance != null) leftPillowInitialRot = leftPillowBalance.TargetRotation;
//    }

//    private void Update()
//    {
//        if (Camera.main == null || body == null) return;

//        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//        mouseWorld.z = 0;
//        attackDir = (mouseWorld - body.transform.position).normalized;

//        Debug.DrawLine(body.transform.position, mouseWorld, Color.blue);
//    }

//    public void AttackHandle()
//    {
//        if (isAttacking) return;
//        isAttacking = true;

//        var values = System.Enum.GetValues(typeof(AttackType));
//        AttackType chosen = (AttackType)values.GetValue(Random.Range(0, values.Length));

//        StartCoroutine(DoAttack(chosen));
//    }

//    private IEnumerator DoAttack(AttackType type)
//    {
//        float extendPhase = attackDuration * 0.6f;
//        float retractPhase = attackDuration - extendPhase;

//        bool affectRightArm = false, affectLeftArm = false, affectRightLeg = false, affectLeftLeg = false;
//        bool affectRightPillow = false, affectLeftPillow = false;
//        float armExt = 0f, elbowExt = 0f, handExt = 0f, legExt = 0f, footExt = 0f, pillowExt = 0f;
//        float armRetr = 0f, elbowRetr = 0f, handRetr = 0f, legRetr = 0f, footRetr = 0f, pillowRetr = 0f;
//        float armImpulse = 0f, handImpulse = 0f, legImpulse = 0f, pillowImpulse = 0f, bodyImpulse = bodyForce;

//        switch (type)
//        {
//            case AttackType.RightPunch:
//                affectRightArm = true;
//                armExt = punchArmExt; elbowExt = punchElbowExt; handExt = punchHandExt;
//                armRetr = punchArmRetract; elbowRetr = punchElbowRetract; handRetr = 0f;
//                armImpulse = attackForce; handImpulse = handForce; bodyImpulse = bodyForce;
//                break;
//            case AttackType.LeftPunch:
//                affectLeftArm = true;
//                armExt = -punchArmExt; elbowExt = -punchElbowExt; handExt = -punchHandExt;
//                armRetr = -punchArmRetract; elbowRetr = -punchElbowRetract; handRetr = 0f;
//                armImpulse = attackForce; handImpulse = handForce; bodyImpulse = bodyForce;
//                break;
//            case AttackType.RightKick:
//                affectRightLeg = true;
//                legExt = kickLegExt; footExt = kickFootExt;
//                legRetr = kickLegRetract; footRetr = kickFootRetract;
//                legImpulse = legForce; bodyImpulse = bodyForce * 1.5f;
//                break;
//            case AttackType.LeftKick:
//                affectLeftLeg = true;
//                legExt = -kickLegExt; footExt = -kickFootExt;
//                legRetr = -kickLegRetract; footRetr = -kickFootRetract;
//                legImpulse = legForce; bodyImpulse = bodyForce * 1.5f;
//                break;
//            case AttackType.RightElbow:
//                affectRightArm = true;
//                armExt = elbowArmExt; elbowExt = elbowElbowExt; handExt = 0f;
//                armRetr = elbowArmRetract; elbowRetr = elbowElbowRetract;
//                armImpulse = attackForce * 0.9f; bodyImpulse = bodyForce * 0.9f;
//                break;
//            case AttackType.LeftElbow:
//                affectLeftArm = true;
//                armExt = -elbowArmExt; elbowExt = -elbowElbowExt; handExt = 0f;
//                armRetr = -elbowArmRetract; elbowRetr = -elbowElbowRetract;
//                armImpulse = attackForce * 0.9f; bodyImpulse = bodyForce * 0.9f;
//                break;
//            case AttackType.RightPillow:
//                affectRightPillow = true;
//                legExt = pillowLegExt; pillowExt = pillowPillowExt;
//                legRetr = pillowLegRetract; pillowRetr = pillowPillowRetract;
//                pillowImpulse = pillowForce; bodyImpulse = bodyForce * 1.2f;
//                break;
//            case AttackType.LeftPillow:
//                affectLeftPillow = true;
//                legExt = -pillowLegExt; pillowExt = -pillowPillowExt;
//                legRetr = -pillowLegRetract; pillowRetr = -pillowPillowRetract;
//                pillowImpulse = pillowForce; bodyImpulse = bodyForce * 1.2f;
//                break;
//        }

//        // Initial impulses
//        if (affectRightArm && rightArmBalance != null)
//        {
//            rightArmBalance.ApplyImpulse(attackDir, armImpulse);
//            if (rightHandBalance != null) rightHandBalance.ApplyImpulse(attackDir, handImpulse);
//            rightArmBalance.ModifyGravityTemporarily(0.5f, attackDuration);
//        }
//        if (affectLeftArm && leftArmBalance != null)
//        {
//            leftArmBalance.ApplyImpulse(attackDir, armImpulse);
//            if (leftHandBalance != null) leftHandBalance.ApplyImpulse(attackDir, handImpulse);
//            leftArmBalance.ModifyGravityTemporarily(0.5f, attackDuration);
//        }
//        if (affectRightLeg && rightLegBalance != null)
//        {
//            rightLegBalance.ApplyImpulse(attackDir, legImpulse);
//            if (rightFootBalance != null) rightFootBalance.ApplyImpulse(attackDir, legImpulse * 0.6f);
//            rightLegBalance.ModifyGravityTemporarily(0.6f, attackDuration);
//        }
//        if (affectLeftLeg && leftLegBalance != null)
//        {
//            leftLegBalance.ApplyImpulse(attackDir, legImpulse);
//            if (leftFootBalance != null) leftFootBalance.ApplyImpulse(attackDir, legImpulse * 0.6f);
//            leftLegBalance.ModifyGravityTemporarily(0.6f, attackDuration);
//        }
//        if (affectRightPillow && rightLegBalance != null && rightPillowBalance != null)
//        {
//            rightLegBalance.ApplyImpulse(attackDir, pillowImpulse * 0.7f);
//            rightPillowBalance.ApplyImpulse(attackDir, pillowImpulse);
//            rightPillowBalance.ModifyGravityTemporarily(0.4f, attackDuration);
//        }
//        if (affectLeftPillow && leftLegBalance != null && leftPillowBalance != null)
//        {
//            leftLegBalance.ApplyImpulse(attackDir, pillowImpulse * 0.7f);
//            leftPillowBalance.ApplyImpulse(attackDir, pillowImpulse);
//            leftPillowBalance.ModifyGravityTemporarily(0.4f, attackDuration);
//        }

//        if (body != null)
//        {
//            body.Rb.AddForce(attackDir * bodyImpulse, ForceMode2D.Impulse);
//            body.ModifyGravityTemporarily(0.9f, attackDuration);
//        }

//        // --- EXTEND PHASE ---
//        float timer = 0f;
//        while (timer < extendPhase)
//        {
//            timer += Time.deltaTime;

//            // Arms extension
//            if (affectRightArm && rightArmBalance != null)
//            {
//                rightArmBalance.SetTargetRotation(Mathf.LerpAngle(rightArmBalance.TargetRotation, armExt, stiffness * Time.deltaTime));
//                if (rightElbowBalance != null) rightElbowBalance.SetTargetRotation(Mathf.LerpAngle(rightElbowBalance.TargetRotation, elbowExt, stiffness * Time.deltaTime));
//                if (rightHandBalance != null) rightHandBalance.SetTargetRotation(Mathf.LerpAngle(rightHandBalance.TargetRotation, handExt, stiffness * Time.deltaTime));

//                rightArmBalance.Rb.linearVelocity = Vector2.Lerp(rightArmBalance.Rb.linearVelocity, attackDir * armImpulse, 0.18f);
//                if (rightHandBalance != null) rightHandBalance.Rb.linearVelocity = Vector2.Lerp(rightHandBalance.Rb.linearVelocity, attackDir * handImpulse, 0.18f);
//            }
//            if (affectLeftArm && leftArmBalance != null)
//            {
//                leftArmBalance.SetTargetRotation(Mathf.LerpAngle(leftArmBalance.TargetRotation, armExt, stiffness * Time.deltaTime));
//                if (leftElbowBalance != null) leftElbowBalance.SetTargetRotation(Mathf.LerpAngle(leftElbowBalance.TargetRotation, elbowExt, stiffness * Time.deltaTime));
//                if (leftHandBalance != null) leftHandBalance.SetTargetRotation(Mathf.LerpAngle(leftHandBalance.TargetRotation, handExt, stiffness * Time.deltaTime));

//                leftArmBalance.Rb.linearVelocity = Vector2.Lerp(leftArmBalance.Rb.linearVelocity, attackDir * armImpulse, 0.18f);
//                if (leftHandBalance != null) leftHandBalance.Rb.linearVelocity = Vector2.Lerp(leftHandBalance.Rb.linearVelocity, attackDir * handImpulse, 0.18f);
//            }

//            // Legs extension
//            if (affectRightLeg && rightLegBalance != null)
//            {
//                rightLegBalance.SetTargetRotation(Mathf.LerpAngle(rightLegBalance.TargetRotation, legExt, stiffness * Time.deltaTime));
//                if (rightFootBalance != null) rightFootBalance.SetTargetRotation(Mathf.LerpAngle(rightFootBalance.TargetRotation, footExt, stiffness * Time.deltaTime));
//                rightLegBalance.Rb.linearVelocity = Vector2.Lerp(rightLegBalance.Rb.linearVelocity, attackDir * legImpulse, 0.14f);
//                if (rightFootBalance != null) rightFootBalance.Rb.linearVelocity = Vector2.Lerp(rightFootBalance.Rb.linearVelocity, attackDir * legImpulse * 0.6f, 0.14f);
//            }
//            if (affectLeftLeg && leftLegBalance != null)
//            {
//                leftLegBalance.SetTargetRotation(Mathf.LerpAngle(leftLegBalance.TargetRotation, legExt, stiffness * Time.deltaTime));
//                if (leftFootBalance != null) leftFootBalance.SetTargetRotation(Mathf.LerpAngle(leftFootBalance.TargetRotation, footExt, stiffness * Time.deltaTime));
//                leftLegBalance.Rb.linearVelocity = Vector2.Lerp(leftLegBalance.Rb.linearVelocity, attackDir * legImpulse, 0.14f);
//                if (leftFootBalance != null) leftFootBalance.Rb.linearVelocity = Vector2.Lerp(leftFootBalance.Rb.linearVelocity, attackDir * legImpulse * 0.6f, 0.14f);
//            }

//            // Pillow extension
//            if (affectRightPillow && rightLegBalance != null && rightPillowBalance != null)
//            {
//                rightLegBalance.SetTargetRotation(Mathf.LerpAngle(rightLegBalance.TargetRotation, legExt, stiffness * Time.deltaTime));
//                rightPillowBalance.SetTargetRotation(Mathf.LerpAngle(rightPillowBalance.TargetRotation, pillowExt, stiffness * Time.deltaTime));
//                rightLegBalance.Rb.linearVelocity = Vector2.Lerp(rightLegBalance.Rb.linearVelocity, attackDir * pillowImpulse * 0.7f, 0.14f);
//                rightPillowBalance.Rb.linearVelocity = Vector2.Lerp(rightPillowBalance.Rb.linearVelocity, attackDir * pillowImpulse, 0.16f);
//            }
//            if (affectLeftPillow && leftLegBalance != null && leftPillowBalance != null)
//            {
//                leftLegBalance.SetTargetRotation(Mathf.LerpAngle(leftLegBalance.TargetRotation, legExt, stiffness * Time.deltaTime));
//                leftPillowBalance.SetTargetRotation(Mathf.LerpAngle(leftPillowBalance.TargetRotation, pillowExt, stiffness * Time.deltaTime));
//                leftLegBalance.Rb.linearVelocity = Vector2.Lerp(leftLegBalance.Rb.linearVelocity, attackDir * pillowImpulse * 0.7f, 0.14f);
//                leftPillowBalance.Rb.linearVelocity = Vector2.Lerp(leftPillowBalance.Rb.linearVelocity, attackDir * pillowImpulse, 0.16f);
//            }

//            if (body != null) body.Rb.linearVelocity = Vector2.Lerp(body.Rb.linearVelocity, attackDir * bodyImpulse, 0.1f);

//            // Apply damping
//            ApplyDamping();
//            yield return null;
//        }

//        // --- RETRACT PHASE ---
//        timer = 0f;
//        Vector2 pullBack = -attackDir * (Mathf.Max(armImpulse, legImpulse, pillowImpulse) * 0.6f);

//        if (affectRightArm && rightArmBalance != null) rightArmBalance.ApplyImpulse(-attackDir, Mathf.Max(armImpulse, 0.5f));
//        if (affectLeftArm && leftArmBalance != null) leftArmBalance.ApplyImpulse(-attackDir, Mathf.Max(armImpulse, 0.5f));
//        if (affectRightLeg && rightLegBalance != null) rightLegBalance.ApplyImpulse(-attackDir, Mathf.Max(legImpulse * 0.6f, 1f));
//        if (affectLeftLeg && leftLegBalance != null) leftLegBalance.ApplyImpulse(-attackDir, Mathf.Max(legImpulse * 0.6f, 1f));
//        if (affectRightPillow && rightPillowBalance != null) rightPillowBalance.ApplyImpulse(-attackDir, Mathf.Max(pillowImpulse * 0.5f, 1f));
//        if (affectLeftPillow && leftPillowBalance != null) leftPillowBalance.ApplyImpulse(-attackDir, Mathf.Max(pillowImpulse * 0.5f, 1f));

//        while (timer < retractPhase)
//        {
//            timer += Time.deltaTime;

//            if (affectRightArm && rightArmBalance != null)
//            {
//                rightArmBalance.SetTargetRotation(Mathf.LerpAngle(rightArmBalance.TargetRotation, armRetr, stiffness * 1.6f * Time.deltaTime));
//                if (rightElbowBalance != null) rightElbowBalance.SetTargetRotation(Mathf.LerpAngle(rightElbowBalance.TargetRotation, elbowRetr, stiffness * 1.6f * Time.deltaTime));
//                if (rightHandBalance != null) rightHandBalance.SetTargetRotation(Mathf.LerpAngle(rightHandBalance.TargetRotation, handRetr, stiffness * 1.6f * Time.deltaTime));

//                rightArmBalance.Rb.linearVelocity = Vector2.Lerp(rightArmBalance.Rb.linearVelocity, pullBack, 0.25f);
//                if (rightHandBalance != null) rightHandBalance.Rb.linearVelocity = Vector2.Lerp(rightHandBalance.Rb.linearVelocity, pullBack * 0.6f, 0.25f);
//            }

//            if (affectLeftArm && leftArmBalance != null)
//            {
//                leftArmBalance.SetTargetRotation(Mathf.LerpAngle(leftArmBalance.TargetRotation, armRetr, stiffness * 1.6f * Time.deltaTime));
//                if (leftElbowBalance != null) leftElbowBalance.SetTargetRotation(Mathf.LerpAngle(leftElbowBalance.TargetRotation, elbowRetr, stiffness * 1.6f * Time.deltaTime));
//                if (leftHandBalance != null) leftHandBalance.SetTargetRotation(Mathf.LerpAngle(leftHandBalance.TargetRotation, handRetr, stiffness * 1.6f * Time.deltaTime));

//                leftArmBalance.Rb.linearVelocity = Vector2.Lerp(leftArmBalance.Rb.linearVelocity, pullBack, 0.25f);
//                if (leftHandBalance != null) leftHandBalance.Rb.linearVelocity = Vector2.Lerp(leftHandBalance.Rb.linearVelocity, pullBack * 0.6f, 0.25f);
//            }

//            if (affectRightLeg && rightLegBalance != null)
//            {
//                rightLegBalance.SetTargetRotation(Mathf.LerpAngle(rightLegBalance.TargetRotation, legRetr, stiffness * 1.6f * Time.deltaTime));
//                if (rightFootBalance != null) rightFootBalance.SetTargetRotation(Mathf.LerpAngle(rightFootBalance.TargetRotation, footRetr, stiffness * 1.6f * Time.deltaTime));
//                rightLegBalance.Rb.linearVelocity = Vector2.Lerp(rightLegBalance.Rb.linearVelocity, pullBack * 0.8f, 0.18f);
//                if (rightFootBalance != null) rightFootBalance.Rb.linearVelocity = Vector2.Lerp(rightFootBalance.Rb.linearVelocity, pullBack * 0.6f, 0.18f);
//            }

//            if (affectLeftLeg && leftLegBalance != null)
//            {
//                leftLegBalance.SetTargetRotation(Mathf.LerpAngle(leftLegBalance.TargetRotation, legRetr, stiffness * 1.6f * Time.deltaTime));
//                if (leftFootBalance != null) leftFootBalance.SetTargetRotation(Mathf.LerpAngle(leftFootBalance.TargetRotation, footRetr, stiffness * 1.6f * Time.deltaTime));
//                leftLegBalance.Rb.linearVelocity = Vector2.Lerp(leftLegBalance.Rb.linearVelocity, pullBack * 0.8f, 0.18f);
//                if (leftFootBalance != null) leftFootBalance.Rb.linearVelocity = Vector2.Lerp(leftFootBalance.Rb.linearVelocity, pullBack * 0.6f, 0.18f);
//            }

//            if (affectRightPillow && rightLegBalance != null && rightPillowBalance != null)
//            {
//                rightLegBalance.SetTargetRotation(Mathf.LerpAngle(rightLegBalance.TargetRotation, legRetr, stiffness * 1.6f * Time.deltaTime));
//                rightPillowBalance.SetTargetRotation(Mathf.LerpAngle(rightPillowBalance.TargetRotation, pillowRetr, stiffness * 1.6f * Time.deltaTime));
//                rightLegBalance.Rb.linearVelocity = Vector2.Lerp(rightLegBalance.Rb.linearVelocity, pullBack * 0.7f, 0.18f);
//                rightPillowBalance.Rb.linearVelocity = Vector2.Lerp(rightPillowBalance.Rb.linearVelocity, pullBack * 0.8f, 0.2f);
//            }

//            if (affectLeftPillow && leftLegBalance != null && leftPillowBalance != null)
//            {
//                leftLegBalance.SetTargetRotation(Mathf.LerpAngle(leftLegBalance.TargetRotation, legRetr, stiffness * 1.6f * Time.deltaTime));
//                leftPillowBalance.SetTargetRotation(Mathf.LerpAngle(leftPillowBalance.TargetRotation, pillowRetr, stiffness * 1.6f * Time.deltaTime));
//                leftLegBalance.Rb.linearVelocity = Vector2.Lerp(leftLegBalance.Rb.linearVelocity, pullBack * 0.7f, 0.18f);
//                leftPillowBalance.Rb.linearVelocity = Vector2.Lerp(leftPillowBalance.Rb.linearVelocity, pullBack * 0.8f, 0.2f);
//            }

//            if (body != null) body.Rb.linearVelocity = Vector2.Lerp(body.Rb.linearVelocity, Vector2.zero, 0.2f);

//            ApplyDamping();
//            yield return null;
//        }

//        // --- RECOVERY PHASE: soft bounce back, cảm giác mềm mại và lơ lửng ---
//        timer = 0f;
//        while (timer < recoveryDuration)
//        {
//            timer += Time.deltaTime;
//            float t = timer / recoveryDuration;

//            // Restore rotations slowly để không cứng
//            if (rightArmBalance != null)
//            {
//                rightArmBalance.SetTargetRotation(Mathf.LerpAngle(rightArmBalance.TargetRotation, armInitialRot, 3f * Time.deltaTime));
//                if (rightElbowBalance != null) rightElbowBalance.SetTargetRotation(Mathf.LerpAngle(rightElbowBalance.TargetRotation, elbowInitialRot, 3f * Time.deltaTime));
//                if (rightHandBalance != null) rightHandBalance.SetTargetRotation(Mathf.LerpAngle(rightHandBalance.TargetRotation, handInitialRot, 3f * Time.deltaTime));
//            }
//            if (leftArmBalance != null)
//            {
//                leftArmBalance.SetTargetRotation(Mathf.LerpAngle(leftArmBalance.TargetRotation, leftArmInitialRot, 3f * Time.deltaTime));
//                if (leftElbowBalance != null) leftElbowBalance.SetTargetRotation(Mathf.LerpAngle(leftElbowBalance.TargetRotation, leftElbowInitialRot, 3f * Time.deltaTime));
//                if (leftHandBalance != null) leftHandBalance.SetTargetRotation(Mathf.LerpAngle(leftHandBalance.TargetRotation, leftHandInitialRot, 3f * Time.deltaTime));
//            }

//            if (rightLegBalance != null)
//            {
//                rightLegBalance.SetTargetRotation(Mathf.LerpAngle(rightLegBalance.TargetRotation, rightLegInitialRot, 3f * Time.deltaTime));
//                if (rightFootBalance != null) rightFootBalance.SetTargetRotation(Mathf.LerpAngle(rightFootBalance.TargetRotation, rightFootInitialRot, 3f * Time.deltaTime));
//            }
//            if (leftLegBalance != null)
//            {
//                leftLegBalance.SetTargetRotation(Mathf.LerpAngle(leftLegBalance.TargetRotation, leftLegInitialRot, 3f * Time.deltaTime));
//                if (leftFootBalance != null) leftFootBalance.SetTargetRotation(Mathf.LerpAngle(leftFootBalance.TargetRotation, leftFootInitialRot, 3f * Time.deltaTime));
//            }

//            if (rightPillowBalance != null) rightPillowBalance.SetTargetRotation(Mathf.LerpAngle(rightPillowBalance.TargetRotation, rightPillowInitialRot, 3f * Time.deltaTime));
//            if (leftPillowBalance != null) leftPillowBalance.SetTargetRotation(Mathf.LerpAngle(leftPillowBalance.TargetRotation, leftPillowInitialRot, 3f * Time.deltaTime));

//            // Soft velocity decay (bouncy feeling)
//            if (body != null) body.Rb.linearVelocity = Vector2.Lerp(body.Rb.linearVelocity, Vector2.zero, 0.08f);
//            if (rightArmBalance != null) rightArmBalance.Rb.linearVelocity = Vector2.Lerp(rightArmBalance.Rb.linearVelocity, Vector2.zero, 0.12f);
//            if (rightElbowBalance != null) rightElbowBalance.Rb.linearVelocity = Vector2.Lerp(rightElbowBalance.Rb.linearVelocity, Vector2.zero, 0.12f);
//            if (rightHandBalance != null) rightHandBalance.Rb.linearVelocity = Vector2.Lerp(rightHandBalance.Rb.linearVelocity, Vector2.zero, 0.12f);
//            if (leftArmBalance != null) leftArmBalance.Rb.linearVelocity = Vector2.Lerp(leftArmBalance.Rb.linearVelocity, Vector2.zero, 0.12f);
//            if (leftElbowBalance != null) leftElbowBalance.Rb.linearVelocity = Vector2.Lerp(leftElbowBalance.Rb.linearVelocity, Vector2.zero, 0.12f);
//            if (leftHandBalance != null) leftHandBalance.Rb.linearVelocity = Vector2.Lerp(leftHandBalance.Rb.linearVelocity, Vector2.zero, 0.12f);
//            if (rightLegBalance != null) rightLegBalance.Rb.linearVelocity = Vector2.Lerp(rightLegBalance.Rb.linearVelocity, Vector2.zero, 0.12f);
//            if (rightFootBalance != null) rightFootBalance.Rb.linearVelocity = Vector2.Lerp(rightFootBalance.Rb.linearVelocity, Vector2.zero, 0.12f);
//            if (rightPillowBalance != null) rightPillowBalance.Rb.linearVelocity = Vector2.Lerp(rightPillowBalance.Rb.linearVelocity, Vector2.zero, 0.12f);
//            if (leftLegBalance != null) leftLegBalance.Rb.linearVelocity = Vector2.Lerp(leftLegBalance.Rb.linearVelocity, Vector2.zero, 0.12f);
//            if (leftFootBalance != null) leftFootBalance.Rb.linearVelocity = Vector2.Lerp(leftFootBalance.Rb.linearVelocity, Vector2.zero, 0.12f);
//            if (leftPillowBalance != null) leftPillowBalance.Rb.linearVelocity = Vector2.Lerp(leftPillowBalance.Rb.linearVelocity, Vector2.zero, 0.12f);

//            // Light damping để giữ bouncy feel
//            ApplyDamping(0.93f);
//            yield return null;
//        }

//        // Final reset
//        ResetAllRotations();
//        ZeroAllVelocities();

//        isAttacking = false;
//        characterCtrl.ChangeState(state.idle);
//    }

//    private void ApplyDamping(float dampingValue = -1f)
//    {
//        if (dampingValue < 0f) dampingValue = damping;

//        if (body != null) body.Rb.linearVelocity *= dampingValue;
//        if (rightArmBalance != null) rightArmBalance.Rb.linearVelocity *= dampingValue;
//        if (rightElbowBalance != null) rightElbowBalance.Rb.linearVelocity *= dampingValue;
//        if (rightHandBalance != null) rightHandBalance.Rb.linearVelocity *= dampingValue;
//        if (leftArmBalance != null) leftArmBalance.Rb.linearVelocity *= dampingValue;
//        if (leftElbowBalance != null) leftElbowBalance.Rb.linearVelocity *= dampingValue;
//        if (leftHandBalance != null) leftHandBalance.Rb.linearVelocity *= dampingValue;
//        if (rightLegBalance != null) rightLegBalance.Rb.linearVelocity *= dampingValue;
//        if (rightFootBalance != null) rightFootBalance.Rb.linearVelocity *= dampingValue;
//        if (rightPillowBalance != null) rightPillowBalance.Rb.linearVelocity *= dampingValue;
//        if (leftLegBalance != null) leftLegBalance.Rb.linearVelocity *= dampingValue;
//        if (leftFootBalance != null) leftFootBalance.Rb.linearVelocity *= dampingValue;
//        if (leftPillowBalance != null) leftPillowBalance.Rb.linearVelocity *= dampingValue;
//    }

//    private void ResetAllRotations()
//    {
//        if (rightArmBalance != null) rightArmBalance.SetTargetRotation(armInitialRot);
//        if (rightElbowBalance != null) rightElbowBalance.SetTargetRotation(elbowInitialRot);
//        if (rightHandBalance != null) rightHandBalance.SetTargetRotation(handInitialRot);
//        if (leftArmBalance != null) leftArmBalance.SetTargetRotation(leftArmInitialRot);
//        if (leftElbowBalance != null) leftElbowBalance.SetTargetRotation(leftElbowInitialRot);
//        if (leftHandBalance != null) leftHandBalance.SetTargetRotation(leftHandInitialRot);
//        if (rightLegBalance != null) rightLegBalance.SetTargetRotation(rightLegInitialRot);
//        if (rightFootBalance != null) rightFootBalance.SetTargetRotation(rightFootInitialRot);
//        if (rightPillowBalance != null) rightPillowBalance.SetTargetRotation(rightPillowInitialRot);
//        if (leftLegBalance != null) leftLegBalance.SetTargetRotation(leftLegInitialRot);
//        if (leftFootBalance != null) leftFootBalance.SetTargetRotation(leftFootInitialRot);
//        if (leftPillowBalance != null) leftPillowBalance.SetTargetRotation(leftPillowInitialRot);
//    }

//    private void ZeroAllVelocities()
//    {
//        if (body != null) body.Rb.linearVelocity = Vector2.zero;
//        if (rightArmBalance != null) rightArmBalance.Rb.linearVelocity = Vector2.zero;
//        if (rightElbowBalance != null) rightElbowBalance.Rb.linearVelocity = Vector2.zero;
//        if (rightHandBalance != null) rightHandBalance.Rb.linearVelocity = Vector2.zero;
//        if (leftArmBalance != null) leftArmBalance.Rb.linearVelocity = Vector2.zero;
//        if (leftElbowBalance != null) leftElbowBalance.Rb.linearVelocity = Vector2.zero;
//        if (leftHandBalance != null) leftHandBalance.Rb.linearVelocity = Vector2.zero;
//        if (rightLegBalance != null) rightLegBalance.Rb.linearVelocity = Vector2.zero;
//        if (rightFootBalance != null) rightFootBalance.Rb.linearVelocity = Vector2.zero;
//        if (rightPillowBalance != null) rightPillowBalance.Rb.linearVelocity = Vector2.zero;
//        if (leftLegBalance != null) leftLegBalance.Rb.linearVelocity = Vector2.zero;
//        if (leftFootBalance != null) leftFootBalance.Rb.linearVelocity = Vector2.zero;
//        if (leftPillowBalance != null) leftPillowBalance.Rb.linearVelocity = Vector2.zero;
//    }
//}
#endregion
//using System.Collections;
//using UnityEngine;

///// <summary>
///// Hệ thống AttackTest_2: Tay phải đấm theo hướng chuột với cảm giác bouncy, smooth.
///// </summary>
//public class AttackTest_2 : ActionBase
//{
//    [Header("Attack Settings")]
//    [SerializeField] private float attackForce = 15f;        // Lực tay khi vươn ra
//    [SerializeField] private float handForce = 15f;          // Lực bàn tay
//    [SerializeField] private float bodyForce = 2f;           // Lực tác động nhẹ lên thân
//    [SerializeField] private float damping = 0.9f;           // Damping tổng thể
//    [SerializeField] private float stiffness = 8f;           // Độ mềm mại của tay khi di chuyển
//    [SerializeField] private float attackDuration = 0.25f;   // Thời gian toàn bộ cú đấm
//    [SerializeField] private float maxReachDistance = 2f;    // Khoảng cách tay vươn ra tối đa

//    [Header("References")]
//    [SerializeField] private RightArmBalance rightArmBalance;
//    [SerializeField] private RightElbowBalance rightElbowBalance;
//    [SerializeField] private RightHandBalance rightHandBalance; // Bàn tay

//    private BodyBalance body;
//    private Vector2 attackDir;
//    private bool isAttacking;

//    // Lưu trạng thái ban đầu
//    private float armInitialRot;
//    private float elbowInitialRot;
//    private float handInitialRot;

//    protected override void Awake()
//    {
//        base.Awake();
//        body = GetComponentInChildren<BodyBalance>();

//        // Lưu trạng thái ban đầu của tay, cùi trỏ, bàn tay
//        armInitialRot = rightArmBalance.TargetRotation;
//        elbowInitialRot = rightElbowBalance.TargetRotation;
//        handInitialRot = rightHandBalance.TargetRotation;
//    }

//    private void Update()
//    {
//        // Lấy hướng từ thân tới con trỏ chuột
//        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//        mouseWorld.z = 0;
//        attackDir = (mouseWorld - body.transform.position).normalized;

//        Debug.DrawLine(body.transform.position, mouseWorld, Color.blue);
//    }

//    /// <summary>
//    /// Hàm được gọi để kích hoạt cú đấm
//    /// </summary>
//    public void AttackHandle()
//    {
//        if (isAttacking) return;
//        isAttacking = true;

//        StartCoroutine(DoStraightPunch());
//    }

//    /// <summary>
//    /// Coroutine thực hiện cú đấm
//    /// Chia làm 2 giai đoạn: forward và retract
//    /// </summary>
//    private IEnumerator DoStraightPunch()
//    {
//        isAttacking = true;
//        Vector2 startPos = rightHandBalance.transform.position;
//        Vector2 targetPos = (Vector2)body.transform.position + attackDir * maxReachDistance;
//        float elapsed = 0f;

//        while (elapsed < attackDuration)
//        {
//            elapsed += Time.deltaTime;
//            float t = elapsed / attackDuration;
//            t = Mathf.SmoothStep(0f, 1f, t);

//            // Tính khoảng cách còn lại và giảm lực
//            Vector2 currentPos = rightHandBalance.transform.position;
//            Vector2 toTarget = targetPos - currentPos;
//            float distance = toTarget.magnitude;
//            float forceMultiplier = Mathf.Clamp01(distance / maxReachDistance);

//            // Di chuyển thẳng tay ra trước
//            Vector2 handVelocity = toTarget.normalized * handForce * forceMultiplier;
//            rightHandBalance.Rb.linearVelocity = handVelocity;

//            // Cùi trỏ + cánh tay follow nhẹ
//            rightElbowBalance.Rb.linearVelocity = handVelocity * 0.8f;
//            rightArmBalance.Rb.linearVelocity = handVelocity * 0.5f;

//            // Body follow nhẹ
//            body.Rb.linearVelocity = handVelocity * 0.2f;

//            // Damping tổng thể
//            ApplyDamping();

//            yield return null;
//        }

//        ResetAll();
//        isAttacking = false;
//        characterCtrl.ChangeState(state.idle);
//    }


//    /// <summary>
//    /// Áp dụng damping để tạo cảm giác bouncy, smooth
//    /// </summary>
//    private void ApplyDamping()
//    {
//        body.Rb.linearVelocity *= damping;
//        rightArmBalance.Rb.linearVelocity *= damping;
//        rightElbowBalance.Rb.linearVelocity *= damping;
//        rightHandBalance.Rb.linearVelocity *= damping;
//    }

//    /// <summary>
//    /// Reset rotation và velocity về trạng thái ban đầu
//    /// </summary>
//    private void ResetAll()
//    {
//        rightArmBalance.SetTargetRotation(armInitialRot);
//        rightElbowBalance.SetTargetRotation(elbowInitialRot);
//        rightHandBalance.SetTargetRotation(handInitialRot);

//        rightArmBalance.Rb.linearVelocity= Vector2.zero;
//        rightElbowBalance.Rb.linearVelocity = Vector2.zero;
//        rightHandBalance.Rb.linearVelocity = Vector2.zero;
//        body.Rb.linearVelocity = Vector2.zero;
//    }
//}
