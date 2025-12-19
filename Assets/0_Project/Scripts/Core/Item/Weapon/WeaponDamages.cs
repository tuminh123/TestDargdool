using System.Collections;
using UnityEngine;

public class WeaponDamages : MonoBehaviour
{
    //public event System.Action OnDamagedTo;
    [SerializeField] protected float damage;
    [SerializeField] protected LayerMask targetLayer;
    public bool isDamaged { get; private set; }
    public LayerMask TargetLayer => targetLayer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(isDamaged);
        if (collision == null) return;
        HealthBase health = collision.GetComponentInChildren<HealthBase>();
        if (health == null) return;
        if (targetLayer.Contains(health.gameObject.layer))
        {
            Debug.Log("Damage");
            if (isDamaged) return;
            health.TakeDamaged(damage);
            isDamaged = true;
            Debug.Log(isDamaged);
        }
    }
    public void SetTargetLayer(string layer)
    {
        this.targetLayer = LayerMask.GetMask(layer);
    }
    public void ResetDamage()
    {
        this.isDamaged = false;
    }
    public void Damaging()
    {
        this.isDamaged = true;
    }
}