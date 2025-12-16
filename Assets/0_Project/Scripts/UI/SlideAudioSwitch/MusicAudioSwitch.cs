using UnityEngine;

public class MusicAudioSwitch : SlideAudioSwitch
{
    protected override void LoadState()
    {
        isOn = SingletonManager.Instance.musicManager.MusicEnabled;
    }


    protected override void SaveState()
    {
        SingletonManager.Instance.musicManager.MusicEnabled = isOn;
    }


    protected override void ApplyLogic()
    {
        SingletonManager.Instance.musicManager.SetMusic(isOn);
    }
}
