using UnityEngine;

public class SmoothPostAction : ActionPostBase
{
    private AttackDataConfigSO configSO;
    public SmoothPostAction(ActionDataSO[] actionDataSO, Balance[] balances,AttackDataConfigSO configSO) : base(actionDataSO, balances)
    {
        this.configSO = configSO;
    }

    public override void SetAction(string name)
    {
        BalanceData[] balanceDatas = GetBalanceArray(name);
        if (balanceDatas == null || balanceDatas.Length <= 0) return;

        foreach (var item in balanceDatas)
        {
            Balance balance = GetBalance(item.type);
            if (balance == null) continue;

            var rb = balance.Rb;
            rb.linearDamping = configSO.LinearDrag;
            rb.angularDamping = configSO.AngularDrag;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

        }

      

        foreach (var item in balanceDatas)
        {
            Balance balance = GetBalance(item.type);
            if (balance == null) continue;

            float t = SmoothMotionHelper.SmoothRotateLimited(
                  balance.Rotation,
                  item.rotChange,
                  configSO.RotateSmoothSpeed,
                  configSO.MaxAngularSpeed
            );

            balance.SetRotation(t);
        }
    }
}
