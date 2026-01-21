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
            //Debug.LogException(e);
            Debug.LogWarning(e);
        }
        catch (System.Exception e)
        {
            //Debug.LogException(e);
            Debug.LogWarning(e);
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

        try
        {
            //Debug.Log(postBase.Balances.Count.ToString());
            while (elapsed < configSO.AttackDuration)
            {
                token.ThrowIfCancellationRequested();
                elapsed += Time.fixedDeltaTime;

                BalanceData[] balancesData = postBase.GetBalanceArray(nameAction);
                if (balancesData.Length <= 0) return;
                foreach (var item in balancesData)
                {
                    Balance balance = postBase.GetBalance(item.type);
                    if (balance == null) continue;

                    Vector2 targetPos = attackDir * configSO.AttackReach + Vector2.Perpendicular(attackDir) * configSO.ProceduralOffset;

                    //Debug.Log($"AttackApply | balance={item.Type} | targetPos={targetPos}");

                    if (elapsed < configSO.LaunchTime)
                    {
                        balance.Rb.AddForce(attackDir * configSO.AttackForce, ForceMode2D.Impulse);
                        //balance.Rb.linearVelocity =  attackDir * configSO.AttackForce;
                    }

                    SmoothMotionHelper.SmoothMoveTowardsLimited(balance.Rb, targetPos, configSO.MaxSpeed, configSO.MaxForce, configSO.DecelDistance);
                }

                await UniTask.WaitForFixedUpdate(token);

            }
        }
        catch (OperationCanceledException e)
        {
            //Debug.LogException(e);
            Debug.LogWarning(e);
        }
        catch (System.Exception e)
        {
            //Debug.LogException(e);
            Debug.LogWarning(e);
        }
    }

    private async UniTask PostAttack(AttackDataConfigSO configSO,CancellationToken token,IPostAction postBase,string nameAction)
    {
        float poseDuration = 0.35f;
        float elapsed = 0f;

        try
        {

            while (elapsed < poseDuration)
            {
                token.ThrowIfCancellationRequested();
                elapsed += Time.fixedDeltaTime;

                postBase.SetAction(nameAction);

                await UniTask.WaitForFixedUpdate(token);
            }
        }
        catch (OperationCanceledException e)
        {
            //Debug.LogException(e);
            Debug.LogWarning(e);
        }
        catch (System.Exception e)
        {
            //Debug.LogException(e);
            Debug.LogWarning(e);
        }
    }

    #endregion

}
