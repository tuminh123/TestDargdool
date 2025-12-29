using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

#region Attack Data

[System.Serializable]
public class AttackProperties
{
    [SerializeField] private Balance balance;
    [SerializeField] private float rot;
    [SerializeField] private float force;

    public Balance Balance => balance;
    public float Rot => rot;
    public float Force => force;
    public void ResetBalance()
    {
        balance.Rb.gravityScale = 1f;
        balance.Rb.mass = 1f;
    }
    public void ApplyBalance()
    {
        balance.Rb.gravityScale = 3f;
        balance.Rb.mass = 2f;
    }
}

[System.Serializable]
public class AttackData
{
    #region Couroutine
    /* //event
     public System.Action OnAttacking;
     public System.Action OnEndAttack;

     [SerializeField] private List<AttackProperties> attackDatas = new List<AttackProperties>();
     private bool isAttacking;
     public bool IsAttacking => isAttacking;
     public IEnumerator ExecuteAttack(AttackDataConfigSO configSO, Vector2 attackDir)
     {
         isAttacking = true;

         yield return AttackApply(configSO, attackDir);
         OnAttacking?.Invoke();
         yield return PostAttack(configSO);

         isAttacking = false;
         OnEndAttack?.Invoke();
     }

     private IEnumerator AttackApply(AttackDataConfigSO configSO, Vector2 attackDir)
     {
         float elapsed = 0f;

         while (elapsed < configSO.AttackDuration)
         {
             elapsed += Time.fixedDeltaTime;
             //elapsed += Time.fixedUnscaledDeltaTime;

             foreach (var item in attackDatas)
             {
                 Balance arm = item.Balance;

                 Vector2 targetPos =
                     attackDir * configSO.AttackReach +
                     Vector2.Perpendicular(attackDir) * configSO.ProceduralOffset;

                 // Apply lực đẩy trong Launch window
                 if (elapsed < configSO.LaunchTime)
                 {
                     arm.Rb.linearVelocity = attackDir * configSO.AttackForce;
                 }

                 // Move procedural giới hạn
                 SmoothMotionHelper.SmoothMoveTowardsLimited(
                     arm.Rb,
                     targetPos,
                     configSO.MaxSpeed,
                     configSO.MaxForce,
                     configSO.DecelDistance
                 );
             }

             yield return new WaitForFixedUpdate();
             //yield return null;
         }
     }

     private IEnumerator PostAttack(AttackDataConfigSO configSO)
     {
         float poseDuration = 0.35f;
         float elapsedPose = 0f;

         // Tạm thời tăng damping để cố định khớp
         foreach (var item in attackDatas)
         {
             item.Balance.Rb.linearDamping = configSO.LinearDrag;
             item.Balance.Rb.angularDamping = configSO.AngularDrag;

             // QUAN TRỌNG: Dừng motion
             item.Balance.Rb.linearVelocity = Vector2.zero;
             item.Balance.Rb.angularVelocity = 0;
         }

         // Xoay vào đúng pose
         while (elapsedPose < poseDuration)
         {
             elapsedPose += Time.fixedDeltaTime;
             //elapsedPose += Time.fixedUnscaledDeltaTime;

             foreach (var item in attackDatas)
             {
                 Balance part = item.Balance;
                 float targetRot = item.Rot;
                 float t = SmoothMotionHelper.SmoothRotateLimited(
                     part.Rotation,
                     targetRot,
                     configSO.RotateSmoothSpeed,
                     configSO.MaxAngularSpeed
                 );

                 part.SetRotation(t);
             }

             yield return new WaitForFixedUpdate();
             //yield return null;
         }
     }*/
    #endregion

    #region Unitask
    // events
    public System.Action OnEndAttack;
    public System.Action OnAttacking;

    [SerializeField] private List<AttackProperties> attackDatas = new List<AttackProperties>();

    private bool isAttacking;
    public bool IsAttacking => isAttacking;

