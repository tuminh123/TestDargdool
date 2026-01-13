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
            if (!dataSO.TryGetBalanceData(item.Type, out var data))
            {
                //Debug.LogWarning($"Missing BalanceData: {item.Type}");
                continue;
            }

            float t = SmoothMotionHelper.SmoothRotateLimited(
                   item.Rotation,
                   data.rotChange,
                   configSO.RotateSmoothSpeed,
                   configSO.MaxAngularSpeed
            );
          
            item.SetRotation(t);
            Debug.Log(
  $"POST {dataSO.actionName} | balance={item.Type} | rot={data.rotChange}"
);

        }
    }
}
