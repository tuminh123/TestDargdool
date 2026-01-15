using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using UnityEngine;

public class RagdollAttackContext
{
    private IRagdollAttackSystem ragdollAttack;
    //token
    private CancellationTokenSource atc;
    private CancellationTokenSource ltc;

    public RagdollAttackContext(IRagdollAttackSystem ragdollAttack)
    {
        this.ragdollAttack = ragdollAttack;
    }
    public void SetRagdollAttack(IRagdollAttackSystem ragdollAttack)
    {
        this.ragdollAttack = ragdollAttack;
    }
    public void ExecuteAttack(Vector2 dir, IPostAction postAction, string name, GameObject obj)
    {

        CancelAttack();

        atc = new CancellationTokenSource();
        ltc = CancellationTokenSource.CreateLinkedTokenSource(atc.Token, obj.GetCancellationTokenOnDestroy());
        var token = ltc.Token;

        UniTaskSafe.Forget
        (
            ct => this.ragdollAttack.ExecuteAttack(ct, dir, postAction, name),
            token,
            "attacking task"
        );

    }
    public void CancelAttack()
    {
        if (ltc != null)
        {
            if (!ltc.IsCancellationRequested) ltc.Cancel();

            ltc.Dispose();
            ltc = null;
        }

        if (atc != null)
        {
            if (!atc.IsCancellationRequested) atc.Cancel();

            atc.Dispose();
            atc = null;
        }
    }
}