using UnityEngine;
using System;

[Serializable]
public class PlayerProgressData
{
    public int level;
    public int currentExp;

    public PlayerProgressData()
    {
        level = 1;
        currentExp = 0;
    }

    public void Reset()
    {
        level = 1;
        currentExp = 0;
    }
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private SimpleExpCurve expCurve;

    private PlayerProgressData progress;

    public int Level => progress.level;
    public int CurrentExp => progress.currentExp;
    public int ExpToNext => expCurve.GetExpToNextLevel(progress.level);

    public event Action<int> OnLevelUp;
    public event Action<int, int> OnExpChanged;

    public void Init(PlayerProgressData data)
    {
        progress = data;
        OnExpChanged?.Invoke(progress.currentExp, ExpToNext);
    }

    public void AddExp(int amount)
    {
        progress.currentExp += amount;
        OnExpChanged?.Invoke(progress.currentExp, ExpToNext);

        while (progress.currentExp >= ExpToNext)
        {
            progress.currentExp -= ExpToNext;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        progress.level++;
        //Debug.Log($"LEVEL UP ? {progress.level}");
        OnLevelUp?.Invoke(progress.level);
    }
}
