using System.Collections.Generic;
using UnityEngine;

public class ActionPostBase 
{
    protected ActionDataSO[] actionDataSO;
    protected Balance[] balances;
    private Dictionary<string, ActionDataSO> actionDataDict;
    private Dictionary<BalanceType, Balance> balanceDict;
    private IPostBalance postBalance;   

    //get
    public Balance[] Balances => balances;
    public ActionDataSO[] ActionsDataSO => actionDataSO;

    public ActionPostBase(ActionDataSO[] actionDataSO, Balance[] balances, IPostBalance postBalance)
    {
        this.actionDataSO = actionDataSO;
        this.balances = balances;
        this.postBalance = postBalance;

        actionDataDict = new Dictionary<string, ActionDataSO>();
        balanceDict = new Dictionary<BalanceType, Balance>();

        if (actionDataSO.Length <= 0) return;
        foreach (var item in actionDataSO)
        {
            if (item == null) continue;
            actionDataDict[item.actionName] = item;
        }

        if (balances.Length <= 0) return;
        foreach (var item in balances)
        {
            if (item == null) return;
            balanceDict[item.Type] = item;
        }  
    }

    public void SetAction(string name)
    {
        ActionDataSO actionData = GetActionData(name);
        if (actionData == null) return;

        if(balances.Length <= 0) return;

        ExecutePostAction(balances, actionData);

    }

    #region Action Post Set

    public void SetPostAction(IPostBalance postBalance)
    {
        this.postBalance = postBalance;
    }
    public void ExecutePostAction(Balance[] balances, ActionDataSO dataSO)
    {
        postBalance.Post(balances, dataSO);
    }

    #endregion

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
    #endregion
}
