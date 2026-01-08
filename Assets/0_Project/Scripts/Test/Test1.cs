using Core;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Test1 : GameElement, IReceive<SignalGoldReceived>,IReceive<SignalEnemyDie>
{
    [SerializeField] private int goldCount = 0;
    [SerializeField] private int enemyCount = 0;


    public void Receive(in SignalGoldReceived signal)
    {
        goldCount += signal.receivedGoldCount;
        //Debug.Log(goldCount);
    }

    public void Receive(in SignalEnemyDie signal)
    {
        enemyCount += signal.enemyDieCount;
    }
}