using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyObjectPool : ObjectPoolManager<EnemyAI>
{
    public static EnemyObjectPool Instance { get;private set; }
    public Transform[] pointsSpawm;

    private float nextWaveTimer;
    private int waveIndex;

    [SerializeField] private AnimationCurve enemySpawmAmount;
    [SerializeField] private AnimationCurve enemyHealthAmount;
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        nextWaveTimer -= Time.deltaTime;

        if(nextWaveTimer <= 0)
        {
            nextWaveTimer = 30;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        waveIndex++;

        int spawmEnemyAmount = Mathf.RoundToInt(enemySpawmAmount.Evaluate(waveIndex));

        for (int i = 0; i < spawmEnemyAmount; i++)
        {
            float health = enemyHealthAmount.Evaluate(waveIndex);
            StartCoroutine(SpawnDelayEnemy(health));
        }
    }

    private IEnumerator SpawnDelayEnemy(float health)
    {
        CreateEnemy(health);
        yield return new WaitForSeconds(2f);
    }

    public void CreateEnemy(float maxHealth)
    {
        Debug.Log("Spawn at frame: " + Time.frameCount);

        EnemyAI enemy = Spawn("Enemy", GetRandomPoint().position, Quaternion.identity);
        enemy.SetTriggerBalance(true);
        enemy.healthBase.SetMaxHealth(maxHealth);
        enemy.healthBase.InitHealth();
    }
    public Transform GetRandomPoint()
    {
        int index = Random.Range(0,pointsSpawm.Length);
        return pointsSpawm[index];
    }
}