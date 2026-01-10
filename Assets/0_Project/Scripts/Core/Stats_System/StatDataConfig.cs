using UnityEngine;

[CreateAssetMenu(fileName = "StatDataConfig", menuName = "Stats System/Stat Data Config", order = 1)]
public class StatDataConfig : ScriptableObject
{
    [SerializeField] private string idConfig;
    [SerializeField] private float hpConfig;
    [SerializeField] private float speedConfig;
    [SerializeField] private float damageConfig;
    [SerializeField] private float criticalChanceConfig;
    [SerializeField] private float criticalMultipleConfig;

    public string IdConfig => idConfig;
    public float HpConfig => hpConfig;
    public float SpeedConfig => speedConfig;
    public float DamageConfig => damageConfig;
    public float CriticalChanceConfig => criticalChanceConfig;
    public float CriticalMultipleConfig => criticalMultipleConfig;
}
