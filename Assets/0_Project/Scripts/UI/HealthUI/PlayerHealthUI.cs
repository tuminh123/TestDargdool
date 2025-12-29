

using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealthUI : HealthUI
{
    [SerializeField] private Image damageOverlayImage;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private ExpBarUI expBarUI;
    protected override void Start()
    {
        GetPlayer();
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
        GetPlayer();
        health.OnHealthChanged -= UpdateBar;

        health.InitHealth();
        UpdateBar(health.CurrentHealth, health.MaxHealth); // Cập nhật ngay lập tức
        damageOverlayImage.gameObject.SetActive(false);
        // Đăng ký lại event
        health.OnHealthChanged += UpdateBar;
    }
    protected override void UpdateBar(float current, float max)
    {
        base.UpdateBar(current, max);

        if (text == null || damageOverlayImage == null) return;

        text.text = $"{current} / {max}";

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
        catch /*(System.Exception e)*/
        {
            //Debug.LogException(e);
        }
    }
    private void GetPlayer()
    {
        CharacterCtrl player = CharacterCtrl.Instance;

        if (player == null) return;
        health = player.healthBase;
        if (health == null) return;

        damageOverlayImage.gameObject.SetActive(false);
    }
}
