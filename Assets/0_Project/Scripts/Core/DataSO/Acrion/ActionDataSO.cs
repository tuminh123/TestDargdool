using System.Collections;
using UnityEngine;

public enum action_type
{
    none = 0,
    idle = 1,
    attack = 2,
    move = 3,
}

[System.Serializable]
public class BalanceData
{
    public BalanceType type;
    public float rotChange;
    public float forceChange;
}

[CreateAssetMenu(fileName ="Action Data SO")]
public class ActionDataSO : ScriptableObject
{
    public string actionName;
    public BalanceData[] balanceDatas;

    public BalanceData GetBalanceData(BalanceType type)
    {
        foreach (var item in balanceDatas)
        {
            if (item == null) continue;
            if (item.type == type) return item;
        }
        return null;
    }
}