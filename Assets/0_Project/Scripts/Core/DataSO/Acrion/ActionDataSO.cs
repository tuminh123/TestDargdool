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
}