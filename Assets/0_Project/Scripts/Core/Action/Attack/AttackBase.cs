using Cysharp.Threading.Tasks;

using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class AttackBase
{
    // events
    public System.Action OnAttackEnd;
    public System.Action OnAttacking;

    [SerializeField] private List<AttackProperties> attackDatas = new List<AttackProperties>();

    private bool isAttacking;
    public bool IsAttacking => isAttacking;

    private CancellationTokenSource cts;

    /// <summary>
    /// Gọi attack (có thể cancel được)
    /// </summary>
    public async UniTask ExecuteAttack(
        AttackDataConfigSO configSO,
        Vector2 attackDir,
        Balance body,
        CancellationToken token
    )
    {
        isAttacking = true;
        cts = CancellationTokenSource.CreateLinkedTokenSource(token);

        try
        {
            // PHASE A: Pose
            await PostAttack(configSO, body, cts.Token);

            // PHASE B: Attack
            OnAttacking?.Invoke();
            await AttackApply(configSO, attackDir, body, cts.Token);
        }
        catch (System.OperationCanceledException)
        {
            // bị cancel thì kết thúc sớm
        }

        isAttacking = false;
        OnAttackEnd?.Invoke();
    }

    /// <summary>
    /// Cancel từ bên ngoài (nếu cần)
    /// </summary>
    public void CancelAttack()
    {
        if (cts != null && !cts.IsCancellationRequested)
            cts.Cancel();
    }


    // ======================================================
    // PHASE B: ATTACK APPLY
    // ======================================================
    private async UniTask AttackApply(
        AttackDataConfigSO configSO,
        Vector2 attackDir,
        Balance body,
        CancellationToken token
    )
    {
        float elapsed = 0f;

        while (elapsed < configSO.AttackDuration)
        {
            token.ThrowIfCancellationRequested();

            elapsed += Time.fixedDeltaTime;

            foreach (var item in attackDatas)
            {
                Balance arm = item.Balance;

                Vector2 targetPos =
                    (Vector2)body.Rb.position +
                    attackDir * configSO.AttackReach +
                    Vector2.Perpendicular(attackDir) * configSO.ProceduralOffset;

                if (elapsed < configSO.LaunchTime)
                {
                    arm.Rb.linearVelocity = attackDir * configSO.AttackForce;
                }

                SmoothMotionHelper.SmoothMoveTowardsLimited(
                    arm.Rb, targetPos,
                    configSO.MaxSpeed,
                    configSO.MaxForce,
                    configSO.DecelDistance
                );
            }

            // Body response
            body.Rb.linearVelocity = attackDir * configSO.AttackForce * 0.3f;

            SmoothMotionHelper.ApplySoftImpulse(
                body.Rb,
                attackDir,
                configSO.AttackForce * 0.2f,
                0.5f
            );

            await UniTask.WaitForFixedUpdate(token);
        }
    }


    // ======================================================
    // PHASE A: POSE PREPARATION
    // ======================================================
    private async UniTask PostAttack(
        AttackDataConfigSO configSO,
        Balance body,
        CancellationToken token
    )
    {
        float poseDuration = 0.35f;
        float elapsedPose = 0f;

        foreach (var item in attackDatas)
        {
            item.Balance.Rb.linearDamping = configSO.LinearDrag;
            item.Balance.Rb.angularDamping = configSO.AngularDrag;

            item.Balance.Rb.linearVelocity = Vector2.zero;
            item.Balance.Rb.angularVelocity = 0;
        }

        body.Rb.linearVelocity = Vector2.zero;
        body.Rb.angularVelocity = 0;

        while (elapsedPose < poseDuration)
        {
            token.ThrowIfCancellationRequested();

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

                part.SetRotation(t);
            }

            await UniTask.WaitForFixedUpdate(token);
        }
    }
}