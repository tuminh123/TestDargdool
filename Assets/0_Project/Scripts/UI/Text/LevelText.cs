using UnityEngine;

public class LevelText : TextBase
{
    private void Start()
    {
        if (ZenManager.Instance == null || ZenManager.Instance.levelManager == null) return;
        GameEventBus.OnLevelUp += GameEventBus_OnLevelUp;

        if (DataManager.Instance == null) return;
        GameEventBus_OnLevelUp(/*DataManager.Instance.ProgressData.level*/ZenManager.Instance.levelManager.Level);
    }

    private void OnDestroy()
    {
        GameEventBus.OnLevelUp -= GameEventBus_OnLevelUp;
    }
    private void GameEventBus_OnLevelUp(int level)
    {
        string text = $"Level : {level}";
        UpdateText(text);
    }
}
