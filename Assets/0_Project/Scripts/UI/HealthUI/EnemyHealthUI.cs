using UnityEngine;

public class EnemyHealthUI : HealthUI
{
    [SerializeField] private HealthBase healthPrefab;
    protected override void Start()
    {
        health = healthPrefab;
        base.Start();
    }
}
