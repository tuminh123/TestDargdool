using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : AudioManager
{
    private void Start()
    {
        DataSettings.MusicEnabled = ES3.Load(DataSettings.MUSIC_KEY,true);
        ApplySetting();
    }
    public void ToggleMusic()
    {
        DataSettings.MusicEnabled = !DataSettings.MusicEnabled;
        ApplySetting();
    }

    public override void ApplySetting()
    {
        audioSource.mute = !DataSettings.MusicEnabled;

        if (DataSettings.MusicEnabled && !audioSource.isPlaying)
            audioSource.Play();
    }
}
