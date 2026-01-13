using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class AttackPhysicSmooth : IAttack
{
    private AttackData currentAttackData;
    private AttackDataConfigSO configSO;
    private ActionPostBase postBase;
    private string nameAttack;
    private GameObject obj;

    private CancellationTokenSource attackCTS;
    private CancellationTokenSource linkedCTS;

    public AttackPhysicSmooth(AttackData currentAttackData, AttackDataConfigSO configSO, ActionPostBase postBase, string nameAttack, GameObject obj)
    {
        this.currentAttackData = currentAttackData;
        this.configSO = configSO;
        this.postBase = postBase;
        this.nameAttack = nameAttack;
        this.obj = obj;
    }

    public void AttackHandle(Vector2 dir)
    {
        CancelAttack();

        attackCTS = new CancellationTokenSource();
        linkedCTS = CancellationTokenSource.CreateLinkedTokenSource(attackCTS.Token, obj.GetCancellationTokenOnDestroy());
        var token = linkedCTS.Token;

        UniTaskSafe.Forget(
           ct => currentAttackData.ExecuteAttack(configSO,dir,ct, postBase, nameAttack),
           token,
           "Smooth physic attack"
        );

        //HandleAttack(dir, postBase, nameAttack).Forget();
    }

    public void CancelAttack()
    {
        if (attackCTS != null)
        {
            attackCTS.Cancel();
            attackCTS.Dispose();
            attackCTS = null;
        }
        if (linkedCTS != null)
        {
            linkedCTS.Cancel();
            linkedCTS.Dispose();
            linkedCTS = null;
        }
    }
}
