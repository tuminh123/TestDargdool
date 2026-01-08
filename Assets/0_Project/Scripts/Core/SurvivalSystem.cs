using Core;
using UnityEngine;

public class SurvivalSystem : GameElement, IReceive<SignalGoldReceived>, IReceive<SignalEnemyDie>, IReceive<SignalTextWave>
{
    [SerializeField] private int goldCount = 0;
    [SerializeField] private int enemyCount = 0;
    [SerializeField] private int waveCount = 0;

    //get
    public int GoldCount => goldCount;
    public int EnemyCount => enemyCount;
    public int WaveCount => waveCount;
    public void Receive(in SignalGoldReceived signal)
    {
        goldCount += signal.receivedGoldCount;
    }

    public void Receive(in SignalEnemyDie signal)
    {
        enemyCount += signal.enemyDieCount;
    }

    public void Receive(in SignalTextWave signal)
    {
        waveCount = signal.WaveIndex-1;
    }
}
