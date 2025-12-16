using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    None = 0,
    Click = 1,
    GameFail = 2,
    Crunch = 3,
    Pop = 4,
    WinClap = 5,

}
[System.Serializable]
public struct SoundData
{
    public SoundType type;
    public AudioClip clip;
}
public class SoundManager : AudioManager
{
    [SerializeField] private List<SoundData> sounds;

    private Dictionary<SoundType, AudioClip> soundDict;

    protected override bool IsEnabled => SoundEnabled;


    protected override void Awake()
    {
        base.Awake();
        LoadSounds();
    }


    private void LoadSounds()
    {
        soundDict = new Dictionary<SoundType, AudioClip>();
        foreach (var s in sounds)
        {
            if (!soundDict.ContainsKey(s.type))
                soundDict.Add(s.type, s.clip);
        }
    }


    public void PlaySound(SoundType type)
    {
        if (!soundDict.TryGetValue(type, out var clip)) return;
        audioSource.PlayOneShot(clip);
    }


    public void SetSound(bool enabled)
    {
        SoundEnabled = enabled;
        ApplySetting();
    }
}