    private CancellationTokenSource cts;

    /// <summary>
    /// Gọi attack (có thể cancel được)
    /// </summary>
    public async UniTask ExecuteAttack(
        AttackDataConfigSO configSO,
        Vector2 attackDir,
        CancellationToken token
    )
    {
        isAttacking = true;
        cts = CancellationTokenSource.CreateLinkedTokenSource(token);


        try
        {

            await AttackApply(configSO, attackDir, cts.Token);


            //await PostMass(configSO,cts.Token);
            //await PostAttack(configSO, cts.Token);

            OnAttacking?.Invoke();



        }
        catch (System.OperationCanceledException)
        {
            // bị cancel thì kết thúc sớm
        }
        isAttacking = false;
        OnEndAttack?.Invoke();
    }

    public void CancelAttack()
    {
        if (cts != null && !cts.IsCancellationRequested)
            cts.Cancel();
    }


    private async UniTask AttackApply(
        AttackDataConfigSO configSO,
        Vector2 attackDir,
        CancellationToken token
    )
    {
        Debug.Log("start");
        float elapsed = 0f;

        while (elapsed < configSO.AttackDuration)
        {
            token.ThrowIfCancellationRequested();

            elapsed += Time.fixedDeltaTime;

            foreach (var item in attackDatas)
            {

                if (item == null) continue;
                Balance arm = item.Balance;
                if (arm == null) continue;
                Vector2 targetPos =
                    attackDir * configSO.AttackReach +
                    Vector2.Perpendicular(attackDir) * configSO.ProceduralOffset;

                if (elapsed < configSO.LaunchTime)
                {
                    arm.Rb.linearVelocity = attackDir * configSO.AttackForce;
                }

                SmoothMotionHelper.SmoothMoveTowardsLimited(
                    arm.Rb, targetPos,
                    configSO.MaxSpeed,
                    configSO.MaxForce,
                    configSO.DecelDistance
                );

                item.ApplyBalance();
            }

            await UniTask.WaitForFixedUpdate(token);
        }
    }

    #region don't need ?
    private async UniTask PostMass(AttackDataConfigSO configSO, CancellationToken token)
    {

        foreach (var item in attackDatas)
        {
            item.Balance.Rb.linearDamping = configSO.LinearDrag;
            item.Balance.Rb.angularDamping = configSO.AngularDrag;

            item.Balance.Rb.linearVelocity = Vector2.zero;
            item.Balance.Rb.angularVelocity = 0;
            item.ResetBalance();
        }

        await UniTask.WaitForFixedUpdate(token);
    }

    private async UniTask PostAttack(
        AttackDataConfigSO configSO,
        CancellationToken token
    )
    {
        float poseDuration = 0.35f;
        float elapsedPose = 0f;

        foreach (var item in attackDatas)
        {
            item.Balance.Rb.linearDamping = configSO.LinearDrag;
            item.Balance.Rb.angularDamping = configSO.AngularDrag;

            item.Balance.Rb.linearVelocity = Vector2.zero;
            item.Balance.Rb.angularVelocity = 0;
            item.ResetBalance();
        }


        while (elapsedPose < poseDuration)
        {
            Debug.Log("1");
            token.ThrowIfCancellationRequested();

            elapsedPose += Time.fixedDeltaTime;

            foreach (var item in attackDatas)
            {
                Debug.Log("2");
                if (item == null) continue;
                Balance part = item.Balance;
                if (part == null) continue;
                float targetRot = item.Rot;

                float t = SmoothMotionHelper.SmoothRotateLimited(
                    part.Rotation,
                    targetRot,
                    configSO.RotateSmoothSpeed,
                    configSO.MaxAngularSpeed
                );

                part.SetRotation(t);
                Debug.Log("3");
            }

            await UniTask.WaitForFixedUpdate(token);
        }
    }
    #endregion

    #endregion

}

#endregion