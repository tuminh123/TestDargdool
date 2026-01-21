using System;
using UnityEngine;

public class LevelText : TextBase
{
    private void Start()
    {
        if (LevelManager.Instance == null || LevelManager.Instance.RuntimeData == null) return;
        LevelManager.Instance.RuntimeData.OnLevelChange += OnLevelUp;
        GameEventBus.OnGameRestart += GameEventBus_OnGameRestart;
    }

    private void OnDestroy()
    {
        if (LevelManager.Instance == null || LevelManager.Instance.RuntimeData == null) return;
        LevelManager.Instance.RuntimeData.OnLevelChange -= OnLevelUp;
        GameEventBus.OnGameRestart -= GameEventBus_OnGameRestart;
    }

    private void GameEventBus_OnGameRestart()
    {
        OnLevelUp(LevelManager.Instance.Level);
    }

    private void OnLevelUp(int level)
    {
        string text = $"Level : {level}";
        UpdateText(text);
    }
}
