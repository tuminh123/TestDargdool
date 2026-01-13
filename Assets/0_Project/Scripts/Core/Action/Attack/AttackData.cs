using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

#region Attack Data
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
    private bool isAttacking;
    public bool IsAttacking => isAttacking;

    #region Public API

    /// <summary>
    /// Execute attack (cancellable)
    /// </summary>
    public async UniTask ExecuteAttack(
        AttackDataConfigSO configSO,
        Vector2 attackDir,
        CancellationToken token,
        ActionPostBase postBase,
        string nameAction
    )
    {
        if (isAttacking) return;

        isAttacking = true;

        try
        {
            OnAttacking?.Invoke();

            postBase.InitBalancesOfActionDataSO(nameAction);
            // Apply physics + motion song song
            await UniTask.WhenAll(
                AttackApply(configSO, attackDir, token, postBase),
                PostAttack(configSO, token, postBase, nameAction)
                
            );

          
        }
        catch (OperationCanceledException)
        {
            // Attack bị cancel
        }
        finally
        {
            isAttacking = false;
            OnEndAttack?.Invoke();
            postBase.ClearBalancesOfActionDataSO();
        }
    }

    #endregion

    #region Core Logic

    private async UniTask AttackApply(AttackDataConfigSO configSO,Vector2 attackDir,CancellationToken token,ActionPostBase postBase)
    {
        float elapsed = 0f;
        Debug.Log("AttackApply started");
        Debug.Log(postBase.BalanceOfActionsData.Count.ToString());
        while (elapsed < configSO.AttackDuration)
        {
            token.ThrowIfCancellationRequested();
            elapsed += Time.fixedDeltaTime;

            foreach (var item in postBase.BalanceOfActionsData)
            {
                if (item == null) continue;

                Vector2 targetPos = attackDir * configSO.AttackReach + Vector2.Perpendicular(attackDir) * configSO.ProceduralOffset;

                Debug.Log($"AttackApply | balance={item.Type} | targetPos={targetPos}");

                // Launch force
                if (elapsed < configSO.LaunchTime)
                {
                    item.Rb.AddForce(attackDir * configSO.AttackForce, ForceMode2D.Impulse);
                }

                SmoothMotionHelper.SmoothMoveTowardsLimited(item.Rb, targetPos, configSO.MaxSpeed, configSO.MaxForce, configSO.DecelDistance);
            }

            await UniTask.WaitForFixedUpdate(token);
        }
}


    private async UniTask PostAttack(
        AttackDataConfigSO configSO,
        CancellationToken token,
        ActionPostBase postBase,
        string nameAction
    )
    {
        float poseDuration = 0.35f;
        float elapsed = 0f;

        // Freeze motion + reset physics
        foreach (var item in postBase.Balances)
        {
            if (item == null) continue;

            var rb = item.Rb;
            rb.linearDamping = configSO.LinearDrag;
            rb.angularDamping = configSO.AngularDrag;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        while (elapsed < poseDuration)
        {
            token.ThrowIfCancellationRequested();
            elapsed += Time.fixedDeltaTime;

            postBase.SetAction(nameAction);

            await UniTask.WaitForFixedUpdate(token);
        }
    }

    #endregion

    #endregion
}


#endregion