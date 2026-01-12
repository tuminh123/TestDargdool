using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ActionPostSmooth : IPostBalance
{
    private AttackDataConfigSO configSO;

    public ActionPostSmooth(AttackDataConfigSO configSO)
    {
        this.configSO = configSO;
    }

    public void Post(Balance[] balances, ActionDataSO dataSO)
    {
        foreach (var item in balances)
        {
            if (item == null) continue;

            // set action
            BalanceData data = dataSO.GetBalanceData(item.Type);

            float t = SmoothMotionHelper.SmoothRotateLimited(
                   item.Rotation,
                   data.rotChange,
                   configSO.RotateSmoothSpeed,
                   configSO.MaxAngularSpeed
               );


            item.SetRotation(t);

        }
    }
}
