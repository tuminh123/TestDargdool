using UnityEngine;

public class LevelText : TextBase
{
    private void Start()
    {
        if (LevelManager.Instance == null) return;
        GameEventBus.OnLevelUp += GameEventBus_OnLevelUp;

        if (DataManager.Instance == null) return;
        GameEventBus_OnLevelUp(LevelManager.Instance.Level);
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
