using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

#region Attack Data

[System.Serializable]
public class AttackImpulse
{
    [SerializeField] private Balance balance;
    [SerializeField] private float force = 10f;
    [SerializeField] private float torque = 5f;
    [SerializeField] private float balanceMultiplier = 1.5f;

    private bool balanceApplied;

    public Balance Balance => balance;
    public float Torque => torque;
   
    public float Force => force;

    public void ApplyBalanceOnce()
    {
        if (balanceApplied || balance == null) return;

        balance.Rb.gravityScale = 3f;
        balance.Rb.mass = 2f;
        balanceApplied = true;
    }

    public void ResetBalance()
    {
        if (balance == null) return;

        balance.Rb.gravityScale = 1f;
        balance.Rb.mass = 1f;
        balanceApplied = false;
    }
}

[System.Serializable]
public class AttackData
{
    #region Couroutine
    /* //event
     public System.Action OnAttacking;
     public System.Action OnEndAttack;

     [SerializeField] private List<AttackImpulse> attackDatas = new List<AttackImpulse>();
     private bool isAttacking;
     public bool IsAttacking => isAttacking;
     public IEnumerator ExecuteAttack(AttackDataConfigSO configSO, Vector2 attackDir)
     {
         isAttacking = true;

         yield return AttackApply(configSO, attackDir);
         OnAttacking?.Invoke();
         yield return PostAttack(configSO);

         isAttacking = false;
         OnEndAttack?.Invoke();
     }

     private IEnumerator AttackApply(AttackDataConfigSO configSO, Vector2 attackDir)
     {
         float elapsed = 0f;

         while (elapsed < configSO.AttackDuration)
         {
             elapsed += Time.fixedDeltaTime;
             //elapsed += Time.fixedUnscaledDeltaTime;

             foreach (var item in attackDatas)
             {
                 Balance arm = item.Balance;

                 Vector2 targetPos =
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

             yield return new WaitForFixedUpdate();
             //yield return null;
         }
     }

     private IEnumerator PostAttack(AttackDataConfigSO configSO)
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

         // Xoay vào đúng pose
         while (elapsedPose < poseDuration)
         {
             elapsedPose += Time.fixedDeltaTime;
             //elapsedPose += Time.fixedUnscaledDeltaTime;

             foreach (var item in attackDatas)
             {
                 Balance part = item.Balance;
                 float targetRot = item.Torque;
                 float t = SmoothMotionHelper.SmoothRotateLimited(
                     part.Rotation,
                     targetRot,
                     configSO.RotateSmoothSpeed,
                     configSO.MaxAngularSpeed
                 );

                 part.SetRotation(t);
             }

             yield return new WaitForFixedUpdate();
             //yield return null;
         }
     }*/
    #endregion

    #region Unitask
    #region Events

    public System.Action OnAttacking;
    public System.Action OnEndAttack;

    #endregion

    [SerializeField] private List<AttackImpulse> attackDatas = new();
    [SerializeField] TrailRenderer[] trails;
    private bool isAttacking;
    public bool IsAttacking => isAttacking;

    private CancellationTokenSource cts;

    #region Public API

    /// <summary>
    /// Execute attack (cancellable)
    /// </summary>
    public async UniTask ExecuteAttack(
        AttackDataConfigSO configSO,
        Vector2 attackDir,
        CancellationToken token
    )
    {
        if (isAttacking) return;

        isAttacking = true;
        cts = CancellationTokenSource.CreateLinkedTokenSource(token);

        try
        {
            OnAttacking?.Invoke();

            // Apply physics + motion song song
            await UniTask.WhenAll(
                AttackApply(configSO, attackDir, cts.Token),
                PostAttack(configSO, cts.Token)
            );
        }
        catch (OperationCanceledException)
        {
            // Attack bị cancel
        }
        finally
        {
            Cleanup();
            isAttacking = false;
            OnEndAttack?.Invoke();
        }
    }

    public void CancelAttack()
    {
        if (cts != null && !cts.IsCancellationRequested)
            cts.Cancel();
    }

    #endregion

    #region Core Logic

    private async UniTask AttackApply(
        AttackDataConfigSO configSO,
        Vector2 attackDir,
        CancellationToken token
    )
    {
        float elapsed = 0f;

        // ✅ Apply balances 1 lần duy nhất
        foreach (var item in attackDatas)
        {
            item?.ApplyBalanceOnce();
        }

        while (elapsed < configSO.AttackDuration)
        {
            token.ThrowIfCancellationRequested();
            elapsed += Time.fixedDeltaTime;

            foreach (var item in attackDatas)
            {
                if (item?.Balance == null) continue;

                Balance arm = item.Balance;

                Vector2 targetPos =
                    attackDir * configSO.AttackReach +
                    Vector2.Perpendicular(attackDir) * configSO.ProceduralOffset;

                // Launch force
                if (elapsed < configSO.LaunchTime)
                {
                    arm.Rb.AddForce(
                        attackDir * configSO.AttackForce,
                        ForceMode2D.Impulse
                    );
                }

                SmoothMotionHelper.SmoothMoveTowardsLimited(
                    arm.Rb,
                    targetPos,
                    configSO.MaxSpeed,
                    configSO.MaxForce,
                    configSO.DecelDistance
                );
            }

            await UniTask.WaitForFixedUpdate(token);
        }
    }

    private async UniTask PostAttack(
        AttackDataConfigSO configSO,
        CancellationToken token
    )
    {
        float poseDuration = 0.35f;
        float elapsed = 0f;

        // Freeze motion + reset physics
        foreach (var item in attackDatas)
        {
            if (item?.Balance == null) continue;

            var rb = item.Balance.Rb;
            rb.linearDamping = configSO.LinearDrag;
            rb.angularDamping = configSO.AngularDrag;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            item.ResetBalance();
        }

        while (elapsed < poseDuration)
        {
            token.ThrowIfCancellationRequested();
            elapsed += Time.fixedDeltaTime;

            foreach (var item in attackDatas)
            {
                if (item?.Balance == null) continue;

                Balance part = item.Balance;

                float t = SmoothMotionHelper.SmoothRotateLimited(
                    part.Rotation,
                    item.Torque,
                    configSO.RotateSmoothSpeed,
                    configSO.MaxAngularSpeed
                );

                part.SetRotation(t);
            }

            await UniTask.WaitForFixedUpdate(token);
        }
    }

    #endregion

    #region Cleanup

    private void Cleanup()
    {
        if (cts == null) return;

        cts.Dispose();
        cts = null;
    }

    #endregion

    public void EnableEffect(bool enable)
    {
        if (trails.Length <= 0) return;
        foreach (var item in trails)
        {
            if (item == null) continue;
            item.emitting = enable;
        }
    }

    #endregion
}


#endregion