using UnityEngine;

public class ActionPostNormal : IPostBalance
{
    public void Post(Balance[] balances, ActionDataSO dataSO)
    {
        foreach (var item in balances)
        {
            if (item == null) continue;

            // set action
            BalanceData data = dataSO.GetBalanceData(item.Type);

            item.SetRotation(data.rotChange);

        }
    }
}
