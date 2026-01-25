using UnityEngine;

[CreateAssetMenu(menuName = "Wave/Auto Wave Config")]
public class AutoWaveConfig : ScriptableObject
{
    public GameObject enemyPrefab;

    public int totalWaves = 10;

    [Header("Enemy Count")]
    public int startCount = 5;
    public int increasePerWave = 5;

    [Header("Timing")]
    public float spawnInterval = 2f;
    public float waveDelay = 2f;

    [Header("Buff")]
    public float healthPerWave = 0.2f;
    public float damagePerWave = 0.1f;

    [Header("Critical")]
    public float critChancePerWave = 0.05f;     // 5% mỗi wave
    public float critMultiplierPerWave = 0.2f;  // +0.2 damage
}
