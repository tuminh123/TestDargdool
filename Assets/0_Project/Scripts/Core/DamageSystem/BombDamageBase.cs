
using UnityEngine;
public abstract class BombDamageBase : MonoBehaviour
{
    [SerializeField] protected float damageBase;

    public abstract void VfxSpawm(GameObject obj);

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col == null) return;
        
        VfxSpawm(col.gameObject);

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
