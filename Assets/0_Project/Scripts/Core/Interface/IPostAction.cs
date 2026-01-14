using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPostAction 
{
    public Balance[] Balances { get; }
    public ActionDataSO GetActionData(string actionName);
    public Balance GetBalance(BalanceType type);
    public void SetAction(string name);
}