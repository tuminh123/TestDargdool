

using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealthUI : HealthUI
{
    [SerializeField] private Image damageOverlayImage;
    protected override void Awake()
    {
        base.Awake();
        health = CharacterCtrl.Instance.healthBase;
    }
    protected override void Start()
    {
        base.Start();
        GameEventBus.OnGameRestart += OnGameRestart;
        if (damageOverlayImage == null) return;
        damageOverlayImage.enabled = false;
    }

   

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameEventBus.OnGameRestart -= OnGameRestart;
    }
    private void OnGameRestart()
    {
        OnPlayerSpawned(CharacterCtrl.Instance);
    }
    private void OnPlayerSpawned(CharacterCtrl player)
    {
        if (health != null) health.OnHealthChanged -= UpdateBar;

        health = player.healthBase;
        health.InitHealth();
        UpdateBar(health.CurrentHealth, health.MaxHealth); // Cập nhật ngay lập tức
        damageOverlayImage.enabled = false;
        // Đăng ký lại event
        health.OnHealthChanged += UpdateBar;
    }
    protected override void UpdateBar(float current, float max)
    {
        base.UpdateBar(current, max);

        if (current < 100)
        {
            DamageEffectHandle().Forget();
        }
    }
    private async UniTask DamageEffectHandle()
    {
        try
        {
            damageOverlayImage.enabled = true;
            await UniTask.Delay(2000);
            damageOverlayImage.enabled = false;
        }
        catch (System.Exception)
        {
            //ignore
        }

    }
}
