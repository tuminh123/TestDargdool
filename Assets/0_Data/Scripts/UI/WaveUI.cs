using Core;
using System.Collections;
using TMPro;
using UnityEngine;

public class WaveUI : GameElement,IReceive<SignalTextWave>
{
    [SerializeField] private TextMeshProUGUI _textWave;

    public void Receive(in SignalTextWave signal)
    {
        UpdateTextWave(signal.WaveIndex);
    }
    public void UpdateTextWave(int wave)
    {
        _textWave.text = $"Wave : {wave}";
    }
}
