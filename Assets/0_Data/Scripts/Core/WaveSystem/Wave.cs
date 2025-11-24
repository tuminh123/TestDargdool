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
}
[System.Serializable]
public class EnemyData
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int count;
    [SerializeField] private float spawnInterval = 0.5f;// Delay giữa mỗi enemy

    //get
    public GameObject EnemyPrefab=>enemyPrefab;
    public float SpawnInterval => spawnInterval;
    public int Count => count;
}
