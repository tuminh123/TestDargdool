using System.Collections.Generic;
using UnityEngine;

public static class AutoWaveGenerator
{
    public static List<Wave> Generate(AutoWaveConfig config)
    {
        List<Wave> waves = new List<Wave>();

        for (int i = 0; i < config.totalWaves; i++)
        {
            int enemyCount = config.startCount + i * config.increasePerWave;

            EnemyData enemyData = new EnemyData(
                config.enemyPrefab,
                enemyCount,
                config.spawnInterval,
                config.healthPerWave,
                config.damagePerWave,
                config.critChancePerWave,
                config.critMultiplierPerWave
            );

            Wave wave = new Wave(
                $"Wave {i + 1}",
                new List<EnemyData> { enemyData },
                config.waveDelay
            );

            waves.Add(wave);
        }

        return waves;
    }
}
