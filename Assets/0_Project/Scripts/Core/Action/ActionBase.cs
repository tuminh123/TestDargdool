using System.Collections.Generic;
using UnityEngine;

public class ActionBase 
{
    protected ActionDataSO[] actionDataSO;
    protected Balance[] balances;
    private Dictionary<string, ActionDataSO> actionDataDict;
    public ActionBase(ActionDataSO[] actionDataSO, Balance[] balances)
    {
        this.actionDataSO = actionDataSO;
        this.balances = balances;

        actionDataDict = new Dictionary<string, ActionDataSO>();

        if (actionDataSO.Length <= 0) return;
        foreach (var item in actionDataSO)
        {
            if (item == null) continue;
            actionDataDict[item.actionName] = item;
        }
    }

    
/*    private void Awake()
    {
        actionDataDict = new Dictionary<string, ActionDataSO>();

        if(actionDataSO.Length <= 0) return;
        foreach (var item in actionDataSO)
        {
            if(item==null) continue;
            actionDataDict[item.actionName] = item;
        }
    }*/

    public void SetAction(string name)
    {
        ActionDataSO actionData = GetActionData(name);
        if (actionData == null) return;

        if(balances.Length <= 0) return;

        foreach (var item in balances)
        {
            if(item == null) continue;

           /* if(actionData.GetBalanceData(item.Type) is BalanceData data)
            {
                item.SetBalanceData(data);
            }*/
           // set action
            BalanceData data = actionData.GetBalanceData(item.Type);

            item.SetRotation(data.rotChange);
            
        }
    }

    public ActionDataSO GetActionData(string actionName)
    {
        if (actionDataDict.TryGetValue(actionName, out ActionDataSO actionData))
        {
            return actionData;
        }
        else
        {
            Debug.LogWarning($"ActionDataSO with name {actionName} not found.");
            return null;
        }
    }
}
