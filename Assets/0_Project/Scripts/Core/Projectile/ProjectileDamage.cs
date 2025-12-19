using UnityEngine;
using Zenject;

public class ProjectileDamage : MonoBehaviour
{
    [InjectOptional]
    private ProjectilePoolManager projectilePoolManager;
    [SerializeField] protected float damage;
    [SerializeField] protected LayerMask targetLayer;
    public ProjectileBase projectile { get; private set; }
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
                ZenManager.Instance. projectilePoolManager.DeSpawn(projectile);
            }
        }
    }

}
