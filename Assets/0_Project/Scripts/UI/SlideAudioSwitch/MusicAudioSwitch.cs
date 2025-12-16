using UnityEngine;

public class MusicAudioSwitch : SlideAudioSwitch
{
    public override void ApplyLogic()
    {
        SingletonManager.Instance.musicManager.ToggleMusic();
    }

    public override void LoadState()
    {
        isOn = DataSettings.MusicEnabled;
    }

    public override void SaveState()
    {
        DataSettings.MusicEnabled = isOn;
    }
}
