using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;


public class AttackSmoothSystem : IRagdollAttackSystem
{
    private bool isAttacking;
    public bool IsAttacking => isAttacking;

    public event Action OnAttackEnd;
    private AttackDataConfigSO configSO;
    
    public AttackSmoothSystem(AttackDataConfigSO configSO)
    {
        this.configSO = configSO;
    }

    #region Public API

    /// <summary>
    /// Execute attack (cancellable)
    /// </summary>

    public async UniTask ExecuteAttack(CancellationToken token, Vector2 dir, IPostAction postBase, string nameAction)
    {
        if (isAttacking) return;

        isAttacking = true;

        try
        {
            //OnAttacking?.Invoke();

            // Apply physics + motion song song
            await UniTask.WhenAll
            (
                AttackApply(configSO, dir, token, postBase,nameAction),
                PostAttack(configSO, token, postBase, nameAction)
            );


        }
        catch (OperationCanceledException e)
        {
            Debug.LogException(e);
        }
        finally
        {
            isAttacking = false;
            OnAttackEnd?.Invoke();
        }
    }

    #endregion

    #region Core Logic

    private async UniTask AttackApply(AttackDataConfigSO configSO,Vector2 attackDir,CancellationToken token,IPostAction postBase,string nameAction)
    {
        float elapsed = 0f;
        ActionDataSO dataSO = postBase.GetActionData(nameAction);
        if (dataSO == null) return;
        //Debug.Log(postBase.Balances.Count.ToString());
        while (elapsed < configSO.AttackDuration)
        {
            token.ThrowIfCancellationRequested();
            elapsed += Time.fixedDeltaTime;

            if (postBase.Balances.Length <= 0) return;
            foreach (var item in postBase.Balances)
            {
                if (item == null) continue;
               
                if (!dataSO.TryGetBalanceData(item.Type, out var data)) continue;

                Vector2 targetPos = attackDir * configSO.AttackReach + Vector2.Perpendicular(attackDir) * configSO.ProceduralOffset;

                //Debug.Log($"AttackApply | balance={item.Type} | targetPos={targetPos}");

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
    private bool IsBalanceFromData(IPostAction postBase,string nameAction, Balance[] balances)
    {
        ActionDataSO dataSO = postBase.GetActionData(nameAction);
        if (dataSO == null) return false;
        foreach (var item in balances)
        {
            if (dataSO.TryGetBalanceData(item.Type, out var data))
            {
                return true;
            }
        }
        return false;

    }

    private async UniTask PostAttack(AttackDataConfigSO configSO,CancellationToken token,IPostAction postBase,string nameAction)
    {
        float poseDuration = 0.35f;
        float elapsed = 0f;

        ActionDataSO dataSO = postBase.GetActionData(nameAction);
        if (dataSO == null) return;
        if (postBase.Balances.Length <= 0) return;
        // Freeze motion + reset physics
        foreach (var item in postBase.Balances)
        {
            if (item == null) continue;
            if (!dataSO.TryGetBalanceData(item.Type, out var data)) continue;

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

            foreach (var item in postBase.Balances)
            {
                if(item == null) continue;
                if (!dataSO.TryGetBalanceData(item.Type, out var data)) continue;

                float t = SmoothMotionHelper.SmoothRotateLimited(
                      item.Rotation,
                      data.rotChange,
                      configSO.RotateSmoothSpeed,
                      configSO.MaxAngularSpeed
                );

                item.SetRotation(t);
            }

            await UniTask.WaitForFixedUpdate(token);
        }
    }

    #endregion

}
