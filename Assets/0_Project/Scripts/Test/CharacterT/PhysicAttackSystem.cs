using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;

public struct AttackIntent
{
    public Vector2 direction; // hướng đánh (trái / phải)
    public float strength; // cường độ (sau này dùng cho charge)
    public float Sign => Mathf.Sign(direction.x); 
}

[System.Serializable] 
public class AttackHandle 
{ 
    public event System.Action OnAttackEnd;
    [SerializeField] private Balance[] balances;
    [SerializeField] private PhysicsAttackOriginalProfile profile;

    public async UniTask Execute(AttackIntent intent, CancellationToken token, ActionPostBase actionPost,WeaponBase weapon)
    {
        try
        {
            await UniTask.WhenAll( BalancePostAttack(intent, token, actionPost),WeaponPostAttack(intent,token,weapon));
            ResetBalanceAttack(weapon,actionPost);
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

    private async UniTask WeaponPostAttack(AttackIntent intent, CancellationToken token,WeaponBase weapon)
    {
        float elapsed = 0f;

        try
        {
            while (elapsed < profile.ImpulseDuration)
            {
                token.ThrowIfCancellationRequested();
                elapsed += Time.fixedDeltaTime;

                if(weapon == null) return;
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
    private async UniTask BalancePostAttack(AttackIntent intent,CancellationToken token, ActionPostBase actionPost) 
    { 
        float elapsed = 0f;
        float totalMass = 0f;

       /* Balance[] balances = actionPost?.Balances;*/

        if (balances.Length <= 0) return;
        foreach (var item in balances)
        {
            if(item == null) continue;
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
                    item.Rb.AddForce(intent.direction.normalized * profile.PushForce * intent.strength*ratio, ForceMode2D.Impulse); 
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
    private void ResetBalanceAttack(ActionPostBase actionPost) { 
       
       /* Balance[] balances = actionPost?.Balances;*/
        if (balances.Length <= 0) return; 
        foreach (var item in balances) 
        { 
            if (item == null) continue;

            // Tăng damping để vật lý tự chậm lại
            item.Rb.angularDamping = profile.RecoveryAngularDamping; 
        } 
    } 
}

public class PhysicAttackSystem : MonoBehaviour
{
    [SerializeField] AttackHandle[] attackHandles; 
    [SerializeField] BalanceTest[] balanceTests; 
    private CancellationTokenSource cts; 
    public AttackHandle currentAttack { get; private set; }
    public void DisableBalance() 
    { if (balanceTests.Length <= 0) return; 
        foreach (var item in balanceTests) 
        { if (item == null) continue; 
            item.DisablePose(); 
        }
    }

    public void EnableBalance() { 
        if (balanceTests.Length <= 0) return; 
        foreach (var item in balanceTests) 
        { if (item == null) continue; 
            item.EnablePose(); 
        } 
    }

    public void Attack(Vector2 dir)
    {
        AttackHandle attack = GetAttack(); 
        if (attack == null) return; currentAttack = attack; 
        /* StopAttack(); cts_Shoot = new CancellationTokenSource();*/ 
        //Vector2 dir = Random.value > 0.5f ? Vector2.right : Vector2.left;
        AttackIntent intent = new AttackIntent { direction = dir, strength = 1f }; 
        //attack.Execute(intent, this.GetCancellationTokenOnDestroy()).Forget(); 
    } 
    public AttackHandle GetAttack() 
    { 
        if(attackHandles.Length <= 0) return null; 
        int index = Random.Range(0, attackHandles.Length); 
        return attackHandles[index];
    } 
    public void StopAttack() {
        if (cts != null) 
        { 
            cts.Dispose(); 
            cts.Cancel(); 
            cts = null; 
        } 
        currentAttack = null;
    } 
}
