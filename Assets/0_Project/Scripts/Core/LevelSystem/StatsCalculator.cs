using UnityEngine;
[System.Serializable]
public class LevelStatModifier
{
    [Header("Health")]
    public float hpPercentPerLevel = 0.1f; // +10%

    [Header("Damage")]
    public float damagePercentPerLevel = 0.08f; // +8%

    [Header("Crit Chance")]
    public float critChanceFlatPerLevel = 0.01f; // +1% mỗi level

    [Header("Crit Multiplier")]
    public float critMultiplierFlatPerLevel = 0.05f; // +5% mỗi level
}

public static class StatsCalculator
{
    public static void ApplyLevelStats(
        Stats runtimeStats,
        int level,
        LevelStatModifier modifier)
    {
        int lv = Mathf.Max(level - 1, 0);

        // Health
        float hp = runtimeStats.MaxHealth * (1f + modifier.hpPercentPerLevel * lv);

        // Damage
        float damage = runtimeStats.DamageBase * (1f + modifier.damagePercentPerLevel * lv);

        // Crit Chance (Clamp để tránh 100%)
        float critChance = runtimeStats.CritChane + modifier.critChanceFlatPerLevel * lv;
        critChance = Mathf.Clamp01(critChance);

        // Crit Multiplier (>= 1)
        float critMultiplier = runtimeStats.CritMultiplier + modifier.critMultiplierFlatPerLevel * lv;
        critMultiplier = Mathf.Max(1f, critMultiplier);

        runtimeStats.SetMaxHealth(hp);
        runtimeStats.SetDamageBase(damage);
        runtimeStats.SetCritChane(critChance);
        runtimeStats.SetCritMultiplier(critMultiplier);
    }
}
