using Core;
using TMPro;
using UnityEngine;

public class EnemyCountUI : GameElement, IReceive<SignalEnemyCount>
{
    [SerializeField] private TMP_Text enemyText;

    public void Receive(in SignalEnemyCount signal)
    {
        OnEnemyCountChanged(signal);
    }

    private void OnEnemyCountChanged(SignalEnemyCount signal)
    {
        enemyText.text = $"Enemy: {signal.Current}/{signal.Total}";
    }
}
