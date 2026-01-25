using Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private List<Wave> waves;
    [Space]
    [Space]
    [Header("Auto spawn")]
    [SerializeField] private bool useAutoWave;
    [SerializeField] private AutoWaveConfig autoWaveConfig;
    /*[SerializeField] private bool isSpawn;*/

    private int currentWaveIndex = 0;
    private int totalEnemiesInWave;
    private int aliveEnemies;

    private void Start()
    {
        if (useAutoWave && autoWaveConfig != null)
        {
            waves = AutoWaveGenerator.Generate(autoWaveConfig);
        }


        WaveSapwning();

        //TÍNH TỔNG ENEMY TRONG WAVE
        Wave wave = waves[currentWaveIndex];
        ResetEnemyCount(wave);


        GameEventBus.OnGameRestart += OnGameRestart;
        GameEventBus.OnEnemyDead += OnEnemyDead;
    }

    private void OnDestroy()
    {
        GameEventBus.OnGameRestart -= OnGameRestart;
        GameEventBus.OnEnemyDead -= OnEnemyDead;
    }

    private void OnEnemyDead()
    {
        aliveEnemies = Mathf.Max(0, aliveEnemies - 1);

        Global.Send(new SignalEnemyCount
        {
            Current = aliveEnemies,
            Total = totalEnemiesInWave
        });
    }

    private void OnGameRestart()
    {
        WaveSapwning();
    }
    private void ResetEnemyCount(Wave wave)
    {
        totalEnemiesInWave = 0;

        foreach (var enemyData in wave.Enemies)
        {
            totalEnemiesInWave += enemyData.Count;
        }

        aliveEnemies = totalEnemiesInWave;

        Global.Send(new SignalEnemyCount
        {
            Current = aliveEnemies,
            Total = totalEnemiesInWave
        });
    }
    public void WaveSapwning()
    {
        Global.Send(new SignalTextWave() { WaveIndex = currentWaveIndex + 1 });
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        //isSpawn = false;
        yield return new WaitForSeconds(1f);

        //isSpawn = true;
        while (currentWaveIndex < waves.Count)
        {
            Wave wave = waves[currentWaveIndex];

            ResetEnemyCount(wave);

            // Spawn toàn bộ enemy của wave
            foreach (var enemyData in wave.Enemies)
            {
                for (int i = 0; i < enemyData.Count; i++)
                {
                    Vector3 spawnPoint = SpawnPoint.Instance.GetSpawnPos();
                    GameObject enemyObj = SpawnEnemy(enemyData.EnemyPrefab, spawnPoint);
                    ApplyBuffToEnemy(enemyObj, enemyData, currentWaveIndex);
                    yield return new WaitForSeconds(enemyData.SpawnInterval);
                }
            }

            // ⚠️ CHỜ TẤT CẢ ENEMY CHẾT MỚI QUA WAVE TIẾP THEO
            while (/*IsAnyEnemyAlive()*/aliveEnemies > 0)
            {
                yield return new WaitForSeconds(0.2f);
            }

            // Delay giữa các wave (nếu có)
            yield return new WaitForSeconds(wave.WaveDelay);

            currentWaveIndex++;
            Global.Send(new SignalTextWave() { WaveIndex = currentWaveIndex + 1 });
        }

        OnAllWavesCompleted();
    }


    public void ClearAllEnemies()
    {
        // Dừng spawn ngay lập tức
        ResetWave();

        // Xóa toàn bộ enemy đang có trên scene
        EnemyAI[] enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
    }

    public void ResetWave()
    {
        StopAllCoroutines();
        currentWaveIndex = 0;
        //isSpawn = false;
    }

    private bool IsAnyEnemyAlive()
    {
        return FindObjectsByType<EnemyAI>(FindObjectsSortMode.None).Length > 0;
    }
    private void OnAllWavesCompleted()
    {
        Debug.Log("Tất cả wave đã hoàn thành.");
        // Bạn có thể làm gì đó ở đây, ví dụ:
        // - Bật cửa ra next level
        // - Hiện thông báo
        // - Thay đổi trạng thái game
        GameEventBus.RaiseGameWin();
    }
    private void ApplyBuffToEnemy(GameObject enemyObj, EnemyData enemyData, int waveIndex)
    {
        EnemyAI enemy = enemyObj.transform.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            float healthMultiplier = 1f + enemyData.healthPerWave * waveIndex;
            float damageMultiplier = 1f + enemyData.damagePerWave * waveIndex;
            float critCMultiplier = 1f + enemyData.critChancePerWave * waveIndex;
            float critMMultiplier = 1f + enemyData.critMultiplierPerWave * waveIndex;

            enemy.Buff(healthMultiplier, damageMultiplier,critCMultiplier,critMMultiplier);
        }
    }

    private GameObject SpawnEnemy(GameObject prefab, Vector3 position)
    {
        GameObject enemy = Instantiate(prefab, position, Quaternion.identity);
        //EnemySpawnUtils(enemy);
        return enemy;
    }

    //private static void EnemySpawnUtils(GameObject enemy)
    //{
    //    // 1. Reset physics ngay khi spawn (quan trọng với ragdoll)
    //    Rigidbody2D[] rbs = enemy.GetComponentsInChildren<Rigidbody2D>();
    //    Rigidbody2D rbParent = enemy.GetComponent<Rigidbody2D>();
    //    foreach (var rb in rbs)
    //    {
    //        rb.linearVelocity = Vector2.zero;
    //        rb.angularVelocity = 0f;
    //        rb.Sleep(); // Wake up khi cần
    //    }
    //    rbParent.linearVelocity = Vector2.zero;
    //    rbParent.angularVelocity = 0f;
    //    rbParent.Sleep(); // Wake up khi cần

    //    Collider2D[] colliders = enemy.GetComponentsInChildren<Collider2D>();
    //    Collider2D parentCollider = enemy.GetComponent<Collider2D>();
    //    foreach (var c in colliders)
    //    {
    //        c.enabled = true; // đảm bảo collider hoạt động
    //    }
    //    parentCollider.enabled = true;
    //}

   
}