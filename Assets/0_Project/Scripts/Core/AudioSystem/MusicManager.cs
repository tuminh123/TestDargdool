using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : AudioManager
{
    protected override bool IsEnabled => MusicEnabled;

    protected override void Awake()
    {
        base.Awake();
        if (IsEnabled && !audioSource.isPlaying)
            audioSource.Play();
    }


    public void SetMusic(bool enabled)
    {
        MusicEnabled = enabled;
        ApplySetting();


        if (enabled && !audioSource.isPlaying)
            audioSource.Play();
    }
}
