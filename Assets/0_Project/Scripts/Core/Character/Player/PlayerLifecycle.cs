using System;
using UnityEngine;

public class PlayerLifecycle : MonoBehaviour
{
    public static PlayerLifecycle Instance;

    public CharacterCtrl Player { get; private set; }

    public event Action<CharacterCtrl> OnPlayerSpawned;
    public event Action OnPlayerDestroyed;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Spawn(CharacterCtrl prefab, Vector3 position)
    {
        DestroyCurrent();

        Player = Instantiate(prefab, position, Quaternion.identity);
        OnPlayerSpawned?.Invoke(Player);
    }

    public void DestroyCurrent()
    {
        if (Player == null) return;

        OnPlayerDestroyed?.Invoke();
        Destroy(Player.gameObject);
        Player = null;
    }
}
