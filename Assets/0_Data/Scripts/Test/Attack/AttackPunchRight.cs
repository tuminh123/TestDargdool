using System.Collections;
using UnityEngine;

public class AttackPunchRight : AttackBase
{
    [Header("References")]
    [SerializeField] private RightArmBalance rightArmBalance;
    [SerializeField] private RightElbowBalance rightElbowBalance;
    [SerializeField] private RightHandBalance rightHandBalance; // Bàn tay
    [SerializeField] private float stiffness = 8f;
    [SerializeField] private float handForce = 15f;   // Độ đàn hồi tay

    // Lưu trạng thái ban đầu
    private float armInitialRot;
    private float elbowInitialRot;
    private float handInitialRot;

    protected override void Awake()
    {
        base.Awake();
        // Lưu trạng thái ban đầu của tay
        armInitialRot = rightArmBalance.TargetRotation;
        elbowInitialRot = rightElbowBalance.TargetRotation;
        handInitialRot = rightHandBalance.TargetRotation;
    }

    public override IEnumerator DoAttack(CharacterCtrl characterController)
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
        characterController.ChangeState(state.idle);
    }
}
