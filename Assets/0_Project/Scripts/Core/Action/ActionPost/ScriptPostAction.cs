using UnityEngine;

public class ScriptPostAction : ActionPostBase
{
    public ScriptPostAction(ActionDataSO[] actionDataSO, Balance[] balances) : base(actionDataSO, balances)
    {
    }
    public override void SetAction(string name)
    {
        BalanceData[] balanceDatas = GetBalanceArray(name);
        if (balanceDatas == null || balanceDatas.Length <= 0) return;
        foreach (var item in balanceDatas)
        {
            Balance balance = GetBalance(item.type);
            if (balance == null) continue;
            balance.SetRotation(item.rotChange);
        }
    }
}
