using System.Collections.Generic;
using UnityEngine;

public class ActionPostBase : IPostAction
{
    protected ActionDataSO[] actionDataSO;
    protected Balance[] balances;
    private Dictionary<string, ActionDataSO> actionDataDict;
    private Dictionary<BalanceType, Balance> balanceDict;
    private List<Balance> balanceOfActionsData = new List<Balance>();

    //get
    public Balance[] Balances => balances;
    public ActionDataSO[] ActionsDataSO => actionDataSO;

    public ActionPostBase(ActionDataSO[] actionDataSO, Balance[] balances)
    {
        this.actionDataSO = actionDataSO;
        this.balances = balances;

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
            if (item == null) continue;
            balanceDict[item.Type] = item;
        }  
    }



    public void SetAction(string name)
    {
        ActionDataSO actionData = GetActionData(name);
        if (actionData == null) return;

        if(balances.Length <= 0) return;

        foreach (var item in balances)
        {
            if (item == null) continue;

            // set action
            if (!actionData.TryGetBalanceData(item.Type, out var data)) continue;

            item.SetRotation(data.rotChange);

        }

    }

    #region Set Balance Attributes 
    
    public void DisableBalance()
    {
        if(balanceOfActionsData.Count <= 0) return;
        foreach (var item in balanceOfActionsData)
        {
            if (item == null) continue;
            item.DisablePose();
        }
    }
    public void EnableBalance()
    {
        if (balanceOfActionsData.Count <= 0) return;
        foreach (var item in balanceOfActionsData)
        {
            if (item == null) continue;
            item.EnablePose();
        }
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
