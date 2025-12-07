
using DamageNumbersPro;
using System;
using UnityEngine;

public class HealthBase : MonoBehaviour,IDamageable,IHeal
{
    public event Action<float, float> OnHealthChanged;
    public System.Action OnTakeDamage;
    public System.Action OnDead;

    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private DamageNumber damageNumber;
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;

    public bool IsDead => currentHealth <= 0;

    private CharacterParent characterParent;
    private void Awake()
    {
        characterParent = GetComponentInParent<CharacterParent>();
        maxHealth = characterParent.Stats.MaxHealth;
    }

    private void Start()
    {
        InitHealth();
    }

    public void InitHealth()
    {
        currentHealth = maxHealth;
        //OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamaged(float damage)
    {
        string damageText = $" -{damage}";
        if(IsDead) return;
        currentHealth -= damage;

        OnTakeDamage?.Invoke();
        damageNumber.Spawn(transform.position, damageText);

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
        //Debug.Log($"{gameObject.name} died.");
        OnDead?.Invoke();
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    public void Heal(float add)
    {
        if (IsDead) return;
        if (currentHealth >= maxHealth) return;
        currentHealth += add;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
