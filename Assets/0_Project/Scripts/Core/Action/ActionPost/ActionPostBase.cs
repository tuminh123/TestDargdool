using System.Collections.Generic;
using UnityEngine;

public class ActionPostBase 
{
    protected ActionDataSO[] actionDataSO;
    protected Balance[] balances;
    private Dictionary<string, ActionDataSO> actionDataDict;
    private Dictionary<BalanceType, Balance> balanceDict;
    private List<Balance> balanceOfActionsData = new List<Balance>();
    private IPostBalance postBalance;  

    //get
    public Balance[] Balances => balances;
    public ActionDataSO[] ActionsDataSO => actionDataSO;
    public IPostBalance PostBalance => postBalance;
    public List<Balance> BalanceOfActionsData => balanceOfActionsData;

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
            if (item == null) continue;
            balanceDict[item.Type] = item;
        }  
    }

    public void InitBalancesOfActionDataSO(string name)
    {
        ActionDataSO actionData = GetActionData(name);
        if (actionData == null) return ;

        if(balanceOfActionsData.Count > 0) return;
        foreach (var item in balances)
        {
            if (!actionData.TryGetBalanceData(item.Type, out var data)) continue; 
            balanceOfActionsData.Add(item);
        }

    }
    public void ClearBalancesOfActionDataSO()
    {
        if( balanceOfActionsData.Count <= 0) return;
        balanceOfActionsData.Clear();
    }

    public void SetAction(string name)
    {
        ActionDataSO actionData = GetActionData(name);
        if (actionData == null) return;

        if(balances.Length <= 0) return;

        ExecutePostAction(balances, actionData);

    }

    #region Set Balance Attributes 
    
    public void DisableBalance(string name)
    {
        ActionDataSO actionData = GetActionData(name);
        if (actionData == null) return;

        if (balances.Length <= 0) return;
        foreach (var item in balances)
        {
            if (item == null) continue;

            // set action
            if (!actionData.TryGetBalanceData(item.Type, out var data))
            {
                //Debug.LogWarning($"Missing BalanceData: {item.Type}");
                continue;
            }
            item.DisablePose();
            item.Rb.mass = 3;
            item.Rb.gravityScale = 2;

        }
    }
    public void EnableBalance(string name)
    {
        ActionDataSO actionData = GetActionData(name);
        if (actionData == null) return;

        if (balances.Length <= 0) return;
        foreach (var item in balances)
        {
            if (item == null) continue;

            // set action
            if (!actionData.TryGetBalanceData(item.Type, out var data))
            {
                //Debug.LogWarning($"Missing BalanceData: {item.Type}");
                continue;
            }
            item.EnablePose();
            item.Rb.mass = 1;
            item.Rb.gravityScale = 1;

        }
    }

    #endregion

    #region Action Post Set

    public void SetPostAction(IPostBalance postBalance)
    {
        this.postBalance = postBalance;
        Debug.Log($"SET POST STRATEGY: {postBalance.GetType().Name}");
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
