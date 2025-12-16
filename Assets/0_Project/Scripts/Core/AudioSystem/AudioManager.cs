using Unity.VisualScripting;
using UnityEngine;

public abstract class AudioManager : MonoBehaviour
{
    protected AudioSource audioSource;

    public const string MUSIC_KEY = "MusicEnabled";
    public const string SOUND_KEY = "SoundEnabled";

    public bool MusicEnabled
    {
        get => PlayerPrefs.GetInt(MUSIC_KEY, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(MUSIC_KEY, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public bool SoundEnabled
    {
        get => PlayerPrefs.GetInt(SOUND_KEY, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(SOUND_KEY, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    protected abstract bool IsEnabled { get; }

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        ApplySetting();
    }


    public virtual void ApplySetting()
    {
        audioSource.mute = !IsEnabled;
    }
}
