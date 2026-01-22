using Core;
using System;
using TMPro;
using UnityEngine;

public class LevelText : GameElement ,IReceive<SignalLevelText>
{
    [SerializeField] private TextMeshProUGUI text;

    private void OnLevelUp(int level)
    {
        string textS = $"Level : {level}";
        text.text = textS;
    }

    public void Receive(in SignalLevelText signal)
    {
        OnLevelUp(signal.level);
    }
}
