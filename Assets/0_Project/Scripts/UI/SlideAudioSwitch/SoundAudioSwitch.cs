using UnityEngine;

public class SoundAudioSwitch : SlideAudioSwitch
{
    protected override void LoadState()
    {
        isOn = SingletonManager.Instance.soundManager.SoundEnabled;
    }


    protected override void SaveState()
    {
        SingletonManager.Instance.soundManager.SoundEnabled = isOn;
    }


    protected override void ApplyLogic()
    {
        SingletonManager.Instance.soundManager.SetSound(isOn);
        SingletonManager.Instance.soundManager.PlaySound(SoundType.Click);
    }
}
