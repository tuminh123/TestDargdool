using UnityEngine;

public class HealthBase : MonoBehaviour,IDamageable
{
    public System.Action<float> OnTakeDamage;

    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;
   
    private bool isDead = false;
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    private void Awake()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamaged(float damage)
    {
        if(isDead) return;
        currentHealth -= damage;
        OnTakeDamage?.Invoke(damage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }



    private void Die()
    {
        isDead = true;
        // xử lý chết ragdoll hoặc enemy
        Debug.Log($"{gameObject.name} died.");
    }

    
}
