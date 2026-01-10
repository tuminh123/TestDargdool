using System.Collections.Generic;
using UnityEngine;

public class ActionBase : MonoBehaviour
{
    [SerializeField] protected ActionDataSO[] actionDataSO;
    [SerializeField] protected RagdollController ragdollController;

    private Dictionary<string, ActionDataSO> actionDataDict;

    private void Awake()
    {
        actionDataDict = new Dictionary<string, ActionDataSO>();

        if(actionDataSO.Length <= 0) return;
        foreach (var item in actionDataSO)
        {
            if(item==null) continue;
            actionDataDict[item.actionName] = item;
        }
    }

    public void SetAction(string name)
    {
        if(ragdollController == null) return;
        Balance[] balances = ragdollController.Balances;
        if(balances.Length <= 0) return;

        foreach (var item in balances)
        {
            if(item == null) continue;

            ActionDataSO actionData = GetActionData(name);
            if(actionData == null) continue;
            if(actionData.GetBalanceData(item.Type) is BalanceData data)
            {
                item.SetBalanceData(data);
            }
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
