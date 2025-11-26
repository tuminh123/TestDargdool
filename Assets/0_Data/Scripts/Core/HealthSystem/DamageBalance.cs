using UnityEditor;
using UnityEngine;

public class DamageBalance : MonoBehaviour
{
    [SerializeField] float damageMultiplier = 1f;
    [SerializeField] float minImpactForce = 100f; // ngưỡng va chạm
    [SerializeField] LayerMask damageableLayers;

    private Balance balance;

    private void Awake()
    {
        balance = GetComponent<Balance>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra layer có thể gây damage
        if ((damageableLayers.value & (1 << collision.gameObject.layer)) == 0) return;

        float impactForce = collision.relativeVelocity.magnitude;// Hoặc dùng: collision.enabledImpulse.magnitude nếu cần chính xác hơn

        if (impactForce < minImpactForce) return;  // không đủ mạnh → không gây damage

        Balance part = collision.transform.GetComponent<Balance>();
        Debug.Log($"part {part}");
        float partMultiplier = part != null ? GetDamageMultiplier() : 1f;
        Debug.Log($"partMultiplier { partMultiplier}");
        float damage = impactForce * damageMultiplier * partMultiplier;
        Debug.Log($"damage : {damage}");

        IDamageable damageable = collision.transform.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamaged(damage);
        }

    }

    private float GetDamageMultiplier()
    {
        switch (balance.DataSO.Type)
        {
            case BalanceType.head:
                return 15;
            case BalanceType.body_up:
            case BalanceType.body_bottom:
            case BalanceType.hip:
                return 10;
            case BalanceType.left_arm:
            case BalanceType.left_forearm:
            case BalanceType.left_hand:
            case BalanceType.left_leg:
            case BalanceType.left_lower_leg:
            case BalanceType.right_arm:
            case BalanceType.right_forearm:
            case BalanceType.right_hand:
            case BalanceType.right_leg:
            case BalanceType.right_lower_leg:
                return 5;
            default:
                return 1;
        }
    }
}
