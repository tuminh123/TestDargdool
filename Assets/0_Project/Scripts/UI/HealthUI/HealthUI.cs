using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class HealthUI : MonoBehaviour
{
    protected HealthBase health;
    protected Image fill;
    protected virtual void Awake()
    {
        fill = GetComponent<Image>();
    }
    protected virtual void Start()
    {
        if (health == null) return;
        health.OnHealthChanged += UpdateBar;
    }
    protected virtual void OnDestroy()
    {
        if (health == null) return;
        health.OnHealthChanged -= UpdateBar;
    }

    protected virtual void UpdateBar(float current, float max)
    {
        fill.fillAmount = current / max;
    }

}
