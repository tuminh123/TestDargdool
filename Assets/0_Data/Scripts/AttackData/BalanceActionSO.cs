
using System.Collections.Generic;
using UnityEngine;
public enum BalanceType
{
    none = 0,
    head = 1,
    body = 2,
    right_arm = 3,
    right_forearm = 4,
    right_hand = 5,
    left_arm = 6,
    left_forearm = 7,
    left_hand = 8,
    right_leg = 9,
    right_lower_leg = 10,
    right_foot = 11,
    left_leg = 12,
    left_lower_leg = 13,
    left_foot = 14,

}

public abstract class BalanceActionSO : ScriptableObject
{
    public List<BalanceData> balanceList = new();
    //Dictionary<BalanceType, BalanceData> balanceAttackDict = new();

    public BalanceData GetBalance(BalanceType type)
    {
        foreach (BalanceData balanceAttack in balanceList)
        {
            if (balanceAttack == null) continue;
            if (balanceAttack.type == type) return balanceAttack;
        }
        return null;
    }

}
[System.Serializable]
public class BalanceData 
{
    [Header("Ref")]
    public BalanceType type;
    //action
    [Header("Set")]
    [Space]
    public float targetRotation;
    public float force;
    //init
    [Header("Reset")]
    [Space]
    public float initTargetRotation;
    public float initForce;
    public float attackForce;

    public void Set(Balance balance)
    {
        Debug.Log("Set");
        if (balance.Type != type) return;
        balance.SetTargetRotation(targetRotation);
        balance.SetForce(force);
    }

    public void Reset(Balance balance)
    {
        Debug.Log("Reset");
        if (balance.Type != type) return;
        balance.SetTargetRotation(initTargetRotation);
        balance.SetForce(initForce);
    }
    public void AddForce(Balance balance,Vector3 dir)
    {
        Debug.Log("Add force");
        if (balance.Type != type) return;
        balance.Rb.linearVelocity = dir * (attackForce*1000) * Time.fixedDeltaTime;
    }

}

