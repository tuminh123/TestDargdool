using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    [SerializeField] private string waveName;
    [SerializeField] private List<EnemyData> enemies;
    [SerializeField] private float waveDelay = 2f;     // Thời gian chờ trước wave tiếp theo
    public List<EnemyData> Enemies => enemies;
    public float WaveDelay => waveDelay;
    public string WaveName => waveName;

    /// <summary>
    /// Tổng thời gian của wave (spawn + delay)
    /// Dùng cho Wave Progress Bar (PvZ style)
    /// </summary>
    public float GetTotalDuration()
    {
        float totalTime = 0f;

        foreach (var enemy in enemies)
        {
            totalTime += enemy.Count * enemy.SpawnInterval;
        }

        totalTime += waveDelay;
        return totalTime;
    }
}
[System.Serializable]
public class EnemyData
{
    [Header("Wave attibute")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int count;
    [SerializeField] private float spawnInterval = 0.5f;// Delay giữa mỗi enemy

    [Space]
    [Header("Wave Buff")]
    public float healthPerWave = 0.2f; // tăng 20% HP mỗi wave
    public float damagePerWave = 0.1f; // tăng 10% damage mỗi wave

   /* [Space]
    [Header("Wave Marker (PvZ Style)")]
    public bool isMajorWave;   // Wave có cờ / đầu lâu
    public bool isBoss;
*/
    //get
    public GameObject EnemyPrefab=>enemyPrefab;
    public float SpawnInterval => spawnInterval;
    public int Count => count;
}
