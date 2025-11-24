using System;
using UnityEngine;

public class HealthBase : MonoBehaviour,IDamageable
{
    public event Action<float, float> OnHealthChanged;
    public System.Action OnTakeDamage;
    public System.Action OnDead;

    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;

    public bool IsDead => currentHealth <= 0;

    private void Awake()
    {
        InitHealth();
    }

    public void InitHealth()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamaged(float damage)
    {
        if(IsDead) return;
        currentHealth -= damage;
        OnTakeDamage?.Invoke();

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //isDead = true;
        // xử lý chết ragdoll hoặc enemy
        Debug.Log($"{gameObject.name} died.");
        OnDead?.Invoke();
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
    }
}
