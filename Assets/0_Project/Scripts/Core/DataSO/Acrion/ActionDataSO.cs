using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct BalanceData
{
    public BalanceType type;
    public float rotChange;
    public float forceChange;
}

[CreateAssetMenu(fileName = "Data SO", menuName = "Data SO/Action")]
public class ActionDataSO : ScriptableObject
{
    public string actionName;
    public  BalanceData[] balanceDatas;
    private Dictionary<BalanceType, BalanceData> balanceDataDict;

    private void OnEnable()
    {
        balanceDataDict = new Dictionary<BalanceType, BalanceData>();
        if (balanceDatas.Length <= 0) return;

        foreach (var item in balanceDatas)
        {
            balanceDataDict[item.type] = item;
        }
    }

    public bool TryGetBalanceData(BalanceType type, out BalanceData data)
    {
        return balanceDataDict.TryGetValue(type, out data);
    }
}