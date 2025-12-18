

using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealthUI : HealthUI
{
    [SerializeField] private Image damageOverlayImage;
    CharacterCtrl player;
    protected override void Awake()
    {
        base.Awake();
        bool flowControl = GetPlayer();
        if (!flowControl)
        {
            return;
        }
    }

    private bool GetPlayer()
    {
        player = FindFirstObjectByType<CharacterCtrl>();

        if (player == null) return false;
        health = player.GetComponentInChildren<HealthBase>();
        if (health == null) return false;

        damageOverlayImage.gameObject.SetActive(false);
        return true;
    }

    protected override void Start()
    {
        base.Start();
        GameEventBus.OnGameRestart += OnGameRestart;
        GameEventBus.OnPlayerRegeneration += GameEventBus_OnPlayerRegeneration;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameEventBus.OnGameRestart -= OnGameRestart;
        GameEventBus.OnPlayerRegeneration -= GameEventBus_OnPlayerRegeneration;
    }
    private void OnGameRestart()
    {
        OnPlayerSpawned();
    }
    private void GameEventBus_OnPlayerRegeneration()
    {
        OnPlayerSpawned();
    }
    private void OnPlayerSpawned()
    {
        if (health == null) return;

        if(!GetPlayer()) return;
        
        health.InitHealth();
        UpdateBar(health.CurrentHealth, health.MaxHealth); // Cập nhật ngay lập tức
        damageOverlayImage.gameObject.SetActive(false);
        // Đăng ký lại event
        health.OnHealthChanged += UpdateBar;
    }
    protected override void UpdateBar(float current, float max)
    {
        base.UpdateBar(current, max);

        if (current <= max * 0.5f)
        {
            ShowDamageOverlay().Forget();
        }
        else
        {
            damageOverlayImage.gameObject.SetActive(false);
        }
    }
    private async UniTaskVoid ShowDamageOverlay()
    {
        try
        {
            if (damageOverlayImage.gameObject.activeSelf) return;

            damageOverlayImage.gameObject.SetActive(true);
            await UniTask.Delay(2000, cancellationToken: this.GetCancellationTokenOnDestroy());
            damageOverlayImage.gameObject.SetActive(false);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
}
