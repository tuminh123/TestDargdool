using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPostAction 
{
    public ActionDataSO GetActionData(string actionName);
    public Balance GetBalance(BalanceType type);
    public BalanceData[] GetBalanceArray(string name);
    public void SetAction(string name);
}