using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class AttackPhysicOriginal : IAttack
{
    private GameObject obj;
    private AttackPhysicSystem attackSystem;
    private string actionName;

    private CancellationTokenSource atcToken;
    private CancellationTokenSource linkToken;

    public AttackPhysicOriginal(GameObject obj, AttackPhysicSystem attackSystem,string actionName)
    {
        this.obj = obj;
        this.attackSystem = attackSystem;
        this.actionName = actionName;
    }

    public void AttackHandle(Vector2 dir)
    {
        StopAttack();

        atcToken = new CancellationTokenSource();
        linkToken = CancellationTokenSource.CreateLinkedTokenSource(atcToken.Token, obj.GetCancellationTokenOnDestroy());

        var token = linkToken.Token;

        //attackSystem.Execute(intent, token, balanses).Forget();
        UniTaskSafe.Forget(
           ct => attackSystem.Execute(ct,actionName),
           token,
           "Weapon original physic attack"
        );
    }

    private void StopAttack()
    {
        if (atcToken != null)
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

/*    #region Balance Attack Handle

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

    #endregion*/
}
