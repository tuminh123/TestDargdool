using UnityEngine;

public class CharacterDamage : DamageBase
{
    private CharacterParent characterParent;

    private void Awake()
    {
        
        characterParent = GetComponentInParent<CharacterParent>();

        damageBase = characterParent.Stats.DamageBase;
    }
    public override bool SenderDamageTo()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, layer);

        foreach (Collider2D collider in colliders)
        {
            if (collider == null) continue;

            Debug.Log(collider.name);

            Balance balance = collider.GetComponent<Balance>();
            if (balance == null) continue;

            CharacterParent characterParent = collider.GetComponentInParent<CharacterParent>();
            if (characterParent == null) continue;

            HealthBase health = characterParent.healthBase;
            if (health == null || health.IsDead) continue;

            DamageHandle(health);
            return true;
        }

        return false;
    }
}
