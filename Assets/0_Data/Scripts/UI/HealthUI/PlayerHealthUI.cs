using UnityEngine;
public class PlayerHealthUI : HealthUI
{
    private void OnEnable()
    {
        GameEventBus.OnPlayerSpawned += OnPlayerSpawned;
    }

    private void OnDisable()
    {
        GameEventBus.OnPlayerSpawned -= OnPlayerSpawned;
    }

    private void OnPlayerSpawned(CharacterCtrl player)
    {
        if (health != null)health.OnHealthChanged -= UpdateBar;

        health = player.healthBase;
        health.InitHealth();
        UpdateBar(health.CurrentHealth, health.MaxHealth); // Cập nhật ngay lập tức

        // Đăng ký lại event
        health.OnHealthChanged += UpdateBar;
    }
}
