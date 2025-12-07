using UnityEngine;

public class HealEffect : IEffect
{
    private float healAmount;

    public HealEffect(float amount)
    {
        healAmount = amount;
    }

    public void Apply(GameObject target)
    {
        var health = target.GetComponentInChildren<IHeal>();
        if (health == null) return;
        health.Heal(healAmount);
    }
}
