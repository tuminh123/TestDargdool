using UnityEngine;

public class RegenerationDamageArea : MonoBehaviour
{
    [SerializeField] private float damage;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col == null) return;
       /* VfxBase vfxFire = null;
        ZenManager.Instance?.vfxPoolManager?.SpawnVfx(StringConst.FIREVFX, col.gameObject, out vfxFire);*/

        if (!col.TryGetComponent(out IDamageable health)) return;

        if (health.IsDead) return;
        health.TakeDamaged(damage);
    }
}
