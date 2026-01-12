using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Profiling;

public class WeaponAttackPhysicOriginal : IAttack
{
    private ActionPostBase postBase;
    private WeaponBase weapon;
    private GameObject obj;
    private AttackSystem attackSystem;

    private CancellationTokenSource atcToken;
    private CancellationTokenSource linkToken;

    public WeaponAttackPhysicOriginal( ActionPostBase postBase, WeaponBase weapon,GameObject obj, AttackSystem attackSystem)
    {
        this.postBase = postBase;
        this.weapon = weapon;
        this.obj = obj;
        this.attackSystem = attackSystem;
    }

    public void AttackHandle(Vector2 dir)
    {
        StopAttack();

        atcToken = new CancellationTokenSource();
        linkToken = CancellationTokenSource.CreateLinkedTokenSource(atcToken.Token, obj.GetCancellationTokenOnDestroy());

        var token = linkToken.Token;
        AttackIntent intent = new AttackIntent { direction = dir, strength = 1f };

        // Wrap the method call in a lambda to match the required Func<CancellationToken, UniTask> signature
        UniTaskSafe.Forget(
            ct => attackSystem.Execute(intent, ct, postBase, weapon),
            token,
            "Weapon original physic attack"
        );
    }

    private void StopAttack()
    {
        if (atcToken != null )
        {
            atcToken.Cancel();
            atcToken.Dispose();
            atcToken = null;
        }
        if (linkToken != null)
        {
            linkToken.Cancel();
            linkToken.Dispose();
            linkToken = null;
        }
    }

   /* #region Weapon Attack Handle
    public async UniTask Execute(AttackIntent intent, CancellationToken token, ActionPostBase actionPost, WeaponBase weapon)
    {
        try
        {
            await WeaponPostAttack(intent, token, actionPost,weapon);
            ResetBalanceAttack(weapon, actionPost);
            OnAttackEnd?.Invoke();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private async UniTask WeaponPostAttack(AttackIntent intent, CancellationToken token, ActionPostBase actionPost, WeaponBase weapon)
    {
        float elapsed = 0f;
        float totalMass = 0f;

        Balance[] balances = actionPost?.Balances;

        if (balances.Length <= 0) return;
        foreach (var item in balances)
        {
            if (item == null) continue;
            totalMass += item.Rb.mass;
        }
        try
        {
            while (elapsed < profile.ImpulseDuration)
            {
                token.ThrowIfCancellationRequested();
                elapsed += Time.fixedDeltaTime;

                foreach (var item in balances)
                {
                    if (item == null) continue;

                    float ratio = item.Rb.mass / totalMass;
                    item.Rb.AddForce(intent.direction.normalized * profile.PushForce * intent.strength * ratio, ForceMode2D.Impulse);
                    item.Rb.AddTorque(intent.Sign * profile.ArmTorque, ForceMode2D.Force);
                }
                if (weapon == null) return;
                weapon.rb.AddForce(intent.direction.normalized * profile.PushForce * intent.strength, ForceMode2D.Impulse);
                weapon.rb.AddTorque(intent.Sign * profile.WeaponTorque, ForceMode2D.Force);

                await UniTask.WaitForFixedUpdate(token);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }

    }
    private void ResetBalanceAttack(WeaponBase weapon,ActionPostBase actionPost)
    {
        Balance[] balances = actionPost?.Balances;
        if (balances.Length <= 0) return;
        foreach (var item in balances)
        {
            if (item == null) continue;
            item.Rb.angularDamping = profile.RecoveryAngularDamping;
        }
        if (weapon == null) return;
        weapon.rb.angularDamping = profile.RecoveryAngularDamping;
    }
    #endregion
*/
}
