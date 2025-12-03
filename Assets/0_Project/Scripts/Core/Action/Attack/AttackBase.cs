using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackBase : IAttackHandle
{
    [SerializeField] protected List<AttackProperties> balances = new List<AttackProperties>();
    [SerializeField] protected float speedAttack;

   
    #region Attck Handle
    public void AttackHandle(Vector2 dir)
    {
        SetTriggerBalance(false);

        Debug.Log("start");
        foreach (AttackProperties attack in balances)
        {
            if (attack == null || attack.Balance == null) continue;

            attack.Balance.SetRotation(attack.Rot);

        }
        SetAttackSpeedToAttack(dir);
    }
    public void AttackEnd()
    {
        Debug.Log("End");

        foreach (var item in balances)
        {
            if (item == null || item.Balance==null) continue;
            item.Balance.ResetData();
            Debug.Log($"Reset - {item.Balance.name}");
        }
        SetTriggerBalance(true);
    }
    #endregion

    #region Attack Element

    protected void SetAttackSpeedToAttack(Vector2 attackDir)
    {
        foreach (var balance in balances)
        {
            if(balance ==  null || balance.Balance == null) continue;
            balance.Balance.Rb.linearVelocity = attackDir * speedAttack;
        }
    }

    public bool CanAttack()
    {
        return balances.Count > 0;
    }

    protected void SetTriggerBalance(bool value)
    {
        foreach (var item in balances)
        {
            if (item == null || item.Balance) continue;
            item.Balance.SetIsTrigger(value);
        }
    }
    #endregion
}