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
    public async UniTask ExecuteAttack(CancellationToken token, Vector2 dir, IPostAction postBase, string nameAction)
    {
        try
        {
            await UniTask.WhenAll(WeaponPostAttack(token, weapon, dir), BalancePostAttack(token, nameAction,postBase,dir));

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

                await UniTask.WaitForFixedUpdate(token);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }

    }

    private async UniTask BalancePostAttack(CancellationToken token, string nameAction, IPostAction postAction, Vector2 dir)
    {
        float elapsed = 0f;
        float totalMass = 0f;

        if (postAction.Balances.Length <= 0) return;
        foreach (var item in postAction.Balances)
        {
            if (item == null) continue;

            if (postAction.GetActionData(nameAction) == null) continue;

            totalMass += item.Rb.mass;
        }

        try
        {
            // Áp lực trong một khoảng thời gian ngắn
            while (elapsed < profile.ImpulseDuration)
            {
                token.ThrowIfCancellationRequested();
                elapsed += Time.fixedDeltaTime;

                foreach (var item in postAction.Balances)
                {
                    if (item == null) continue;
                    if (postAction.GetActionData(nameAction) == null) continue;

                    float ratio = item.Rb.mass / totalMass;
                    item.Rb.AddForce(dir.normalized * profile.PushForce * ratio, ForceMode2D.Impulse);
                    item.Rb.AddTorque(dir.x * profile.ArmTorque, ForceMode2D.Force);
                }

                await UniTask.WaitForFixedUpdate(token);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void ResetBalanceAttack(WeaponBase weapon, IPostAction postBase,string nameAction)
    {
        if (postBase.Balances.Length <= 0) return;
        foreach (var item in postBase.Balances)
        {
            if (item == null) continue;
            if (postBase.GetActionData(nameAction) == null) continue;
            // Tăng damping để vật lý tự chậm lại
            item.Rb.angularDamping = profile.RecoveryAngularDamping;
        }

        if (weapon == null) return;
        weapon.rb.angularDamping = profile.RecoveryAngularDamping;

    }
}