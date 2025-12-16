using UnityEngine;

public static class DataSettings
{
    public const string MUSIC_KEY = "MusicEnabled";
    public const string SOUND_KEY = "SoundEnabled";

    /*public static bool MusicEnabled
    {
        get => PlayerPrefs.GetInt(MUSIC_KEY, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(MUSIC_KEY, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static bool SoundEnabled
    {
        get => PlayerPrefs.GetInt(SOUND_KEY, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(SOUND_KEY, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }*/
    #region ES3 Save
    private static bool musicEnabled = true;
    private static bool soundEnabled = true;
    public static bool MusicEnabled
    {
        get => musicEnabled;
        set
        {
            musicEnabled = value;
            ES3.Save(MUSIC_KEY, value);
        }
    }

    public static bool SoundEnabled
    {
        get => soundEnabled;
        set
        {
            soundEnabled = value;
            ES3.Save(SOUND_KEY, value);
        }
    }
    #endregion
}
