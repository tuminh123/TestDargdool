using Unity.VisualScripting;
using UnityEngine;

public abstract class AudioManager : MonoBehaviour
{
    protected AudioSource audioSource;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public abstract void ApplySetting();
}
