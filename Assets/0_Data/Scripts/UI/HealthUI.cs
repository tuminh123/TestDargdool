using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public HealthBase health;
    public Image fill;

    void Start()
    {
        // Đăng ký event khi máu thay đổi
        health.OnHealthChanged += UpdateBar;
    }
    private void OnDestroy()
    {
        health.OnHealthChanged -= UpdateBar;
    }

    void UpdateBar(float current, float max)
    {
        fill.fillAmount = current / max;
    }

}
