using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected LayerMask targetLayer;
    public ProjectileBase projectile;
    private void Awake()
    {
        projectile = GetComponentInParent<ProjectileBase>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if(collision.TryGetComponent(out HealthBase health))
        {
            if (health == null) return;
            if (targetLayer.Contains(health.gameObject.layer)) 
            {
                health.TakeDamaged(damage);
                SingletonManager.Instance.projectilePoolManager.DeSpawn(projectile);
            }
        }
    }

}
