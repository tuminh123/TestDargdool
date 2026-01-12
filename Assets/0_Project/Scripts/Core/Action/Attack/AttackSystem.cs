using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class AttackSystem
{
    public event System.Action OnAttackEnd;
    private PhysicsAttackOriginalProfile profile;

    //get
    public PhysicsAttackOriginalProfile Profile => profile;

    public AttackSystem(PhysicsAttackOriginalProfile profile)
    {
        this.profile = profile;
    }

    public async UniTask Execute(AttackIntent intent, CancellationToken token, ActionPostBase actionPost, WeaponBase weapon)
    {
        try
        {
            await UniTask.WhenAll(BalancePostAttack(intent, token, actionPost), WeaponPostAttack(intent, token, weapon));
            ResetBalanceAttack(weapon, actionPost);
            OnAttackEnd?.Invoke();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }

    public async UniTask Execute(AttackIntent intent, CancellationToken token, ActionPostBase actionPost)
    {
        try
        {
            await BalancePostAttack(intent, token, actionPost);
            ResetBalanceAttack(actionPost);
            OnAttackEnd?.Invoke();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }

    private async UniTask WeaponPostAttack(AttackIntent intent, CancellationToken token, WeaponBase weapon)
    {
        float elapsed = 0f;

        try
        {
            while (elapsed < profile.ImpulseDuration)
            {
                token.ThrowIfCancellationRequested();
                elapsed += Time.fixedDeltaTime;

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
    private async UniTask BalancePostAttack(AttackIntent intent, CancellationToken token, ActionPostBase actionPost)
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
            // Áp lực trong một khoảng thời gian ngắn
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

                await UniTask.WaitForFixedUpdate(token);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }


    private void ResetBalanceAttack(WeaponBase weapon, ActionPostBase actionPost)
    {
        if (weapon == null) return;
        weapon.rb.angularDamping = profile.RecoveryAngularDamping;
        ResetBalanceAttack(actionPost);
    }
    private void ResetBalanceAttack(ActionPostBase actionPost)
    {

        Balance[] balances = actionPost?.Balances;
        if (balances.Length <= 0) return;
        foreach (var item in balances)
        {
            if (item == null) continue;

            // Tăng damping để vật lý tự chậm lại
            item.Rb.angularDamping = profile.RecoveryAngularDamping;
        }
    }
}
