using UnityEngine;

public class SoundAudioSwitch : SlideAudioSwitch
{
    public override void ApplyLogic()
    {
        SingletonManager.Instance.soundManager.ToggleSound();
    }

    public override void LoadState()
    {
        isOn = DataSettings.SoundEnabled;
    }

    public override void SaveState()
    {
        DataSettings.SoundEnabled = isOn;
    }
}
