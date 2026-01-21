
using UnityEngine;
public class DamageBase : MonoBehaviour
{
    [SerializeField] protected float damageBase;

    public void DamageHandle()
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col == null) return;
        VfxBase vfxFire = null;
        ZenManager.Instance?.vfxPoolManager?.SpawnVfx(StringConst.FIREVFX, col.gameObject, out vfxFire);

        if (!col.TryGetComponent(out IDamageable health)) return;

        if (health.IsDead) return;
        DamageHandle(health);
    }

    protected void DamageHandle(IDamageable health)
    {
        //Debug.Log($"1 ");
        health.TakeDamaged(damageBase);
    }
    public void DisableDamage()
    {
        gameObject.SetActive(false);
    }
    public void EnableDamage()
    {
        gameObject.SetActive(true);
    }
}
