

using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealthUI : HealthUI
{
    [SerializeField] private Image damageOverlayImage;
    [SerializeField] private CharacterCtrl playerCtrl;
    protected override void Awake()
    {
        base.Awake();
        health = playerCtrl.healthBase;
    }
    protected override void Start()
    {
        base.Start();
        if (damageOverlayImage == null) return;
        damageOverlayImage.enabled = false;
    }

    private void OnPlayerSpawned(CharacterCtrl player)
    {
        if (health != null)health.OnHealthChanged -= UpdateBar;

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

        if(current < 100)
        {
            DamageEffectHandle().Forget();
        }
    }
    private async UniTask DamageEffectHandle()
    {
        damageOverlayImage.enabled = true;
        await UniTask.Delay(2000);
        damageOverlayImage.enabled = false;
    }
    
}
