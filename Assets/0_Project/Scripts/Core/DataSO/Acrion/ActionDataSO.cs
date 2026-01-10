using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public enum action_type
{
    none = 0,
    idle = 1,
    attack = 2,
    move = 3,
}*/

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

    private void Awake()
    {
        balanceDataDict = new Dictionary<BalanceType, BalanceData>();
        if(balanceDatas.Length <= 0) return;

        foreach (var item in balanceDatas)
        {
            balanceDataDict[item.type] = item;
        }
    }

    public BalanceData GetBalanceData(BalanceType type)
    {
        if(balanceDataDict.TryGetValue(type, out BalanceData data))
        {
            return data;
        }
        else
        {
            return default;
        }
            /*foreach (var item in balanceDatas)
            {
                // Removed 'item == null' check because BalanceData is a struct and cannot be null
                if (item.type == type) return item;
            }
            // Return a default BalanceData if not found*/
           
    }
}