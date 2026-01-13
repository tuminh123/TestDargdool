using System.Collections;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.Profiling;

public class ActionPostPhysic : IPostBalance
{
    private AttackIntent intent;
    private PhysicsAttackOriginalProfile profile;

    public ActionPostPhysic(AttackIntent intent, PhysicsAttackOriginalProfile profile)
    {
        this.intent = intent;
        this.profile = profile;
    }

    public void Post(Balance[] balances, ActionDataSO dataSO)
    {
        float totalMass = 0f;

        if (balances.Length <= 0) return;
        foreach (var item in balances)
        {
            if (item == null) continue;
            totalMass += item.Rb.mass*0.5f;
        }


        foreach (var item in balances)
        {
            if (item == null) continue;

            // set action
            if (!dataSO.TryGetBalanceData(item.Type, out var data))
            {
                //Debug.LogWarning($"Missing BalanceData: {item.Type}");
                continue;
            }

            float ratio = item.Rb.mass / totalMass;

            item.Rb.AddForce(intent.direction.normalized * profile.PushForce * intent.strength * ratio, ForceMode2D.Impulse);
            item.Rb.AddTorque(intent.Sign * profile.ArmTorque, ForceMode2D.Force);

        }
    }
}