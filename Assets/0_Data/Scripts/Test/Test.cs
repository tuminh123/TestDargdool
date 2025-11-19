using Core;
using UnityEngine;

namespace _0_Data.Scripts.Test
{
    public struct SignalTest
    {
    }
    public class Test : Visual, IReceive<SignalTest>
    {
        public void Receive(in SignalTest signal)
        {
            Debug.Log("a");
        }
          #region Test
    /*private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.E) && attackDir.x > 0)
        {
            IEnumerator rightPunch = SmoothAttack(rightArm, rightArmDown, 90, 80);
            IEnumerator rightKick = SmoothAttack(rightLeg, rightLegDown, 110, 100);
            IEnumerator rightElbow = SmoothAttack(rightArm, rightArm, 140, -120);
            IEnumerator rightPillow = SmoothAttack(rightLeg, rightLegDown,120,-60);

            IEnumerator[] rightBodyPart = { rightPunch, rightKick, rightElbow, rightPillow };
            int rand = Random.Range(0, rightBodyPart.Length);

            StartCoroutine(rightBodyPart[rand]);
        }
        if (InputManager.Instance.ConsumeAttackLeftBuffer() && attackDir.x < 0)
        {
            IEnumerator leftPunch = SmoothAttack(leftArm, leftArmDown, -90,- 80);
            IEnumerator leftKick = SmoothAttack(leftLeg,leftLegDown, -110, -100);
            IEnumerator leftElbow = SmoothAttack(leftArm, leftArmDown, -140, 120);
            IEnumerator leftPillow = SmoothAttack(leftLeg, leftLegDown,-120, 60);

            IEnumerator[] leftBodyPart = { leftPunch, leftKick, leftElbow, leftPillow };
            int rand = Random.Range(0, leftBodyPart.Length);

            StartCoroutine(leftBodyPart[rand]);
        }
           
    }

    private IEnumerator SmoothAttack(Balance part1, Balance part2, float rot_1, float rot_2)
    {
        isAttacking = true;

        // Lưu rotation ban đầu
        float initRot1 = part1.TargetRotation;
        float initRot2 = part2.TargetRotation;

        // Mục tiêu xoay forward
        float targetRot1 = initRot1 + rot_1;
        float targetRot2 = initRot2 + rot_2;

        // Cài đặt drag vật lý để tránh văng khớp
        part1.Rb.linearDamping = configSO.LinearDrag;
        part2.Rb.linearDamping = configSO.LinearDrag;
        part1.Rb.angularDamping = configSO.AngularDrag;
        part2.Rb.angularDamping = configSO.AngularDrag;

        float elapsed = 0f;
        float launchTime = 0.3f; // 10% duration dùng lực chính
        float proceduralOffset = 0.2f;
        while (elapsed < configSO.AttackDuration)
        {
            elapsed += Time.fixedDeltaTime;

            // 1️⃣ Xoay tay mượt với giới hạn tốc độ xoay
            float t_1 = SmoothMotionHelper.SmoothRotateLimited(part1.TargetRotation, targetRot1, configSO.RotateSmoothSpeed,configSO.MaxAngularSpeed);
            float t_2 = SmoothMotionHelper.SmoothRotateLimited(part2.TargetRotation, targetRot2, configSO.RotateSmoothSpeed,configSO.MaxAngularSpeed);
            part1.SetTargetRotation(t_1);
            part2.SetTargetRotation(t_2);
            // 2️⃣ Tính target theo momentum cơ thể
            Vector2 targetPos = (Vector2)body.Rb.position + attackDir *configSO.AttackReach + Vector2.Perpendicular(attackDir) * proceduralOffset;
            if (elapsed < launchTime)
            {
                part1.Rb.linearVelocity = attackDir * configSO.AttackForce;  // Đẩy tay thẳng tới target
                part2.Rb.linearVelocity = attackDir * configSO.AttackForce;
                body.Rb.AddForce(attackDir *configSO.AttackForce * 0.3f, ForceMode2D.Impulse); // Kéo body
            }
            // 3️⃣ Di chuyển tay procedural với lực giới hạn và giảm tốc
            SmoothMotionHelper.SmoothMoveTowardsLimited(part1.Rb, targetPos, maxSpeed, configSO.MaxForce, configSO.DecelDistance);
            SmoothMotionHelper.SmoothMoveTowardsLimited(part2.Rb, targetPos, maxSpeed, configSO.MaxForce, configSO.DecelDistance);

            // 4️⃣ Đẩy cơ thể bay nhẹ procedural
            SmoothMotionHelper.ApplySoftImpulse(body.Rb, attackDir, configSO.AttackForce * 0.2f, 0.5f);

            yield return new WaitForFixedUpdate();
        }

        // Reset rotation mượt
        part1.SetTargetRotation(initRot1);
        part2.SetTargetRotation(initRot2);

        isAttacking = false;
    }
    */


    #endregion

    }
}