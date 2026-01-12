using Cysharp.Threading.Tasks;
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
        HandleAttack(dir, postBase, nameAttack, obj).Forget();
    }
    public async UniTask HandleAttack(Vector2 attackDir, ActionPostBase postBase, string nameAttack, GameObject @object)
    {

        CancelAttack();

        attackCTS = new CancellationTokenSource();
        linkedCTS = CancellationTokenSource.CreateLinkedTokenSource(attackCTS.Token, @object.GetCancellationTokenOnDestroy());
        var token = linkedCTS.Token;

        AttackData attackData = new AttackData();

        currentAttackData = attackData;

        try
        {

            await attackData.ExecuteAttack(configSO, attackDir, token, postBase, nameAttack);
        }
        catch (System.OperationCanceledException)
        {
            Debug.Log("Attack error ");
        }
    }
    public void CancelAttack()
    {
        if (attackCTS == null)
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
