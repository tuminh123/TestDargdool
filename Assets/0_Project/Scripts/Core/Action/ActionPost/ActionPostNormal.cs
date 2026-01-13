using UnityEngine;

public class ActionPostNormal : IPostBalance
{
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

            item.SetRotation(data.rotChange);

        }
    }
}
