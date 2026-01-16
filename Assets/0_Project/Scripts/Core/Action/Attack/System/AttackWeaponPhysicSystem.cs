using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Profiling;

public class AttackWeaponPhysicSystem : IRagdollAttackSystem
{
    public event Action OnAttackEnd;
    private PhysicsAttackOriginalProfile profile;
    private WeaponBase weapon;

    public AttackWeaponPhysicSystem(PhysicsAttackOriginalProfile profile,WeaponBase weapon)
    {
        this.profile = profile;
        this.weapon = weapon;
    }
    public void SetWeapon(WeaponBase weapon)
    {
        this.weapon = weapon;
    }
    public async UniTask ExecuteAttack(CancellationToken token, Vector2 dir, IPostAction postBase, string nameAction)
    {
        try
        {
            await UniTask.WhenAll(WeaponPostAttack(token, weapon, dir), BalancePostAttack(token, nameAction,postBase,dir));

        }
        catch (OperationCanceledException e)
        {
            Debug.Log(e);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {
            ResetBalanceAttack(weapon,postBase,nameAction);
            OnAttackEnd?.Invoke();
        }
    }
    private async UniTask WeaponPostAttack(CancellationToken token, WeaponBase weapon, Vector2 dir)
    {
        float elapsed = 0f;

        try
        {
            while (elapsed < profile.ImpulseDuration)
            {
                token.ThrowIfCancellationRequested();
                elapsed += Time.fixedDeltaTime;

                if (weapon == null) return;

                Debug.Log($"Apply force to {weapon.name}");

                weapon.rb.AddForce(dir * profile.PushForce, ForceMode2D.Impulse);
                weapon.rb.AddTorque(dir.x * profile.WeaponTorque, ForceMode2D.Force);

                if (token == null) return;
                await UniTask.WaitForFixedUpdate(token);
            }
        }
        catch (OperationCanceledException e)
        {
            Debug.Log(e);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }

    }

    private async UniTask BalancePostAttack(CancellationToken token, string nameAction, IPostAction postAction, Vector2 dir)
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
            Debug.Log(e);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void ResetBalanceAttack(WeaponBase weapon, IPostAction postBase,string nameAction)
    {
        BalanceData[] balancesData = postBase.GetBalanceArray(nameAction);
        if (balancesData.Length <= 0) return;
        foreach (var item in balancesData)
        {
            Balance balance = postBase.GetBalance(item.type);
            if (balance == null) continue;
            balance.Rb.angularDamping = profile.RecoveryAngularDamping;
        }

        if (weapon == null) return;
        weapon.rb.angularDamping = profile.RecoveryAngularDamping;

    }
}