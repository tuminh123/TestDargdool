using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using Unity.Android.Gradle.Manifest;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public struct AttackIntent
{
    public Vector2 direction;   // hướng đánh (trái / phải)
    public float strength;      // cường độ (sau này dùng cho charge)
    public float Sign => Mathf.Sign(direction.x);
}
[System.Serializable]
public class PhysicsAttackProfile
{
    [Header("Torque (lực xoay)")]

    [Tooltip("Lực torque tác động lên tay")]
    public float armTorque = 10f;

    [Tooltip("Lực torque tác động lên vũ khí")]
    public float weaponTorque = 15f;

    [Header("Timing")]

    [Tooltip("Thời gian áp lực (giây)")]
    public float impulseDuration = 0.1f;

    [Header("Recovery")]
    [Header("Translation Force")]
    public float pushForce = 2.5f; // Lực lao tới

    [Header("Recovery")]
    [Tooltip("Angular damping sau khi đánh (giúp tay/vũ khí chậm dần)")]
    public float recoveryAngularDamping = 6f;
}
[System.Serializable]
public class AttackHandle
{
    public event System.Action OnAttackEnd;
    [SerializeField] private PhysicsAttackProfile profile;
    [SerializeField] private BalanceTest[] balances;

    public async UniTask Execute(AttackIntent intent,CancellationToken token)
    {
        try
        {
            await PostAttack(intent,token);

            ResetAttack();

            OnAttackEnd?.Invoke();
        }
        catch { }
    }

    private async UniTask PostAttack(AttackIntent intent,CancellationToken token)
    {

        float elapsed = 0f;

        try
        {

            // Áp lực trong một khoảng thời gian ngắn
            while (elapsed < profile.impulseDuration)
            {
                token.ThrowIfCancellationRequested();
                elapsed += Time.fixedDeltaTime;

                if (balances.Length <= 0) return;
                foreach (var item in balances)
                {
                    if (item == null) continue;
                    item.rb.AddForce(intent.direction.normalized * profile.pushForce * intent.strength, ForceMode2D.Impulse);

                    item.rb.AddTorque(intent.Sign * profile.armTorque, ForceMode2D.Impulse);
                    
                }
                await UniTask.WaitForFixedUpdate(token);
            }
        }
        catch { }
    }
    private void ResetAttack()
    {
        // Tăng damping để vật lý tự chậm lại
        if (balances.Length <= 0) return;
        foreach (var item in balances)
        {
            if (item == null) continue;
            item.rb.angularDamping = profile.recoveryAngularDamping;
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
    {
        if(balanceTests.Length <= 0) return;
        foreach (var item in balanceTests)
        {
            if(item == null ) continue;
            item.DisablePose();
        }
    }
    public void EnableBalance()
    {
        if (balanceTests.Length <= 0) return;
        foreach (var item in balanceTests)
        {
            if (item == null) continue;
            item.EnablePose();
        }
    }
    public void Attack(Vector2 dir)
    {
        AttackHandle attack = GetAttack();
        if(attack == null) return;

        currentAttack = attack;
       /* StopAttack();
        cts = new CancellationTokenSource();*/

        //Vector2 dir = Random.value > 0.5f ? Vector2.right : Vector2.left; 
        AttackIntent intent = new AttackIntent
        {
            direction = dir,
            strength = 1f
        };

        attack.Execute(intent, this.GetCancellationTokenOnDestroy()).Forget();
    }
    public AttackHandle GetAttack()
    {
        if(attackHandles.Length <= 0) return null;  
        int index = Random.Range(0, attackHandles.Length);
        return attackHandles[index];
    }

    public void StopAttack()
    {
        if (cts != null)
        {
            cts.Dispose();
            cts.Cancel();
            cts = null;
        }
        currentAttack = null;
    }
}

