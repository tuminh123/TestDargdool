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

    protected override void Awake()
    {
        base.Awake();
        LoadSounds();
    }
    private void Start()
    {
        DataSettings.SoundEnabled = ES3.Load(DataSettings.SOUND_KEY, true);

        
        ApplySetting();
    }
    private void LoadSounds()
    {
        soundDict = new Dictionary<SoundType, AudioClip>();

        foreach (var s in sounds)
        {
            soundDict[s.type] = s.clip;
        }
    }

    public void PlaySound(SoundType type)
    {
        if (!DataSettings.SoundEnabled) return;
        if (!soundDict.ContainsKey(type)) return;

        audioSource.PlayOneShot(soundDict[type]);
    }

    public void ToggleSound()
    {
        DataSettings.SoundEnabled = !DataSettings.SoundEnabled;
        ApplySetting();
    }

    public override void ApplySetting()
    {
        audioSource.mute = !DataSettings.SoundEnabled;
    }
}
