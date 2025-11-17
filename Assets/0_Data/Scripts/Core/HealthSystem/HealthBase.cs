using UnityEngine;

public class HealthBase : MonoBehaviour,IDamageable
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;
    [SerializeField] private Faction faction;
    //[SerializeField] private float damageTaken ;
    private bool isDead = false;
    private HealthBalance[] healthBalance;
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public Faction Faction => faction;
    //public float DamageTaken => damageTaken;

    private void Awake()
    {
        currentHealth = maxHealth;
        healthBalance = GetComponentsInChildren<HealthBalance>();
    }
    private void OnEnable()
    {
        for (int i = 0; i < healthBalance.Length; i++)
        {
            healthBalance[i].OnDamage += TakeDamaged;
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < healthBalance.Length; i++)
        {
            healthBalance[i].OnDamage -= TakeDamaged;
        }
    }
    public void TakeDamaged(float damage)
    {
        if(isDead) return;
        currentHealth -= damage;
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
