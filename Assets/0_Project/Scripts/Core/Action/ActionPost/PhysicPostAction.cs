using UnityEngine;
using UnityEngine.Profiling;

public class PhysicPostAction : ActionPostBase
{
    private Vector2 dir;
    private PhysicsAttackOriginalProfile profile;
    public PhysicPostAction(ActionDataSO[] actionDataSO, Balance[] balances,Vector2 dir,PhysicsAttackOriginalProfile profile) : base(actionDataSO, balances)
    {
        this.profile = profile;
        this.dir = dir;
    }
    public void SetDir(Vector2 dir)
    {
        this.dir = dir;
    }
    public override void SetAction(string name)
    {
        BalanceData[] balanceDatas = GetBalanceArray(name);
        if (balanceDatas == null || balanceDatas.Length <= 0) return;

        float totalMass = 0f;

        foreach (var item in balanceDatas)
        {
            Balance balance = GetBalance(item.type);
            if (balance == null) continue;

            totalMass += balance.Rb.mass;
        }

        foreach (var item in balanceDatas)
        {
            Balance balance = GetBalance(item.type);
            if (balance == null) continue;

            float ratio = balance.Rb.mass / totalMass;
            balance.Rb.AddForce(dir.normalized * profile.PushForce * ratio, ForceMode2D.Impulse);
            balance.Rb.AddTorque(dir.x * profile.ArmTorque, ForceMode2D.Force);
        }
    }   

}
