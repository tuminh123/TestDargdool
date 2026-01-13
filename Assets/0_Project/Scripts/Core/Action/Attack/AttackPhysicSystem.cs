using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class AttackPhysicSystem
{
    public event System.Action OnAttackEnd;

    private Balance[] balances;
    private ActionDataSO[] actions;
    private PhysicsAttackOriginalProfile profile;
    private ActionPostBase actionPost;
    private AttackIntent intent;
    private IPostBalance postBalance;

    public AttackPhysicSystem(Balance[] balances, ActionDataSO[] actions, PhysicsAttackOriginalProfile profile, ActionPostBase actionPost, AttackIntent intent, IPostBalance postBalance)
    {
        this.balances = balances;
        this.actions = actions;
        this.profile = profile;
        this.actionPost = actionPost;
        this.intent = intent;
        this.postBalance = postBalance;
        actionPost = new ActionPostBase(actions, balances, /*new ActionPostPhysic(intent, profile)*/ postBalance);
        
    }

    public async UniTask Execute(CancellationToken token, WeaponBase weapon,string nameAction)
    {
        try
        {
            await UniTask.WhenAll(BalancePostAttack(token, nameAction), WeaponPostAttack( token, weapon));
            ResetBalanceAttack(weapon);
            OnAttackEnd?.Invoke();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }

    public async UniTask Execute(CancellationToken token,string nameAction)
    {
        try
        {
            await BalancePostAttack(token,nameAction);
            ResetBalanceAttack();
            OnAttackEnd?.Invoke();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }

    private async UniTask WeaponPostAttack(CancellationToken token, WeaponBase weapon)
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
    private async UniTask BalancePostAttack(CancellationToken token,string nameAction)
    {
        float elapsed = 0f;
        //float totalMass = 0f;

      /*  if (balances.Length <= 0) return;
        foreach (var item in balances)
        {
            if (item == null) continue;
            totalMass += item.Rb.mass;
        }*/

        try
        {
            // Áp lực trong một khoảng thời gian ngắn
            while (elapsed < profile.ImpulseDuration)
            {
                token.ThrowIfCancellationRequested();
                elapsed += Time.fixedDeltaTime;

                /* foreach (var item in balances)
                 {
                     if (item == null) continue;

                     float ratio = item.Rb.mass / totalMass;
                     item.Rb.AddForce(intent.direction.normalized * profile.PushForce * intent.strength * ratio, ForceMode2D.Impulse);
                     item.Rb.AddTorque(intent.Sign * profile.ArmTorque, ForceMode2D.Force);
                 }*/

                actionPost.SetAction(nameAction);

                await UniTask.WaitForFixedUpdate(token);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }


    private void ResetBalanceAttack(WeaponBase weapon)
    {
        if (weapon == null) return;
        weapon.rb.angularDamping = profile.RecoveryAngularDamping;
        ResetBalanceAttack();
    }
    private void ResetBalanceAttack()
    {
        if (balances.Length <= 0) return;
        foreach (var item in balances)
        {
            if (item == null) continue;

            // Tăng damping để vật lý tự chậm lại
            item.Rb.angularDamping = profile.RecoveryAngularDamping;
        }
    }
}
