using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class AttackPhysicSystem : IRagdollAttackSystem
{
    public event System.Action OnAttackEnd;

    private PhysicsAttackOriginalProfile profile;


    public AttackPhysicSystem(PhysicsAttackOriginalProfile profile)
    {
        this.profile = profile;   
    }

    public async UniTask ExecuteAttack(CancellationToken token, Vector2 dir, IPostAction postBase, string nameAction)
    {
        try
        {
            await BalancePostAttack(token, nameAction,postBase,dir);

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
            ResetBalanceAttack(postBase,nameAction);
            OnAttackEnd?.Invoke();
        }
    }
    private async UniTask BalancePostAttack(CancellationToken token, string nameAction,IPostAction postAction , Vector2 dir)
    {
        float elapsed = 0f;

        try
        {
            // Áp lực trong một khoảng thời gian ngắn
            while (elapsed < profile.ImpulseDuration)
            {
                token.ThrowIfCancellationRequested();
                elapsed += Time.fixedDeltaTime;

                postAction.SetAction(nameAction);

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

    private void ResetBalanceAttack(IPostAction postBase,string nameAction)
    {
        BalanceData[] balancesData = postBase.GetBalanceArray(nameAction);
        if (balancesData.Length <= 0) return;
        foreach (var item in balancesData)
        {
            Balance balance = postBase.GetBalance(item.type);
            if (balance == null) continue;
            balance.Rb.angularDamping = profile.RecoveryAngularDamping;
        }
    }
}
