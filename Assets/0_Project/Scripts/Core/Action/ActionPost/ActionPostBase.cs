using System.Collections.Generic;
using UnityEngine;

public abstract class ActionPostBase : IPostAction
{
    protected ActionDataSO[] actionDataSO;
    protected Balance[] balances;

    protected Dictionary<string, ActionDataSO> actionDataDict;
    protected Dictionary<BalanceType, Balance> balanceDict;
    protected Dictionary<ActionDataSO, BalanceData[]> balancesArrayDataDict;


    public ActionPostBase(ActionDataSO[] actionDataSO, Balance[] balances)
    {
        this.actionDataSO = actionDataSO;
        this.balances = balances;

        actionDataDict = new Dictionary<string, ActionDataSO>();
        balanceDict = new Dictionary<BalanceType, Balance>();
        balancesArrayDataDict = new Dictionary<ActionDataSO, BalanceData[]>();

        if (actionDataSO.Length <= 0) return;
        foreach (var item in actionDataSO)
        {
            if (item == null) continue;
            actionDataDict[item.actionName] = item;
        }
        foreach (var item in actionDataSO)
        {
            if (item == null) continue;
            balancesArrayDataDict[item] = item.balanceDatas;
        }

        if (balances.Length <= 0) return;
        foreach (var item in balances)
        {
            if (item == null) continue;
            balanceDict[item.Type] = item;
        }

    }
    public abstract void SetAction(string name);

    #region Get Data
    public ActionDataSO GetActionData(string actionName)
    {
        if (actionDataDict.TryGetValue(actionName, out ActionDataSO actionData))
        {
            return actionData;
        }
        else
        {
            Debug.LogWarning($"ActionDataSO with Name: {actionName} not found.");
            return null;
        }
    }
    public Balance GetBalance(BalanceType type)
    {
        if (balanceDict.TryGetValue(type, out Balance balance))
        {
            return balance;
        }
        else
        {
            Debug.LogWarning($"Balance with Type: {type} not found.");
            return null;
        }
    }
    public BalanceData[] GetBalanceArray(string name)
    {
        ActionDataSO actionData = GetActionData(name);
        if(actionData == null) return null;
        if (!balancesArrayDataDict.TryGetValue(actionData, out var balanceDatas)) return null;
        return balanceDatas;
    }

    #endregion
}
