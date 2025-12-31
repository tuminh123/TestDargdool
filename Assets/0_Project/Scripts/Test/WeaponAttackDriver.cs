using UnityEngine;

public class WeaponAttackDriver : MonoBehaviour
{
    [SerializeField] WeaponLimbController limb;

    public void Attack()
    {
        if (!limb.joint.enabled) return;

        limb.rb.AddTorque(800f, ForceMode2D.Impulse);
    }
}
