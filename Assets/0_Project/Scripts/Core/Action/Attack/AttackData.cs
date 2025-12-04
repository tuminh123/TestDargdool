using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Attack Data

[System.Serializable]
public class AttackProperties
{
    [SerializeField] private Balance balance;
    [SerializeField] private float rot;
    [SerializeField] private float force;

    public Balance Balance => balance;
    public float Rot => rot;
    public float Force => force;
}

[System.Serializable]
public class AttackData
{
    //event
    public System.Action OnAttackEnd;
    public System.Action OnAttacking;

    [SerializeField] private List<AttackProperties> attackDatas = new List<AttackProperties>();
    private bool isAttacking;
    public bool IsAttacking => isAttacking;
    public IEnumerator ExecuteAttack(AttackDataConfigSO configSO, Vector2 attackDir, Balance body)
    {
        isAttacking = true;

        // ===================== PHASE A: POSE =====================
        yield return PostAttack(configSO, body);


        // ===================== PHASE B: ATTACK =====================
        OnAttacking?.Invoke();

        yield return AttackApply(configSO, attackDir, body);

        isAttacking = false;
        OnAttackEnd?.Invoke();
    }

    private IEnumerator AttackApply(AttackDataConfigSO configSO, Vector2 attackDir, Balance body)
    {
        float elapsed = 0f;

        while (elapsed < configSO.AttackDuration)
        {
            elapsed += Time.fixedDeltaTime;

            foreach (var item in attackDatas)
            {
                Balance arm = item.Balance;

                Vector2 targetPos =
                    (Vector2)body.Rb.position +
                    attackDir * configSO.AttackReach +
                    Vector2.Perpendicular(attackDir) * configSO.ProceduralOffset;

                // Apply lực đẩy trong Launch window
                if (elapsed < configSO.LaunchTime)
                {
                    arm.Rb.linearVelocity = attackDir * configSO.AttackForce;
                }

                // Move procedural giới hạn
                SmoothMotionHelper.SmoothMoveTowardsLimited(
                    arm.Rb,
                    targetPos,
                    configSO.MaxSpeed,
                    configSO.MaxForce,
                    configSO.DecelDistance
                );
            }

            // Body bị kéo theo — CHỈ TRONG PHASE ATTACK
            body.Rb.linearVelocity = attackDir * configSO.AttackForce * 0.3f;

            SmoothMotionHelper.ApplySoftImpulse(
                body.Rb,
                attackDir,
                configSO.AttackForce * 0.2f,
                0.5f
            );

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator PostAttack(AttackDataConfigSO configSO, Balance body)
    {
        float poseDuration = 0.35f;
        float elapsedPose = 0f;

        // Tạm thời tăng damping để cố định khớp
        foreach (var item in attackDatas)
        {
            item.Balance.Rb.linearDamping = configSO.LinearDrag;
            item.Balance.Rb.angularDamping = configSO.AngularDrag;

            // QUAN TRỌNG: Dừng motion
            item.Balance.Rb.linearVelocity = Vector2.zero;
            item.Balance.Rb.angularVelocity = 0;
        }

        // Body cũng tạm thời “giữ” để không bị kéo sớm
        body.Rb.linearVelocity = Vector2.zero;
        body.Rb.angularVelocity = 0;

        // Xoay vào đúng pose
        while (elapsedPose < poseDuration)
        {
            elapsedPose += Time.fixedDeltaTime;

            foreach (var item in attackDatas)
            {
                Balance part = item.Balance;
                float targetRot = item.Rot;

                float t = SmoothMotionHelper.SmoothRotateLimited(
                    part.Rotation,
                    targetRot,
                    configSO.RotateSmoothSpeed,
                    configSO.MaxAngularSpeed
                );

                part.SetRotation(targetRot);
            }

            yield return new WaitForFixedUpdate();
        }
    }
}

#endregion