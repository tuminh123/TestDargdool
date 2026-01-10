using UnityEngine;

public class StatRuntime 
{
    private StatDataConfig config;

    //current base stats
    public string runtimeId { get;private set; }
    public float runtimeHp { get; private set; }
    public float runtimeSpeed { get; private set; }
    public float runtimeDamage { get; private set; }
    public float runtimeCriticalChance { get; private set; }
    public float runtimeCriticalMultiple { get; private set; }


    public StatRuntime(out StatDataBase @base,in StatDataConfig config)
    {
        this.config = config;
        @base = new StatDataBase
        {
            idBase = config.IdConfig,
            hpBase = config.HpConfig,
            speedBase = config.SpeedConfig,
            damageBase = config.DamageConfig,
            criticalChanceBase = config.CriticalChanceConfig,
            criticalMultipleBase = config.CriticalMultipleConfig
        };
    }
    public void Init(in StatDataBase @base)
    {
        this.runtimeId = @base.idBase;
        this.runtimeHp = @base.hpBase;
        this.runtimeSpeed = @base.speedBase;
        this.runtimeDamage = @base.damageBase;
        this.runtimeCriticalChance = @base.criticalChanceBase;
        this.runtimeCriticalMultiple = @base.criticalMultipleBase;
    }
    
}
